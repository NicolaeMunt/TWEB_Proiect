using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;   // HasIndex (EF 6.1+)
using System.Xml.Linq;
using Domain.Entities.User;                                // your POCO classes

namespace BusinessLogic.Data
{
    public class ApplicationDbContext : DbContext
    {
        // “DefaultConnection” is the <connectionStrings> name in Web.config
        public ApplicationDbContext() : base("name=DefaultConnection") { }

        public DbSet<UserApp> Users { get; set; }
        public DbSet<ULoginDataApp> LoginData { get; set; }
        public DbSet<ULoginRespApp> LoginResponses { get; set; }
        public DbSet<UDbTableApp> DbTables { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /* ---------- User ------------------------------------------ */
            var user = modelBuilder.Entity<UserApp>()
                                   .ToTable("Users")
                                   .HasKey(u => u.Id);

            user.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            user.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            user.HasIndex(u => u.Username).IsUnique();
            user.HasIndex(u => u.Email).IsUnique();

            /* ---------- ULoginData  (1 : 1) ---------------------------- */
            var loginData = modelBuilder.Entity<ULoginDataApp>()
                                        .ToTable("UserLoginData")
                                        .HasKey(l => l.Id);

            loginData.HasRequired(l => l.User)
                     .WithOptional()                 // 1-to-1
                     .WillCascadeOnDelete(true);

            /* ---------- ULoginResp  (many : 1) ------------------------ */
            var loginResp = modelBuilder.Entity<ULoginRespApp>()
                                        .ToTable("UserLoginSessions")
                                        .HasKey(r => r.Id);

            loginResp.HasRequired(r => r.User)
                     .WithMany()
                     .HasForeignKey(r => r.UserId)
                     .WillCascadeOnDelete(true);

            loginResp.HasIndex(r => r.Token).IsUnique();

            /* ---------- UDbTable  (many : 1) -------------------------- */
            var dbTable = modelBuilder.Entity<UDbTableApp>()
                                      .ToTable("UserDbTables")
                                      .HasKey(t => t.Id);

            dbTable.HasRequired(t => t.User)
                   .WithMany()
                   .HasForeignKey(t => t.UserId)
                   .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
