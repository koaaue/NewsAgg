using System.Data.Entity;
using MvcApplication8.Models;

using System.Linq;

namespace MvcApplication8.Models
{
    public class CarContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MvcApplication8.Models.CarContext>());

        public CarContext() : base("name=CarContext")
        {
        }

        public DbSet<item> items { get; set; }
        public DbSet<source> sources { get; set; }
        public DbSet<like> likes { get; set; }

        public DbSet<KeywordsTotal> keywordsTotal { get; set; }

        public DbSet<ArticleKeyword> articleKeyword { get; set; }
    }
}
