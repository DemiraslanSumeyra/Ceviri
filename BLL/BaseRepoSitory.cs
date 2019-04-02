using DAL;
using Entity;
using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace BLL
{
    public class BaseRepoSitory<T> where T : class,IEntity
    {
        private DictionaryContext _db;
        public BaseRepoSitory(DictionaryContext db)
        {
            _db = db;
        }

        public List<T> GetAll()
        {
            return _db.Set<T>().ToList(); 
        }
        //bool Func (T word)
        //{
        //    Word word = new Word();
        //    return word.Language.ID == 2;
        //}
        public List<T> Search(Func<T,bool> query)
        {
            return _db.Set<T>().Where(query).ToList();
        }

        public IQueryable<T> Queryable()
        {
            return _db.Set<T>().AsQueryable();
        }

        public T GetOne(int ID)
        {
            return _db.Set<T>().Find(ID);
        }

        public bool Add(T record)
        {
            try
            {
                _db.Set<T>().Add(record);
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public virtual bool Delete(int id)
        {
            try
            {
                T t = GetOne(id);
                _db.Set<T>().Remove(t);
                return true;
            }
            catch (Exception)
            {

                return false ;
            }
           
        }
        public bool Update(T newRecord)
        {
            try
            {
                var old = GetOne(newRecord.ID);
                _db.Entry(old).CurrentValues.SetValues(newRecord);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
           
        }
    }
}
