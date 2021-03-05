using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Controllers.Permission
{
    public class PermissionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
