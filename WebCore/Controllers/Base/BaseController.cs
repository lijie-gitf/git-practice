using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Controllers
{
    public class BaseController<T> : Controller where T :class
    {
        public  IActionResult Index()
        {
            return View();
        }
        //public virtual T Get(int id)
        //{ 
           
        //}
    }
}
