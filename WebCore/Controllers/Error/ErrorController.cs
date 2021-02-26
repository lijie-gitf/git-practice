using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Models;

namespace WebCore.Controllers.Error
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
          
            return View();
        }
    }
}
