using CoreBll.UserService;
using CoreModel.EntirtyModel;
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
        private IUserService _service;
        public AccountController(IUserService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userAccount,string passWord)
        {
            var result = new { code = "0", message = "", url = "/Home/Index" };
            if (string.IsNullOrWhiteSpace(userAccount))
            {

                result = new { code = "1", message = "请填写用户名", url = "" };
                return Json(result);
            }
             if (string.IsNullOrWhiteSpace(passWord))
            {
                result = new { code = "1", message = "请填写用户密码", url = "" };
                return Json(result);
            }
       
                Tb_User User = _service.GetUserByAccount(userAccount);
                if (User == null)
                {
                    result = new { code = "2", message = "用户不存在", url = "" };
                    return Json(result);
                }
                if (User.Password != passWord)
                {
                    result = new { code = "2", message = "密码错误", url = "" };
                    return Json(result);
                }
           
           

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, User.Account));
            identity.AddClaim(new Claim(ClaimTypes.Role, User.RoleCode));
            identity.AddClaim(new Claim(ClaimTypes.Gender, User.Sex.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, User.Email??""));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
            {
                IsPersistent = false, //true:保持登陆不过期 false:关闭浏览器就过期
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10),//过期时间
                AllowRefresh = true  //是否允许重置用户身份信息
            });
          
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
