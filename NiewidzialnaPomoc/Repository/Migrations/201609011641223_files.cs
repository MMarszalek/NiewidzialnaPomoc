namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class files : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.Files", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "ApplicationUserId" });
            DropIndex("dbo.Files", new[] { "AdvertisementId" });
            CreateTable(
                "dbo.Avatars",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Content = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            DropTable("dbo.Files");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        AdvertisementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileId);
            
            DropForeignKey("dbo.Avatars", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Avatars", new[] { "Id" });
            DropTable("dbo.Avatars");
            CreateIndex("dbo.Files", "AdvertisementId");
            CreateIndex("dbo.Files", "ApplicationUserId");
            AddForeignKey("dbo.Files", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Files", "AdvertisementId", "dbo.Advertisements", "Id", cascadeDelete: true);
        }
    }
}
