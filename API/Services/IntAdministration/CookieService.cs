namespace API.Services.IntAdministration;
using Microsoft.AspNetCore.Http;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _ctx;

    public CookieService(IHttpContextAccessor ctx)
    {
        _ctx = ctx;
    }

    public void SetAccessToken(string token, TimeSpan lifetime)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.Add(lifetime),
            Path = "/"
        };
        _ctx.HttpContext!.Response.Cookies.Append("jwt", token, options);
    }

    public void DeleteAccessToken()
    {
        _ctx.HttpContext!.Response.Cookies.Delete("jwt", new CookieOptions
        {
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/"
        });
    }
}
