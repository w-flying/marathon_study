using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Marathon.Common
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //获取控制器名称
            string controller = context.RouteData.Values["controller"].ToString();
            //获取方法名
            //string action = context.RouteData.Values["action"].ToString();
            byte[] id;
            context.HttpContext.Session.TryGetValue("User", out id);
            if (controller.Equals("Login") || id != null)
            {
                base.OnActionExecuting(context);
                return;
            }
            else
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                //return ;
            }
        }
    }




}
