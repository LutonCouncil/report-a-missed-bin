using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Waste_Bin_Collections.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            HttpCookie xsrfCookie = new HttpCookie("XSRF-TOKEN");
            xsrfCookie.Value = Guid.NewGuid().ToString();
            xsrfCookie.Expires = DateTime.Now.AddHours(1);
            xsrfCookie.HttpOnly = false;

            Response.Cookies.Add(xsrfCookie);

            return View();
        }
    }
}
