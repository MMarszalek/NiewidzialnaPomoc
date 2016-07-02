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
            SeedRoles(context);
            SeedUsers(context);
            SeedAdvertisements(context);
            SeedCategories(context);
            SeedLocations(context);
            SeedRewards(context);
            SeedRewardCodes(context);
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
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {

                var user = new ApplicationUser { UserName = "admin" };
                var result = manager.Create(user, "12345"); //password: 12345
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "tomasz@gmail.com"))
            {

                var user = new ApplicationUser { UserName = "tomasz@gmail.com", FirstName = "Tomasz", LastName = "Nowak", Points = 0 };
                var result = manager.Create(user, "12345");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "marek@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "marek@gmail.com", FirstName = "Marek", LastName = "Kowalski", Points = 0 };
                var result = manager.Create(user, "12345");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "robert@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "robert@gmail.com", FirstName = "Robert", LastName = "Lewandowski", Points = 0 };
                var result = manager.Create(user, "12345");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }
        }

        private void SeedAdvertisements(ApplicationDbContext context)
        {
            //Categories, Helpers
            var adverisements = new List<Advertisement>
            {
                new Advertisement { Id = 1, Title = "Pomoc w ogrodzie", Content = "Szukam pomocy przy œciêciu drzewa", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "tomasz@gmail.com").FirstOrDefault().Id,
                    LocationId = 1, IsFinished = false },
                new Advertisement { Id = 2, Title = "Pomoc w gra¿u", Content = "Szukam pomocy przy naprawie samochodu", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "tomasz@gmail.com").FirstOrDefault().Id,
                    LocationId = 1, IsFinished = false },
                new Advertisement { Id = 3, Title = "Pomoc przy przeprowadzce", Content = "Szukam pomocy przy przenoszeniu mebli", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "tomasz@gmail.com").FirstOrDefault().Id,
                    LocationId = 2, IsFinished = false },
                new Advertisement { Id = 4, Title = "Pomoc przy remoncie", Content = "Szukam pomocy przy malowaniu œcian", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "marek@gmail.com").FirstOrDefault().Id,
                    LocationId = 1, IsFinished = false },
                new Advertisement { Id = 5, Title = "Pomoc w dotraciu na koncert", Content = "Szukam pomocy przy dotraciu na koncert", AddDate = DateTime.Now,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "marek@gmail.com").FirstOrDefault().Id,
                    LocationId = 2, IsFinished = false },
            };

            adverisements.ForEach(s => context.Advertisements.AddOrUpdate(p => p.Id, s));
            context.SaveChanges();
        }

        private void SeedCategories(ApplicationDbContext context)
        {
        }

        private void SeedLocations(ApplicationDbContext context)
        {
        }

        private void SeedRewards(ApplicationDbContext context)
        {
        }

        private void SeedRewardCodes(ApplicationDbContext context)
        {
        }
    }
}