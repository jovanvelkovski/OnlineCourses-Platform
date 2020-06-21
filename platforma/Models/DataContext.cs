using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base(nameOrConnectionString: "MyConnection") { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumComment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}