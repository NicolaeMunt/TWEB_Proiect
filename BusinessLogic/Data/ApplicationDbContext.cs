using System.Data.Entity;
using TWEB_Proiect.Domain.Entities;

namespace TWEB_Proiect.Data
{
     public class ApplicationDbContext : DbContext
     {
          public ApplicationDbContext() : base("ApplicationDbContext")
          {
               // Автоматически создавать базу данных если она не существует
               Database.SetInitializer<ApplicationDbContext>(null);
          }

          // DbSets
          public DbSet<User> Users { get; set; }
          public DbSet<Question> Questions { get; set; }
          public DbSet<Answer> Answers { get; set; }

          protected override void OnModelCreating(DbModelBuilder modelBuilder)
          {
               // Настройка таблиц
               modelBuilder.Entity<User>().ToTable("Users");
               modelBuilder.Entity<Question>().ToTable("Questions");
               modelBuilder.Entity<Answer>().ToTable("Answers");

               // Настройка связей для Questions
               modelBuilder.Entity<Question>()
                   .HasOptional(q => q.User)
                   .WithMany()
                   .HasForeignKey(q => q.UserId);

               // Настройка связей для Answers
               modelBuilder.Entity<Answer>()
                   .HasRequired(a => a.Question)
                   .WithMany(q => q.QuestionAnswers)
                   .HasForeignKey(a => a.QuestionId)
                   .WillCascadeOnDelete(true);

               modelBuilder.Entity<Answer>()
                   .HasRequired(a => a.User)
                   .WithMany()
                   .HasForeignKey(a => a.UserId)
                   .WillCascadeOnDelete(false);

               base.OnModelCreating(modelBuilder);
          }
     }
}