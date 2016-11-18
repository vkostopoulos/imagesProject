using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace imagesAzureProject.Models
{
    public class ImagesContext : DbContext
    { 
        public ImagesContext() : base("name=imageconnectionstring")
        {
            Database.SetInitializer<ImagesContext>(new DropCreateDatabaseIfModelChanges<ImagesContext>());
        }
        public DbSet<Image> Images { get; set; }
    }
}