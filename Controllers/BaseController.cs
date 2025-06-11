using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Boatify.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Nếu chưa có session nhưng có cookie => tự đăng nhập
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")) &&
                Request.Cookies.TryGetValue("UserEmail", out var email))
            {
                HttpContext.Session.SetString("UserEmail", email);
                // Tuỳ chọn: nếu bạn muốn lưu cả role/UserId thì phải truy DB ở đây
            }

            base.OnActionExecuting(context);
        }

        protected void SetSuccess(string message)
        {
            TempData["Success"] = message;
        }

        protected void SetError(string message)
        {
            TempData["Error"] = message;
        }
    }
}
