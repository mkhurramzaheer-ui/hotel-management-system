using MediatR;

namespace HotelMobileApp.NetMAUI.Features.Auth;

public sealed record LoginCommand(string Username, string Password) : IRequest<LoginResult>;

public sealed record LoginResult(bool IsSuccess, string ErrorMessage = "");
