using DAL;
using Entity;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TranslateManager
    {
        DictionaryContext _db;
        public TranslateManager(DictionaryContext db)
        {
            _db = db;

        }
        public List<string> Translate(HomeViewModel home)
        {
            var p1 = new SqlParameter("word",home.FromWord);
            var p2 = new SqlParameter("fromlang", home.FromLang);
            var p3 = new SqlParameter("tolang", home.ToLang);

            return _db.Database.SqlQuery<string>("exec Translate @word,@fromlang,@tolang",p1,p2,p3).ToList();
        }

    }
}
