using System.Net;
using System.Text.Json;

namespace HotelAdmin.Mvc.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Store error in TempData (via session)
                context.Session.SetString("ErrorMessage", ex.Message);

                context.Response.Redirect("/Home/Error");
            }
        }
    }
}
