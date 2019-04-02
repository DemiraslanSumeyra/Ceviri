using Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;

namespace DAL
{
    public class DictionaryContext : IdentityDbContext<Person>//DbContext kullanıcı işlemleri olmayan düz db, IdentityDbContext<T> T tipindeki kullanıcıya göre 5 tablo ekler.
    {
        public DictionaryContext():base("DictionaryContext")
        {
            
        }
        //public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<WordRequest> WordRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region TableConfiguration
            modelBuilder.Entity<Language>().HasKey(x => x.ID);
            modelBuilder.Entity<Word>().HasKey(x => x.ID);
            modelBuilder.Entity<WordRequest>().HasKey(x => x.ID);
            #endregion
            #region Relations
            modelBuilder.Entity<Language>().HasMany(x => x.Words).WithRequired(x => x.Language).HasForeignKey(x=> x.Language_ID);
            modelBuilder.Entity<Word>().HasMany(x => x.Translations).WithMany();
            modelBuilder.Entity<WordRequest>().HasRequired(x => x.Person).WithMany(x => x.WordRequests);
            modelBuilder.Entity<WordRequest>().HasRequired(x => x.Language).WithMany(x=> x.WordRequests).HasForeignKey(x=> x.LanguageID);
            
            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
