using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;



namespace WebCore.Controllers.Account
{
    public class AccountController :Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName,string passWord)
        {
            var result = new { code = "0", message = "", url = "" };
            if (string.IsNullOrWhiteSpace(userName))
            {

                result = new { code = "1", message = "请填写用户名", url = "" };

            }
            else if (string.IsNullOrWhiteSpace(passWord))
            {
                result = new { code = "1", message = "请填写用户密码", url = "" };

            }
            else
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, userName));
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
              await  HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
                {
                    IsPersistent = false, //true:保持登陆不过期 false:关闭浏览器就过期
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(10),//过期时间
                    AllowRefresh = true  //是否允许重置用户身份信息
                });

            }
       
            return Json(result);

        }
        [HttpPost]
        public async Task<IActionResult> LoginOut()
        {
            await HttpContext.SignOutAsync();
            return Json(new { code = "0", message = "", url = "" });

        }



    }
}
