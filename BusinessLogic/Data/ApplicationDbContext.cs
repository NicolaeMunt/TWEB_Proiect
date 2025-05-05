using Domain.Entities.User;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BusinessLogic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ULoginData> LoginData { get; set; }
        public DbSet<ULoginResp> LoginResponses { get; set; }
        public DbSet<UDbTable> DbTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Configure User entity
            modelBuilder.Entity<User>()
                .ToTable("User");

            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            // Configure ULoginData entity
            modelBuilder.Entity<ULoginData>()
                .ToTable("ULoginData");

            modelBuilder.Entity<ULoginData>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<ULoginData>()
                .HasRequired(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(true);

            // Configure ULoginResp entity
            modelBuilder.Entity<ULoginResp>()
                .ToTable("ULoginResp");

            modelBuilder.Entity<ULoginResp>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<ULoginResp>()
                .HasRequired(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ULoginResp>()
                .Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(100);

            // Configure UDbTable entity
            modelBuilder.Entity<UDbTable>()
                .ToTable("UDbTable");

            modelBuilder.Entity<UDbTable>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<UDbTable>()
                .HasRequired(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
