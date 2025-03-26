namespace API.Helpers;

public interface IAccessTokenHelper
{
    string GetAccessToken();
}

public class AccessTokenHelper(IHttpContextAccessor httpContextAccessor) : IAccessTokenHelper
{
    public string GetAccessToken()
    {
        var authHeader = httpContextAccessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault();
        return string.IsNullOrEmpty(authHeader)
            ? string.Empty
            : authHeader.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
    }
}