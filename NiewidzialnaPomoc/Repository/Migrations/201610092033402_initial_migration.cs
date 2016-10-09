namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdvertisementPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        FileContent = c.Binary(),
                        AdvertisementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId, cascadeDelete: true)
                .Index(t => t.AdvertisementId);
            
            CreateTable(
                "dbo.Advertisements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 72),
                        Content = c.String(nullable: false, maxLength: 500),
                        AddDate = c.DateTime(nullable: false),
                        DifficultyId = c.Int(nullable: false),
                        PerformanceId = c.Int(nullable: false),
                        AuthorId = c.String(maxLength: 128),
                        LocationId = c.Int(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.Difficulties", t => t.DifficultyId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Performances", t => t.PerformanceId, cascadeDelete: true)
                .Index(t => t.DifficultyId)
                .Index(t => t.PerformanceId)
                .Index(t => t.AuthorId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Points = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Avatars",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FileName = c.String(),
                        ContentType = c.String(),
                        FileContent = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RewardCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        IsUsed = c.Boolean(nullable: false),
                        ReceivedDate = c.DateTime(),
                        RewardId = c.Int(nullable: false),
                        RewardOwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RewardOwnerId)
                .ForeignKey("dbo.Rewards", t => t.RewardId, cascadeDelete: true)
                .Index(t => t.RewardId)
                .Index(t => t.RewardOwnerId);
            
            CreateTable(
                "dbo.Rewards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RewardPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FileName = c.String(),
                        ContentType = c.String(),
                        FileContent = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rewards", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Difficulties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Performances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ApplicationUserAdvertisements",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Advertisement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Advertisement_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Advertisement_Id);
            
            CreateTable(
                "dbo.CategoryAdvertisements",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        Advertisement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.Advertisement_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id, cascadeDelete: true)
                .Index(t => t.Category_Id)
                .Index(t => t.Advertisement_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Advertisements", "PerformanceId", "dbo.Performances");
            DropForeignKey("dbo.Advertisements", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Advertisements", "DifficultyId", "dbo.Difficulties");
            DropForeignKey("dbo.CategoryAdvertisements", "Advertisement_Id", "dbo.Advertisements");
            DropForeignKey("dbo.CategoryAdvertisements", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.ApplicationUserAdvertisements", "Advertisement_Id", "dbo.Advertisements");
            DropForeignKey("dbo.ApplicationUserAdvertisements", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RewardPhotoes", "Id", "dbo.Rewards");
            DropForeignKey("dbo.RewardCodes", "RewardId", "dbo.Rewards");
            DropForeignKey("dbo.RewardCodes", "RewardOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Advertisements", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Avatars", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AdvertisementPhotoes", "AdvertisementId", "dbo.Advertisements");
            DropIndex("dbo.CategoryAdvertisements", new[] { "Advertisement_Id" });
            DropIndex("dbo.CategoryAdvertisements", new[] { "Category_Id" });
            DropIndex("dbo.ApplicationUserAdvertisements", new[] { "Advertisement_Id" });
            DropIndex("dbo.ApplicationUserAdvertisements", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.RewardPhotoes", new[] { "Id" });
            DropIndex("dbo.RewardCodes", new[] { "RewardOwnerId" });
            DropIndex("dbo.RewardCodes", new[] { "RewardId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Avatars", new[] { "Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Advertisements", new[] { "LocationId" });
            DropIndex("dbo.Advertisements", new[] { "AuthorId" });
            DropIndex("dbo.Advertisements", new[] { "PerformanceId" });
            DropIndex("dbo.Advertisements", new[] { "DifficultyId" });
            DropIndex("dbo.AdvertisementPhotoes", new[] { "AdvertisementId" });
            DropTable("dbo.CategoryAdvertisements");
            DropTable("dbo.ApplicationUserAdvertisements");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DefaultPhotoes");
            DropTable("dbo.Performances");
            DropTable("dbo.Locations");
            DropTable("dbo.Difficulties");
            DropTable("dbo.Categories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.RewardPhotoes");
            DropTable("dbo.Rewards");
            DropTable("dbo.RewardCodes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Avatars");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Advertisements");
            DropTable("dbo.AdvertisementPhotoes");
        }
    }
}
