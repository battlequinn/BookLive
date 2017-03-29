namespace BookLive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedforeignkeysinrequestandaddedFromUserEmail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "ToUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "FromUserId" });
            DropIndex("dbo.Requests", new[] { "ToUserId" });
            AddColumn("dbo.Requests", "FromUserEmail", c => c.String());
            AlterColumn("dbo.Requests", "FromUserId", c => c.String());
            AlterColumn("dbo.Requests", "ToUserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "ToUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Requests", "FromUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Requests", "FromUserEmail");
            CreateIndex("dbo.Requests", "ToUserId");
            CreateIndex("dbo.Requests", "FromUserId");
            AddForeignKey("dbo.Requests", "ToUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Requests", "FromUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
