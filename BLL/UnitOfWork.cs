using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UnitOfWork
    {
        public DictionaryContext db;
        public UnitOfWork()
        {
            object oylesine = "";
            if (db == null)
            {
                lock (oylesine)
                {
                    if (db == null)
                    {
                        db = new DictionaryContext();
                    }
                }
            }
            Languages = new BaseRepoSitory<Language>(db);
            Words = new WordRepoSitory(db);
            WordRequests = new BaseRepoSitory<WordRequest>(db);
            TranslateManager = new TranslateManager(db);

        }
        public static DictionaryContext Create()
        {
            return new DictionaryContext();
        }

        public bool Complete()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            
            
        }

        public BaseRepoSitory<Language> Languages;
        public BaseRepoSitory<Word> Words;
        public BaseRepoSitory<WordRequest> WordRequests;
        public TranslateManager TranslateManager;
    }
}
