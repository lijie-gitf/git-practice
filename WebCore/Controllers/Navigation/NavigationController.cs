using CoreModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Controllers.Navigation
{
    public class NavigationController : BaseController<object>
    {
       [HttpPost]
        public IActionResult GetMenuByRole()
        {
            List<MenuToDo> ResultDaTo =new List<MenuToDo>();
            Menu meitem = new Menu() { id="1", text = "用户管理", url = "/Home/Index" };
            MenuToDo MenuToDo = new MenuToDo() { id = "0", text = "系统管理", url = "" };
            MenuToDo.menus = new List<Menu>();
            MenuToDo.menus.Add(meitem);
            ResultDaTo.Add(MenuToDo);

            return Json(new { Data = ResultDaTo, code = 0 });
        }
    }
}
