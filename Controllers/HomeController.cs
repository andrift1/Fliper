using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fliper.Models;
using Microsoft.AspNet.Identity;
using fliper.ViewModels;

namespace fliper.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        fliperEntities db = new fliperEntities();


        public ActionResult Index(string name)
        {
            AspNetUsers userInfo;
            var myName = User.Identity.GetUserId();
            var Nickname = db.AspNetUsers.Where(s => s.Id == myName).FirstOrDefault().UserName;

            if (name == null)
            {
                
                userInfo = db.AspNetUsers.Where(a => a.Id == myName).Take(1).FirstOrDefault();

            }
            else
            {
                userInfo = db.AspNetUsers.Where(a => a.UserName == name).Take(1).FirstOrDefault();
            }

            if (userInfo == null)
            {

                return RedirectToAction("NoUser", "Home");
            }

            var dateOfBirth = userInfo.BornOfDate;
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;


            var photos = db.Photos.Where(e => e.kogo == userInfo.Id).OrderByDescending(sd => sd.id).Take(6).ToList();
            //bool IsYourProfile = name == Nickname;



            bool IsYourProfile;
            if (name == null)
            {
                IsYourProfile = true;
            }
            else
            {
                IsYourProfile = name == Nickname;
            }


            bool IsAlreadyLiked = true;
            if (!IsYourProfile)
            {
                var odkogo = User.Identity.GetUserId();
                var dlakogo = db.AspNetUsers.Where(a => a.UserName == name).FirstOrDefault().Id;


                IsAlreadyLiked = db.polubienia.Any(a => a.dlakogo == dlakogo && a.odkogo == myName);
            }


            var zainteresowania = from zain in db.zainteresowania
                                  join zapisani in db.Zapisani on zain.id equals zapisani.jakieZainteresowanie
                                  where zapisani.kogo == userInfo.Id
                                  orderby zapisani.id descending
                                  select zain;
            //   select new zainteresowania { nazwa = zain.nazwa, zdjecie = zain.zdjecie};

            var profileInfo = db.ProfileInfo.Where(a => a.userid == userInfo.Id).FirstOrDefault();

            var model = new ProfileViewModel();
            model.NickName = userInfo.UserName;
            model.City = userInfo.City;
            model.Age = age;
            model.Zdjecia = photos;
            model.IsYourProfile = IsYourProfile;
            model.IsAlreadyLiked = IsAlreadyLiked;
            model.zainteresowania = zainteresowania;
            model.profileInfo = profileInfo;

            return View(model);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult DodajZdjecie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DodajZdjecie(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {


                    string[] formats = new string[] { ".jpg", ".png", ".jpeg" }; // add more if u like...


                    var Isgoodformat = formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));

                    if (!Isgoodformat)
                    {
                        ViewBag.Message = "Zły format zdjęcia";
                        return View();
                    }


                    if (file.FileName.Length > 60)
                    {
                        ViewBag.Message = "Zbyt długa nazwa zdjecia";
                        return View();
                    }


                    string fileName = Guid.NewGuid().ToString() + file.FileName;
                    string path = Path.Combine(Server.MapPath("~/Content/photos"),
                                               fileName);
                    file.SaveAs(path);

                    var photo = new Photos();
                    photo.kogo = User.Identity.GetUserId();
                    photo.nazwazdjecia = fileName;

                    db.Photos.Add(photo);
                    db.SaveChanges();

                    ViewBag.Message = "Zdjecie zostało wrzucone :)";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "Nie podałes pliku. (Akceptowane .jpg .png. .jpeg)";
            }
            return View();
        }

        public ActionResult LosujUzytkownika()
        {
            var myName = User.Identity.GetUserId();

            var userName = db.AspNetUsers.Where(a => a.Id != myName).OrderBy(g => Guid.NewGuid()).Take(1).FirstOrDefault().UserName;


            return RedirectToAction("Index", "Home", new { name = userName });
        }

        public ActionResult FajnyProfil()
        {
            var myId = User.Identity.GetUserId();


            var result = (from users in db.AspNetUsers
                         join likes in db.polubienia on users.Id equals likes.odkogo
                         where likes.dlakogo == myId
                         orderby likes.id descending
                         select new polubieniaLinq { Osoba = users.UserName,ID = likes.id, IsShowed = likes.IsShowed }).ToList();

            foreach (var item in result)
            {
                var record = db.polubienia.Where(a => a.id == item.ID).FirstOrDefault();
                record.IsShowed = true;
            }
            db.SaveChanges();

            return View(result);
        }

        


        public ActionResult Zainteresowania()

        {
            var result = db.zainteresowania.OrderByDescending(a => a.ileOsob);

            var finalresult = new IntrestingViewModel();
            finalresult.zainteresowania = result;


            return View(finalresult);
        }

        [HttpPost]
        public ActionResult Zainteresowania(IntrestingViewModel model)
        {
            // var result = db.zainteresowania.
            if (String.IsNullOrEmpty(model.Nazwa))
            {
                return RedirectToAction("Zainteresowania");
            }

            var result = from m in db.zainteresowania
                          where m.nazwa.Contains(model.Nazwa)
                          orderby m.ileOsob descending
                          select m;

            var finalresult = new IntrestingViewModel();
            finalresult.zainteresowania = result;


            return View(finalresult);
        }



        public ActionResult DodajZainteresowanie()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajZainteresowanie(HttpPostedFileBase file,zainteresowania model)
        {


            if (file != null && file.ContentLength > 0)
                try
                {
                    if (model.nazwa.Length > 13)
                    {
                        ViewBag.Message = "Zbyt długa nazwa ";
                        return View();
                    }

                    string[] formats = new string[] { ".jpg", ".png", ".jpeg" }; // add more if u like...


                    var Isgoodformat = formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));

                    if (!Isgoodformat)
                    {
                        ViewBag.Message = "Zły format zdjęcia";
                        return View();
                    }


                    if (file.FileName.Length > 60)
                    {
                        ViewBag.Message = "Zbyt długa nazwa zdjecia";
                        return View();
                    }


                    string fileName = Guid.NewGuid().ToString() + file.FileName;
                    string path = Path.Combine(Server.MapPath("~/Content/Intresting"),
                                               fileName);
                    file.SaveAs(path);

                    var zainteresowanie = new zainteresowania();
                    zainteresowanie.autor = User.Identity.GetUserId();
                    zainteresowanie.zdjecie = fileName;
                    zainteresowanie.nazwa = model.nazwa;
                    zainteresowanie.ileOsob = 1;


                    db.zainteresowania.Add(zainteresowanie);
                    db.SaveChanges();


                    var zapisani = new Zapisani();
                    zapisani.kogo = User.Identity.GetUserId();
                    zapisani.jakieZainteresowanie = zainteresowanie.id;

                    db.Zapisani.Add(zapisani);





                    db.SaveChanges();

                    ViewBag.Message = "Zainteresowanie zostało dodane :)";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "Nie podałes pliku. (Akceptowane .jpg .png. .jpeg)";
            }


            return View();
        }

        public ActionResult Zainteresowanie(int id)
        {
            var result = db.zainteresowania.Where(a => a.id == id).FirstOrDefault();


            var myName = User.Identity.GetUserId();

            var IsAlreadyIn = db.Zapisani.Any(a => a.kogo == myName && a.jakieZainteresowanie == id);
            ViewBag.IsAlreadyIn = IsAlreadyIn;

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Zainteresowanie(zainteresowania model)
        {
            var myName = User.Identity.GetUserId();

            var IsAlreadyIn = db.Zapisani.Any(a => a.kogo == myName && a.jakieZainteresowanie == model.id);
            if (IsAlreadyIn)
            {
                return RedirectToAction("Zainteresowanie");
            }

            var zapisani = new Zapisani();
            zapisani.kogo = myName;
            zapisani.jakieZainteresowanie = model.id;

            db.Zapisani.Add(zapisani);


            var zainteresowanie = db.zainteresowania.Where(a => a.id == model.id).FirstOrDefault();
            zainteresowanie.ileOsob++;


            db.SaveChanges();

            return RedirectToAction("Zainteresowanie");
        }




            public ActionResult NoUser()
        {
            ViewBag.Message = "Nie ma takiego Profilu";

            return View();

        }

        

        public ActionResult Wiadomosc()
        {
            return RedirectToAction("Wiadomosci", new { userid = "user5" });
        }
        

        public ActionResult Wiadomosci(string userid)

        {
            if (userid == null)
            {
                return RedirectToAction("Wiadomosci", new { userid = "user5" });

            }

            var OtherUser = db.AspNetUsers.Where(s => s.UserName == userid).FirstOrDefault();

            if (OtherUser == null)
            {
                return RedirectToAction("NoUser");
            }

            var YourID = User.Identity.GetUserId();

            var idOtherUser = OtherUser.Id;
            var Msg = db.Wiadomosci.Where(a => a.odkogo == YourID && a.dokogo == idOtherUser ||
            a.odkogo == idOtherUser && a.dokogo == YourID).OrderByDescending(b => b.id).Take(14).AsEnumerable().Reverse();


            ViewBag.YourID = User.Identity.GetUserId();
            ViewBag.IdOtherUser = OtherUser.Id;

            
            return View(Msg);
        }


        public ActionResult ProfileInfo()
        {
            var myName = User.Identity.GetUserId();

            var data = db.ProfileInfo.Where(a => a.userid == myName).FirstOrDefault();


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileInfo(ProfileInfo model)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var myName = User.Identity.GetUserId();
            var data = db.ProfileInfo.Where(a => a.userid == myName).FirstOrDefault();
            
            if (data == null)
            {
                model.userid = myName;
                db.ProfileInfo.Add(model);

                db.SaveChanges();
            }
            else

            {
                data.colubie = model.colubie;
                data.favcolor = model.favcolor;
                data.opis = model.opis;
                data.phonenumber = model.phonenumber;
                data.pseudomin = model.pseudomin;
                data.samochod = model.samochod;
                data.waga = model.waga;
                data.wzrost = model.wzrost;

                db.SaveChanges();
            }


            return RedirectToAction("ProfileInfo");
        }

    }
}