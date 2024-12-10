using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
namespace Marathon.Common
{
    public class CustomActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly ILogger<CustomActionFilterAttribute> _ILogger;
        public CustomActionFilterAttribute(ILogger<CustomActionFilterAttribute> iLogger)
        {
            this._ILogger = iLogger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var para = context.HttpContext.Request.QueryString.Value;
            var controllerName = context.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ActionDescriptor.RouteValues["action"];
            _ILogger.LogInformation($"执行{controllerName}控制器--{actionName}方法；参数为：{para}");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var result = System.Text.Json.JsonSerializer.Serialize(context.Result);
            // var controllerName = context.HttpContext.GetRouteValue("controller");
            var controllerName = context.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ActionDescriptor.RouteValues["action"];
            _ILogger.LogInformation($"执行{controllerName}控制器--{actionName}方法:执行结果为：{result}");
        }
    }
}
