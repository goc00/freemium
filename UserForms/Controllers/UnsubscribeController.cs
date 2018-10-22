using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserForms.Controllers
{
    public class UnsubscribeController : Controller
    {
        // GET: Unsubscribe
        public ActionResult Index(String id)
        {
            ViewBag.Step = id;
            return View();
        }
    }
}