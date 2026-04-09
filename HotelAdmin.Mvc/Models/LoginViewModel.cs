namespace HotelAdmin.Mvc.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; } 
        public string Password { get; set; } 
    }
    public class AuthResponse
    {
        public string Token { get; set; }
    }
}
