namespace BusinessLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "ParentCommentId", "dbo.Comment");
            DropForeignKey("dbo.Post", "UserId", "dbo.User");
            DropForeignKey("dbo.Comment", "PostId", "dbo.Post");
            DropForeignKey("dbo.Comment", "UserId", "dbo.User");
            DropForeignKey("dbo.UDbTable", "UserId", "dbo.User");
            DropForeignKey("dbo.ULoginData", "UserId", "dbo.User");
            DropForeignKey("dbo.ULoginResp", "UserId", "dbo.User");
            DropIndex("dbo.Comment", new[] { "PostId" });
            DropIndex("dbo.Comment", new[] { "UserId" });
            DropIndex("dbo.Comment", new[] { "ParentCommentId" });
            DropIndex("dbo.Post", new[] { "UserId" });
            DropIndex("dbo.UDbTable", new[] { "UserId" });
            DropIndex("dbo.ULoginData", new[] { "UserId" });
            DropIndex("dbo.ULoginResp", new[] { "UserId" });
            AddColumn("dbo.User", "Password", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.User", "LoginTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.User", "IsAdmin");
            DropTable("dbo.Comment");
            DropTable("dbo.Post");
            DropTable("dbo.UDbTable");
            DropTable("dbo.ULoginData");
            DropTable("dbo.ULoginResp");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ULoginResp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Token = c.String(nullable: false, maxLength: 100),
                        ExpiryDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ULoginData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PasswordHash = c.String(),
                        Salt = c.String(),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UDbTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TableName = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Content = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        Upvotes = c.Int(nullable: false),
                        Downvotes = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ParentCommentId = c.Int(),
                        Content = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        Upvotes = c.Int(nullable: false),
                        Downvotes = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.User", "IsAdmin", c => c.Boolean(nullable: false));
            DropColumn("dbo.User", "LoginTime");
            DropColumn("dbo.User", "Password");
            CreateIndex("dbo.ULoginResp", "UserId");
            CreateIndex("dbo.ULoginData", "UserId");
            CreateIndex("dbo.UDbTable", "UserId");
            CreateIndex("dbo.Post", "UserId");
            CreateIndex("dbo.Comment", "ParentCommentId");
            CreateIndex("dbo.Comment", "UserId");
            CreateIndex("dbo.Comment", "PostId");
            AddForeignKey("dbo.ULoginResp", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ULoginData", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UDbTable", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comment", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.Comment", "PostId", "dbo.Post", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Post", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.Comment", "ParentCommentId", "dbo.Comment", "Id");
        }
    }
}
