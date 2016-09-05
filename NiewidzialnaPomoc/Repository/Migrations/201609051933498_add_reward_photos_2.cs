namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_reward_photos_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertisementPhotoes", "FileContent", c => c.Binary());
            DropColumn("dbo.AdvertisementPhotoes", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdvertisementPhotoes", "Content", c => c.Binary());
            DropColumn("dbo.AdvertisementPhotoes", "FileContent");
        }
    }
}
