namespace BookLive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimePlaceOfferandCommenttoRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Time", c => c.String());
            AddColumn("dbo.Requests", "Place", c => c.String());
            AddColumn("dbo.Requests", "Offer", c => c.String());
            AddColumn("dbo.Requests", "Comment", c => c.String());
            AlterColumn("dbo.Requests", "FromUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "FromUserId");
            AddForeignKey("dbo.Requests", "FromUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "FromUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "FromUserId" });
            AlterColumn("dbo.Requests", "FromUserId", c => c.String());
            DropColumn("dbo.Requests", "Comment");
            DropColumn("dbo.Requests", "Offer");
            DropColumn("dbo.Requests", "Place");
            DropColumn("dbo.Requests", "Time");
        }
    }
}
