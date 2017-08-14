using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RushHour.Attributes;

namespace RushHour.Controllers
{
    [CustomAuthorize(Enums.CustomAuthorizeEnum.Everyone)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}