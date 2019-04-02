using BLL;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSozluk.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork _uw = new UnitOfWork();
        [HttpGet]
        public ActionResult Index()
        {
            
            ViewBag.Langs = _uw.Languages.GetAll().Select(x =>
            new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()

            });
            return View();
        }
        [HttpPost]
        public ActionResult Index(HomeViewModel reqest)
        {
            reqest.Translations = _uw.TranslateManager.Translate(reqest);
            ViewBag.Langs = _uw.Languages.GetAll().Select(x =>
            new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()

            });
            return View(reqest);
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
    }
}