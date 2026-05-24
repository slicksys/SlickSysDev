namespace ManagementData.Api.Features.Auth;

public static class AuthorizationPolicies
{
    public const string PracticeMember = "PracticeMember";
    public const string PracticeUser = "PracticeUser";
    public const string PracticeAdmin = "PracticeAdmin";
    public const string CurrentUserRoute = "CurrentUserRoute";
    public const string SelfOrPracticeAdmin = "SelfOrPracticeAdmin";
}
