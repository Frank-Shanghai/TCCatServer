namespace TCCatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDomainModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fora",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(maxLength: 128),
                        LastUpdateDate = c.DateTime(),
                        Author_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.PostLikes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        PostId = c.Guid(nullable: false),
                        IsLike = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ThreadId = c.Guid(nullable: false),
                        ParentId = c.Guid(),
                        Message = c.String(nullable: false),
                        PostedOn = c.DateTime(nullable: false),
                        Author_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.ParentId)
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.ThreadId)
                .Index(t => t.ParentId)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Threads",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 500),
                        LastUpdateDate = c.DateTime(nullable: false),
                        ForumId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Author_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Fora", t => t.ForumId)
                .Index(t => t.ForumId)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.ThreadFavorites",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ThreadId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ThreadId);
            
            CreateTable(
                "dbo.ThreadLikes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ThreadId = c.Guid(nullable: false),
                        IsLike = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ThreadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Threads", "ForumId", "dbo.Fora");
            DropForeignKey("dbo.Threads", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ThreadLikes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ThreadFavorites", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostLikes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ThreadLikes", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Posts", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.ThreadFavorites", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.PostLikes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Posts", "ParentId", "dbo.Posts");
            DropForeignKey("dbo.Fora", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ThreadLikes", new[] { "ThreadId" });
            DropIndex("dbo.ThreadLikes", new[] { "UserId" });
            DropIndex("dbo.ThreadFavorites", new[] { "ThreadId" });
            DropIndex("dbo.ThreadFavorites", new[] { "UserId" });
            DropIndex("dbo.Threads", new[] { "Author_Id" });
            DropIndex("dbo.Threads", new[] { "ForumId" });
            DropIndex("dbo.Posts", new[] { "Author_Id" });
            DropIndex("dbo.Posts", new[] { "ParentId" });
            DropIndex("dbo.Posts", new[] { "ThreadId" });
            DropIndex("dbo.PostLikes", new[] { "PostId" });
            DropIndex("dbo.PostLikes", new[] { "UserId" });
            DropIndex("dbo.Fora", new[] { "Author_Id" });
            DropTable("dbo.ThreadLikes");
            DropTable("dbo.ThreadFavorites");
            DropTable("dbo.Threads");
            DropTable("dbo.Posts");
            DropTable("dbo.PostLikes");
            DropTable("dbo.Fora");
        }
    }
}
