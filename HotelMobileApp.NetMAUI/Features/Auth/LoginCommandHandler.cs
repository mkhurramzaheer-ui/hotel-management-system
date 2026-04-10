using HotelMobileApp.NetMAUI.Services;
using MediatR;

namespace HotelMobileApp.NetMAUI.Features.Auth;

public sealed class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IAuthService _authService = authService;

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResult(false, "Username and password are required.");
        }

        try
        {
            var isSuccess = await _authService.LoginAsync(request.Username, request.Password, cancellationToken);
            return isSuccess
                ? new LoginResult(true)
                : new LoginResult(false, "Login failed. Please check your username and password.");
        }
        catch (HttpRequestException ex)
        {
            return new LoginResult(false, ex.Message);
        }
    }
}
