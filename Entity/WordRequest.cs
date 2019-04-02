using Entity.Abstract;

namespace Entity
{
    public  class WordRequest:IEntity
    {
        public int ID { get; set; }
        public string Word { get; set; }
        public virtual Person Person { get; set; }

        public int LanguageID { get; set; }
        public virtual Language Language { get; set; }
        
    }
}
