namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class data_annotations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Avatars", "FileContent", c => c.Binary());
            AddColumn("dbo.RewardPhotoes", "FileContent", c => c.Binary());
            AlterColumn("dbo.Advertisements", "Title", c => c.String(nullable: false, maxLength: 72));
            AlterColumn("dbo.Advertisements", "Content", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.Avatars", "Content");
            DropColumn("dbo.RewardPhotoes", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RewardPhotoes", "Content", c => c.Binary());
            AddColumn("dbo.Avatars", "Content", c => c.Binary());
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.Advertisements", "Content", c => c.String(maxLength: 500));
            AlterColumn("dbo.Advertisements", "Title", c => c.String(maxLength: 72));
            DropColumn("dbo.RewardPhotoes", "FileContent");
            DropColumn("dbo.Avatars", "FileContent");
        }
    }
}
