using BLL;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCSozluk.Controllers
{
    [Authorize(Roles = "admin")]
    public class WordsController : Controller
    {
        UnitOfWork _uw = new UnitOfWork();

        // GET: Words
        public ActionResult Index(int? sil,int? LanguageID)
        {
            var list=new List<Word>();
            List<SelectListItem> Langs =  _uw.Languages.GetAll().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()

            }).ToList();
            Langs.Insert(0, new SelectListItem()
            {
                Text = "Choose",
                Value = ""
            });
            ViewData["LangOptions"] = Langs;

            if (LanguageID.HasValue)
            {
                list = _uw.Words.Search(x=> x.Language_ID == LanguageID.Value);
            }
            else
            {
                list = _uw.Words.GetAll();
            }

            if (sil.HasValue)
            {
                _uw.Words.Delete(sil.Value);
                _uw.Complete();
                return RedirectToAction("Index", new { @LanguageID = LanguageID });
            }

            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            //Viewbag yerine kullanılabilecek diğer optionlar.
            //ViewData["LangOptions"] = _uw.Languages.GetAll();
            TempData["LangOptions"] = _uw.Languages.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ID.ToString()

            });//Bir seferlik contollerdan controlera veri gönderbiliyor.
            return View();
        }

        [HttpPost]
        public ActionResult Create(string WordTxt, int LanguageID)
        {
            if (string.IsNullOrEmpty(WordTxt))
            {
                ModelState.AddModelError("", "Words cannot be left blank.");
            }
            if (ModelState.IsValid)
            {
                Word word = new Word();
                word.WordTxt = WordTxt;
                word.Language_ID = LanguageID;

                var langs = _uw.Languages.GetAll();
                word.Translations = new List<Word>();

                foreach (var item in langs)
                {
                    string input = Request.Form["Translate" + item.ID];

                    string[] words = input.Split(',');

                    foreach (string a in words)
                    { 
                        if (!string.IsNullOrEmpty(a))
                        {
                            Word w = new Word();
                            w.Language_ID = item.ID;
                            w.WordTxt = a;
                            word.Translations.Add(w);
                        }
                    }                 
                }

                _uw.Words.Add(word);
                _uw.Complete();
                return RedirectToAction("Index", "Words");
            }

            return View();
        }
        public JsonResult AutoComplete(int langid, string q)
        {
            var list = _uw.Words.Search(x => x.Language_ID == langid && x.WordTxt.ToLower().StartsWith(q.ToLower())).Select(x => x.WordTxt).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}