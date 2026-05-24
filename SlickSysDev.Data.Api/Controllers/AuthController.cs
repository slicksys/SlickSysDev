using ManagementData.Api.Contracts.Auth;
using ManagementData.Api.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementData.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController(IAuthRepository authRepository, ITokenService tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> CreateToken(
        [FromBody] TokenRequest request,
        CancellationToken cancellationToken)
    {
        var user = await authRepository.ValidateCredentialsAsync(request.UsernameOrEmail, request.Password, cancellationToken);
        if (user is null)
        {
            return Unauthorized();
        }

        IReadOnlyCollection<string> roles = [];

        if (request.PracticeId.HasValue)
        {
            var isAuthorized = await authRepository.IsAuthorizedForPracticeAsync(
                request.PracticeId.Value,
                user.UserId,
                request.RequiredRoleName,
                cancellationToken);

            if (!isAuthorized)
            {
                return Unauthorized();
            }

            roles = await authRepository.GetPracticeRolesAsync(request.PracticeId.Value, user.UserId, cancellationToken);
        }

        var response = tokenService.CreateToken(new AuthTokenContext
        {
            User = user,
            PracticeId = request.PracticeId,
            Roles = roles
        });

        return Ok(response);
    }
}