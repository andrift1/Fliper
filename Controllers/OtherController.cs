using fliper.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fliper.Controllers
{
    [Authorize]
    public class OtherController : Controller
    {
        fliperEntities db = new fliperEntities();
        // GET: Other
        [ChildActionOnly]
        public ActionResult Countofmsg()
        {
            var myName = User.Identity.GetUserId();

            var data = db.WiadomosciMenu.Where(a => a.odkogo == myName && a.isReaded == false).Count();

            ViewBag.count = data;

            return PartialView();
        }

        public ActionResult Countofwow()
        {
            var myName = User.Identity.GetUserId();


            var data = db.polubienia.Where(a => a.dlakogo == myName && a.IsShowed == false).Count();

            ViewBag.count = data;

            return PartialView();
        }
    }
}