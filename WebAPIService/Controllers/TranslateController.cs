using BLL;
using Entity;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIService.Controllers
{
    public class TranslateController : ApiController
    {
        UnitOfWork _uw = new UnitOfWork();

        public List<string> GetWords(int FromLangID,int ToLangID,string word)
        {
            HomeViewModel model = new HomeViewModel();
            model.FromLang = FromLangID;
            model.ToLang = ToLangID;
            model.FromWord = word;

            var sonuc = _uw.TranslateManager.Translate(model);

            return sonuc;
        }
    }
}
