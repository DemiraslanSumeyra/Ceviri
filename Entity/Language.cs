using Entity.Abstract;
using System.Collections.Generic;

namespace Entity
{
    public class Language:IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<Word> Words { get; set; }
        public virtual List<WordRequest> WordRequests { get; set; }

    }
}
