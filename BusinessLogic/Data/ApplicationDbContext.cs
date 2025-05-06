using System.Data.Entity;
using TWEB_Proiect.Domain.Entities;

namespace TWEB_Proiect.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("ApplicationDbContext")
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}