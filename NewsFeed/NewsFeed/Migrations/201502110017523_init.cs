namespace NewsFeed.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        pubDate = c.String(),
                        guid = c.String(),
                        Date = c.DateTime(),
                        totalLike = c.Int(nullable: false),
                        srcName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.sources", t => t.srcName)
                .Index(t => t.srcName);
            
            CreateTable(
                "dbo.sources",
                c => new
                    {
                        srcName = c.String(nullable: false, maxLength: 128),
                        newDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.srcName);
            
            CreateTable(
                "dbo.likes",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserName, t.ItemId });
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.items", new[] { "srcName" });
            DropForeignKey("dbo.items", "srcName", "dbo.sources");
            DropTable("dbo.likes");
            DropTable("dbo.sources");
            DropTable("dbo.items");
            DropTable("dbo.UserProfile");
        }
    }
}
