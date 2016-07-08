namespace Repository.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Repository.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Repository.Models.ApplicationDbContext context)
        {
            // Do debugowania metody seed
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            SeedRoles(context);
            SeedUsers(context);
            SeedCategories(context);
            SeedLocations(context);
            SeedRewards(context);
            SeedRewardCodes(context);
            SeedAdvertisements(context);
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            if (!context.Users.Any(u => u.UserName == "admin"))
            {

                var user = new ApplicationUser { UserName = "admin" };
                var result = manager.Create(user, "12345678"); //password: 12345678
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "tomasz@gmail.com"))
            {

                var user = new ApplicationUser { UserName = "tomasz@gmail.com", FirstName = "Tomasz", LastName = "Nowak", Points = 0 };
                var result = manager.Create(user, "12345678");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "marek@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "marek@gmail.com", FirstName = "Marek", LastName = "Kowalski", Points = 0 };
                var result = manager.Create(user, "12345678");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "robert@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "robert@gmail.com", FirstName = "Robert", LastName = "Lewandowski", Points = 0 };
                var result = manager.Create(user, "12345678");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }
        }

        private void SeedCategories(ApplicationDbContext context)
        {
            var categories = new List<Category>
            {
                new Category { Name = "Inne" },
                new Category { Name = "Ogród" },
                new Category { Name = "Transport" }
            };

            categories.ForEach(s => context.Categories.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

        private void SeedAdvertisements(ApplicationDbContext context)
        {
            var adverisements = new List<Advertisement>
            {
                new Advertisement { Title = "Pomoc w ogrodzie", Content = "Szukam pomocy przy œciêciu drzewa", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "tomasz@gmail.com").FirstOrDefault().Id,
                    LocationId = 1, IsFinished = false, Categories = new List<Category>() },
                new Advertisement { Title = "Pomoc w gra¿u", Content = "Szukam pomocy przy naprawie samochodu", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "tomasz@gmail.com").FirstOrDefault().Id,
                    LocationId = 1, IsFinished = false, Categories = new List<Category>() },
                new Advertisement { Title = "Pomoc przy przeprowadzce", Content = "Szukam pomocy przy przenoszeniu mebli", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "tomasz@gmail.com").FirstOrDefault().Id,
                    LocationId = 2, IsFinished = false, Categories = new List<Category>() },
                new Advertisement { Title = "Pomoc przy remoncie", Content = "Szukam pomocy przy malowaniu œcian", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "marek@gmail.com").FirstOrDefault().Id,
                    LocationId = 1, IsFinished = false },
                new Advertisement { Title = "Pomoc w dotraciu na koncert", Content = "Szukam pomocy przy dotraciu na koncert", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "marek@gmail.com").FirstOrDefault().Id,
                    LocationId = 2, IsFinished = false }
            };

            adverisements.ForEach(s => context.Advertisements.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            AddOrUpdateCategories(context, "Pomoc w ogrodzie", "Inne");
            AddOrUpdateCategories(context, "Pomoc w ogrodzie", "Ogród");
            AddOrUpdateCategories(context, "Pomoc w gra¿u", "Inne");
            AddOrUpdateCategories(context, "Pomoc przy przeprowadzce", "Inne");
            AddOrUpdateCategories(context, "Pomoc przy przeprowadzce", "Transport");
        }

        private void SeedLocations(ApplicationDbContext context)
        {
            var locations = new List<Location>
            {
                new Location { Name = "Poznañ" },
                new Location { Name = "Kraków" },
                new Location { Name = "Warszawa" }
            };

            locations.ForEach(s => context.Locations.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

        private void SeedRewards(ApplicationDbContext context)
        {
            var rewards = new List<Reward>
            {
                new Reward { Name = "Empik 50 z³", Price = 500 },
                new Reward { Name = "Empik 100 z³", Price = 1000 },
                new Reward { Name = "Sodexo 100 z³", Price = 1200 }
            };

            rewards.ForEach(s => context.Rewards.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

        private void SeedRewardCodes(ApplicationDbContext context)
        {
            var codes = new List<RewardCode>
            {
                new RewardCode { Code = "1234", IsUsed = false, RewardId = 1 },
                new RewardCode { Code = "5678", IsUsed = false, RewardId = 1 },
                new RewardCode { Code = "asdf", IsUsed = false, RewardId = 2 },
                new RewardCode { Code = "zxcv", IsUsed = false, RewardId = 2 }
            };

            codes.ForEach(s => context.RewardCodes.AddOrUpdate(p => p.Code, s));
            context.SaveChanges();
        }

        void AddOrUpdateCategories(ApplicationDbContext context, string advertisementTitle, string categorieName)
        {
            var adv = context.Advertisements.SingleOrDefault(a => a.Title == advertisementTitle);
            var inst = adv.Categories.SingleOrDefault(i => i.Name == categorieName);
            if (inst == null)
                adv.Categories.Add(context.Categories.Single(i => i.Name == categorieName));
        } 
    }
}