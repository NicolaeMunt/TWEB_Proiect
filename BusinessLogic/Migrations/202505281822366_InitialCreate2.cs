namespace BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.User", newName: "Users");
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 255),
                        Title = c.String(nullable: false, maxLength: 200),
                        Category = c.String(nullable: false, maxLength: 100),
                        Details = c.String(storeType: "ntext"),
                        AttachmentPath = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                        UserId = c.Int(),
                        Views = c.Int(nullable: false),
                        Answers = c.Int(nullable: false),
                        IsResolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "UserId", "dbo.Users");
            DropIndex("dbo.Questions", new[] { "UserId" });
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Users", "IsActive");
            DropColumn("dbo.Users", "CreatedDate");
            DropTable("dbo.Questions");
            RenameTable(name: "dbo.Users", newName: "User");
        }
    }
}
