using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelAdmin.Mvc.Filters
{
    public class SuccessMessageFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is RedirectToActionResult)
            {
                var httpContext = context.HttpContext;

                // Only for POST/PUT/DELETE
                if (httpContext.Request.Method != "GET")
                {
                    httpContext.Session.SetString("SuccessMessage", "Operation completed successfully!");
                }
            }
        }
    }
}
