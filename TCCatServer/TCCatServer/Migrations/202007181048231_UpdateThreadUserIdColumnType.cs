namespace TCCatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThreadUserIdColumnType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Threads", "UserId", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Threads", "UserId", c => c.Guid(nullable: false));
        }
    }
}
