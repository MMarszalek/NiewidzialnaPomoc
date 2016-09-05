namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_reward_photos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RewardPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Content = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rewards", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RewardPhotoes", "Id", "dbo.Rewards");
            DropIndex("dbo.RewardPhotoes", new[] { "Id" });
            DropTable("dbo.RewardPhotoes");
        }
    }
}
