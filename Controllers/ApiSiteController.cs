using fliper.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace fliper.Controllers
{
    [RoutePrefix("Api")]
    [Authorize]
    public class ApiSiteController : ApiController
    {

        fliperEntities db = new fliperEntities();

        [Route("next")]
        [HttpPost]
        public IHttpActionResult next(PhotoNext model)
        {
            var getUserId = db.AspNetUsers.Where(a => a.UserName == model.namewho).FirstOrDefault().Id;

            var result = db.Photos.Where(a => a.kogo == getUserId && a.id < model.lastid).OrderByDescending(a => a.id).Take(1);

            return Ok(result);
        }


        [Route("prev")]
        [HttpPost]
        public IHttpActionResult prev(PhotoNext model)
        {
            var getUserId = db.AspNetUsers.Where(a => a.UserName == model.namewho).FirstOrDefault().Id;

            var result = db.Photos.Where(a => a.kogo == getUserId && a.id > model.lastid).Take(1);

            return Ok(result);
        }

        [Route("fajnyprofil")]
        [HttpPost]
        public IHttpActionResult fajnyprofil(PhotoPolub model)
        {

            var odkogo = User.Identity.GetUserId();


            var dlakogo = db.AspNetUsers.Where(a => a.UserName == model.DlaKogo).FirstOrDefault().Id;


            var IsAlreadyLiked = db.polubienia.Any(a => a.dlakogo == dlakogo && a.odkogo == odkogo);

            if (odkogo != dlakogo && IsAlreadyLiked == false)
            {
                var polubienie = new polubienia();
                polubienie.dlakogo = dlakogo;
                polubienie.odkogo = odkogo;
                polubienie.IsShowed = false;

                db.polubienia.Add(polubienie);
                db.SaveChanges();
            }
            else
            {
                return Ok("Juz dales ze to fajny profil");
            }

            return Ok("Finished");
        }


        [Route("AddYourGuid")]
        [HttpPost]
        public IHttpActionResult AddYourGuid(AddGuid model)
        {

            var YourID = User.Identity.GetUserId();


            var YourUser = db.AspNetUsers.Where(a => a.Id == YourID).FirstOrDefault();

            YourUser.GuidChat = model.Guid;
            db.SaveChanges();

            return Ok("Updated");
        }

        [Route("IsGuidNull")]
        [HttpGet]
        public IHttpActionResult IsGuidNull()
        {
            var YourID = User.Identity.GetUserId();


            var YourUser = db.AspNetUsers.Where(a => a.Id == YourID).FirstOrDefault();
            if (String.IsNullOrEmpty(YourUser.GuidChat))
            {
                return Ok("neeedtoaddnew");
            }
            else
            {
                return Ok("alreasyhas");
            }
        }


        [Route("SendMsg")]
        [HttpPost]
        public IHttpActionResult SendMsg(AddMsg model)
        {
            var YourID = User.Identity.GetUserId();


            var msg = new Wiadomosci();
            msg.odkogo = YourID;
            msg.dokogo = model.DoKogo;
            msg.tresc = model.Text;
            msg.isReaded = false;

            db.Wiadomosci.Add(msg);
            //added





            var first = db.WiadomosciMenu.Where(a => a.odkogo == YourID && a.dokogo == model.DoKogo).FirstOrDefault();

            if (first == null)
            {
                var msgMenu = new WiadomosciMenu();


                msgMenu.odkogo = YourID;
                msgMenu.dokogo = model.DoKogo;
                msgMenu.data = DateTime.Now;
                msgMenu.isReaded = true;

                db.WiadomosciMenu.Add(msgMenu);
            }
            else
            {
                first.data = DateTime.Now;
                first.isReaded = true;
                
            }

            var second = db.WiadomosciMenu.Where(a => a.odkogo == model.DoKogo && a.dokogo == YourID).FirstOrDefault();

            if (second == null)
            {
                var msgMenu = new WiadomosciMenu();


                msgMenu.odkogo = model.DoKogo;
                msgMenu.dokogo = YourID;
                msgMenu.data = DateTime.Now;
                msgMenu.isReaded = false;

                db.WiadomosciMenu.Add(msgMenu);
            }
            else
            {
                second.data = DateTime.Now;
                second.isReaded = false;
            }


            //var second = db.WiadomosciMenu.Where(a=> a.odkogo == model.DoKogo && a.dokogo == YourID).FirstOrDefault();








            db.SaveChanges();

            return Ok("Wiadmosc doszla");

        }

        [Route("GetMsg")]
        [HttpPost]
        public IHttpActionResult GetMsg(AddMsg model)
        {
            var YourID = User.Identity.GetUserId();

            var Msg = db.Wiadomosci.Where(a => a.odkogo == YourID && a.dokogo == model.DoKogo ||
            a.odkogo == model.DoKogo && a.dokogo == YourID).OrderByDescending(b => b.id).Take(1).FirstOrDefault();

            return Ok(Msg);

        }

        [Route("GetMsgMenu")]
        [HttpPost]
        public IHttpActionResult GetMsgMenu(AddMsg model)
        {
            var YourID = User.Identity.GetUserId();


            var wiadmosciMenu = (from menu in db.WiadomosciMenu
                                  join users in db.AspNetUsers on menu.dokogo equals users.Id
                                  where (menu.odkogo == YourID)
                                  orderby menu.data descending
                                  select new WiadomosciMenuLinq {Nazwa = users.UserName,Id= menu.id });
            foreach (var item in wiadmosciMenu)
            {
                var record = db.WiadomosciMenu.Where(a => a.id == item.Id).FirstOrDefault();
                record.isReaded = true;
            }
            db.SaveChanges();
            

            return Ok(wiadmosciMenu);
        }
    }
}
