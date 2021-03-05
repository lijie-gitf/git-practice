using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebCore.Filter.PermissionFilter
{
    public class PermissionFilterAttribute:ActionFilterAttribute
    {
        /// <summary>
        /// 权限集合
        /// </summary>
        internal static IList<UserPermission> _permissions;

        /// <summary>
        /// 权限相关配置
        /// </summary>

        private readonly PermissionOption _permissionOption;

        public PermissionFilterAttribute(PermissionOption permissionOption)
        {
            _permissionOption=permissionOption;
            _permissions = permissionOption.UserPerssions;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string queryUrl = context.HttpContext.Request.Path.Value.ToLower();
            if (!context.HttpContext.User.Identity.IsAuthenticated && (queryUrl!="/" && !queryUrl.Contains("account")))
            {
                //未登录跳转到无权限页面
                context.HttpContext.Response.Redirect(_permissionOption.NoPermissionAction);
            }
         
            bool isDefind = false;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //以后可对具体方法过滤
            var actionName = controllerActionDescriptor.ActionName;
            if(controllerActionDescriptor!=null)
            {
                //如果方法标记了无须验证
                isDefind = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(NoPermissionRequiredAttribute)));
            }
            if (isDefind)
            {
                return;
            }

            ///页面是否在授权内
            if (_permissions.GroupBy(p => p.Url).Where(p => p.Key.ToLower().Equals(queryUrl)).Count() > 0)
            {
                //获取用户角色
                string userCode = context.HttpContext.User.Claims.Single(p => p.Type.Equals(ClaimTypes.Role)).Value;

                //判断角色是否有页面的使用权限
                if (!_permissions.Any(w => w.RoleCode.ToLower().Equals(userCode.ToLower()) && w.Url.ToLower().Equals(queryUrl)))
                {
                    context.HttpContext.Response.Redirect(_permissionOption.NoPermissionAction);
                }
            }
            base.OnActionExecuting(context);

        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

    }
}
