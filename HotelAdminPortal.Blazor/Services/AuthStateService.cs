
namespace HotelAdmin.Services
{
    public class AuthStateService
    {
        private string? _token;
        public string? Token => _token;
        public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
        public string Username { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public void SetToken(string token, string username)
        {
            _token = token;
            Username = username;
            StateChanged?.Invoke();
        }

        public void Logout()
        {
            _token = null;
            Username = string.Empty;
            StateChanged?.Invoke();
        }
    }
}
