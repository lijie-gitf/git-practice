using CoreCommon;
using CoreCommon.PushMessage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Controllers.Home
{
    public class HomeController : Controller
    {
        private IPushMessageService _pushMessageService;
        public HomeController(IPushMessageService pushMessageService)
        {
            _pushMessageService = pushMessageService;
        }
        public IActionResult Index()
        {
            _pushMessageService.PushMessage<string>(new MessageModel<string> {data="测试推送消息队列" });
            return View();
        }
    }
}
