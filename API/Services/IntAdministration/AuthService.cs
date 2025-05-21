using AutoMapper;
using softserve.projectlabs.Shared.DTOs.User;
using softserve.projectlabs.Shared.Utilities;
using API.Implementations.Domain;
using API.Models.IntAdmin;
using API.Services.IntAdministration;
using softserve.projectlabs.Shared.DTOs.Auth;

namespace API.Services.IntAdmin;

public class AuthService : IAuthService
{
    private readonly UserDomain _userDomain;
    private readonly TokenGenerator _tokenGenerator;
    private readonly ICookieService _cookieSvc;
    private readonly IMapper _mapper;


    public AuthService(UserDomain userDomain,
                       TokenGenerator tokenGenerator,
                       ICookieService cookieSvc,
                       IMapper mapper)
    {
        _userDomain = userDomain;
        _tokenGenerator = tokenGenerator;
        _cookieSvc = cookieSvc;
        _mapper = mapper;
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(UserLoginDto dto)
    {
        var userRes = await _userDomain.AuthenticateAsync(dto.UserEmail, dto.UserPassword);
        if (!userRes.IsSuccess)
            return Result<AuthResponseDto>.Failure(userRes.ErrorMessage);

        var token = _tokenGenerator.GenerateJwt(userRes.Data);
        var life = TimeSpan.FromHours(18);

        _cookieSvc.SetAccessToken(token, life);

        var response = new AuthResponseDto
        {
            AccessToken = token,
            ExpiresIn = (int)life.TotalSeconds
        };

        return Result<AuthResponseDto>.Success(response);
    }

    public Task<Result<bool>> LogoutAsync()
    {
        _cookieSvc.DeleteAccessToken();
        return Task.FromResult(Result<bool>.Success(true));
    }
}
