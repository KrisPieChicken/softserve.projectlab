namespace API.Services.IntAdministration;


public interface ICookieService
{
    void SetAccessToken(string token, TimeSpan lifetime);
    void DeleteAccessToken();
}
