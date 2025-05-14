using System.Data.Entity;
using TWEB_Proiect.Domain.Entities;

namespace TWEB_Proiect.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("ApplicationDbContext")
        {
        }

        // ТОЛЬКО ЭТИ ДВА DbSet - БОЛЬШЕ НИЧЕГО!
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройка таблиц
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Question>().ToTable("Questions");

            // Настройка связей
            modelBuilder.Entity<Question>()
                .HasOptional(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}