namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class default_photos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DefaultPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        FileContent = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DefaultPhotoes");
        }
    }
}
