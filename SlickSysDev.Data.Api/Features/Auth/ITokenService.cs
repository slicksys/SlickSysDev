using ManagementData.Api.Contracts.Auth;

namespace ManagementData.Api.Features.Auth;

public interface ITokenService
{
    TokenResponse CreateToken(AuthTokenContext context);
}