using Entity.Abstract;
using System.Collections.Generic;

namespace Entity
{
    public class Word : IEntity
    {
        public int ID { get; set; }
        public string WordTxt { get; set; }
        public int Language_ID { get; set; }
        public virtual Language Language { get; set; }
        public virtual List<Word> Translations { get; set; }
    }
}
