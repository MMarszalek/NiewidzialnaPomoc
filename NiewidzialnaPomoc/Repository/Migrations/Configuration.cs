namespace Repository.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
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
            SeedDifficulties(context);
            SeedPerformances(context);
            SeedLocations(context);
            SeedRewards(context);
            SeedRewardCodes(context);
            SeedAdvertisements(context);
            SeedRewardPhotos(context);
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

                var user = new ApplicationUser { UserName = "admin", Email = "admin@gmail.com" };
                var result = manager.Create(user, "12345678"); //password: 12345678
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "arkadiusz@gmail.com"))
            {

                var user = new ApplicationUser { UserName = "arkadiusz@gmail.com", Email = "arkadiusz@gmail.com", FirstName = "Arkadiusz", LastName = "Nowak", Points = 3000 };
                var result = manager.Create(user, "12345678");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "sebastian@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "sebastian@gmail.com", Email = "sebastian@gmail.com", FirstName = "Sebastian", LastName = "Kowalski", Points = 2000 };
                var result = manager.Create(user, "12345678");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "kamil@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "kamil@gmail.com", Email = "kamil@gmail.com", FirstName = "Kamil", LastName = "Winiarczyk", Points = 2500 };
                var result = manager.Create(user, "12345678");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "michal@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "michal@gmail.com", Email = "michal@gmail.com", FirstName = "Micha³", LastName = "G³owacki", Points = 1000 };
                var result = manager.Create(user, "12345678");
                if (result.Succeeded)
                    manager.AddToRole(user.Id, "User");
            }

            if (!context.Users.Any(u => u.UserName == "robert@gmail.com"))
            {
                var user = new ApplicationUser { UserName = "robert@gmail.com", Email = "robert@gmail.com", FirstName = "Robert", LastName = "Mazurek", Points = 500 };
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
                new Category { Name = "Ogrodnictwo" },
                new Category { Name = "Transport" },
                new Category { Name = "Remont" },
                new Category { Name = "Wolontariat" }
            };

            categories.ForEach(s => context.Categories.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

        private void SeedDifficulties(ApplicationDbContext context)
        {
            var difficulties = new List<Difficulty>
            {
                new Difficulty { Name = "Niska", Points = 50 },
                new Difficulty { Name = "Œrednia", Points = 100 },
                new Difficulty { Name = "Wysoka", Points = 200 }
            };

            difficulties.ForEach(s => context.Difficulties.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

        private void SeedPerformances(ApplicationDbContext context)
        {
            var performances = new List<Performance>
            {
                new Performance { Name = "S³abe", Points = 0 },
                new Performance { Name = "Œrednie", Points = 20 },
                new Performance { Name = "Dobre", Points = 40 },
                new Performance { Name = "Bardzo dobre", Points = 60 },
                new Performance { Name = "Znakomite", Points = 80 },
            };

            performances.ForEach(s => context.Performances.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

        private void SeedLocations(ApplicationDbContext context)
        {
            var locations = new List<Location>
            {
                new Location { Name = "Poznañ" },
                new Location { Name = "Kraków" },
                new Location { Name = "Wroc³aw" },
                new Location { Name = "Katowice" },
                new Location { Name = "Warszawa" }
            };

            locations.ForEach(s => context.Locations.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

        private void SeedRewards(ApplicationDbContext context)
        {
            var rewards = new List<Reward>
            {
                new Reward { Name = "Karta prezentowa Empik 50 z³", Price = 500 },
                new Reward { Name = "Karta prezentowa Empik 100 z³", Price = 1000 },
                new Reward { Name = "Karta prezentowa Empik 150 z³", Price = 1500 },
                new Reward { Name = "Karta podarunkowa C&A 50 z³", Price = 500 },
                new Reward { Name = "Karta podarunkowa C&A 100 z³", Price = 1000 },
                new Reward { Name = "Karta podarunkowa C&A 150 z³", Price = 1500 },
                new Reward { Name = "Karta podarunkowa H&M 50 z³", Price = 600 },
                new Reward { Name = "Karta podarunkowa H&M 100 z³", Price = 1200 },
                new Reward { Name = "Karta podarunkowa H&M 150 z³", Price = 1800 }
            };

            rewards.ForEach(s => context.Rewards.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

        private void SeedRewardCodes(ApplicationDbContext context)
        {
            var codes = new List<RewardCode>
            {
                new RewardCode { Code = "3452345", IsUsed = false, RewardId = 1 },
                new RewardCode { Code = "4567456", IsUsed = false, RewardId = 1 },
                new RewardCode { Code = "7856788", IsUsed = false, RewardId = 1 },

                new RewardCode { Code = "rtetets", IsUsed = false, RewardId = 2 },
                new RewardCode { Code = "vzxcvfz", IsUsed = false, RewardId = 2 },
                new RewardCode { Code = "bxcvbcv", IsUsed = false, RewardId = 2 },

                new RewardCode { Code = "4689678", IsUsed = false, RewardId = 3 },
                new RewardCode { Code = "8657546", IsUsed = false, RewardId = 3 },
                new RewardCode { Code = "3123243", IsUsed = false, RewardId = 3 },

                new RewardCode { Code = "cvzxcvx", IsUsed = false, RewardId = 4 },
                new RewardCode { Code = "gfhfghg", IsUsed = false, RewardId = 4 },
                new RewardCode { Code = "mbnmbnm", IsUsed = false, RewardId = 4 },

                new RewardCode { Code = "4357876", IsUsed = false, RewardId = 5 },
                new RewardCode { Code = "6574345", IsUsed = false, RewardId = 5 },
                new RewardCode { Code = "1128678", IsUsed = false, RewardId = 5 },

                new RewardCode { Code = "lkjlljk", IsUsed = false, RewardId = 6 },
                new RewardCode { Code = "xczxvcs", IsUsed = false, RewardId = 6 },
                new RewardCode { Code = "vbnbvgt", IsUsed = false, RewardId = 6 },

                new RewardCode { Code = "4324289", IsUsed = false, RewardId = 7 },
                new RewardCode { Code = "7567901", IsUsed = false, RewardId = 7 },
                new RewardCode { Code = "3427689", IsUsed = false, RewardId = 7 },

                new RewardCode { Code = "rtehbvh", IsUsed = false, RewardId = 8 },
                new RewardCode { Code = "jkghqwe", IsUsed = false, RewardId = 8 },
                new RewardCode { Code = "cbcvbuy", IsUsed = false, RewardId = 8 },

                new RewardCode { Code = "6547892", IsUsed = false, RewardId = 9 },
                new RewardCode { Code = "3343679", IsUsed = false, RewardId = 9 },
                new RewardCode { Code = "7878345", IsUsed = false, RewardId = 9 }
            };

            codes.ForEach(s => context.RewardCodes.AddOrUpdate(p => p.Code, s));
            context.SaveChanges();
        }

        private void SeedRewardPhotos(ApplicationDbContext context)
        {
            var path = MapPath("~/Content/RewardPhotos/");
            var rewardPhotos = new List<RewardPhoto>
            {
                new RewardPhoto { Id = 1, FileName = "nagroda1.jpg", FileContent = File.ReadAllBytes(path + "nagroda1.jpg"), ContentType = "image/jpeg"},
                new RewardPhoto { Id = 2, FileName = "nagroda1.jpg", FileContent = File.ReadAllBytes(path + "nagroda1.jpg"), ContentType = "image/jpeg"},
                new RewardPhoto { Id = 3, FileName = "nagroda1.jpg", FileContent = File.ReadAllBytes(path + "nagroda1.jpg"), ContentType = "image/jpeg"},

                new RewardPhoto { Id = 4, FileName = "nagroda2.jpg", FileContent = File.ReadAllBytes(path + "nagroda2.jpg"), ContentType = "image/jpeg"},
                new RewardPhoto { Id = 5, FileName = "nagroda2.jpg", FileContent = File.ReadAllBytes(path + "nagroda2.jpg"), ContentType = "image/jpeg"},
                new RewardPhoto { Id = 6, FileName = "nagroda2.jpg", FileContent = File.ReadAllBytes(path + "nagroda2.jpg"), ContentType = "image/jpeg"},

                new RewardPhoto { Id = 7, FileName = "nagroda3.jpg", FileContent = File.ReadAllBytes(path + "nagroda3.jpg"), ContentType = "image/jpeg"},
                new RewardPhoto { Id = 8, FileName = "nagroda3.jpg", FileContent = File.ReadAllBytes(path + "nagroda3.jpg"), ContentType = "image/jpeg"},
                new RewardPhoto { Id = 9, FileName = "nagroda3.jpg", FileContent = File.ReadAllBytes(path + "nagroda3.jpg"), ContentType = "image/jpeg"}
            };

            rewardPhotos.ForEach(s => context.RewardPhotos.AddOrUpdate(rp => rp.Id, s));
            context.SaveChanges();
        }

        private string MapPath(string seedFile)
        {
            if (HttpContext.Current != null)
                return HostingEnvironment.MapPath(seedFile);

            var localPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var directoryName = Path.GetDirectoryName(localPath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~'));

            return path;
        }

        private void SeedAdvertisements(ApplicationDbContext context)
        {
            var adverisements = new List<Advertisement>
            {
                new Advertisement { Title = "Kwiaty pod blokiem", Content = "Szukam osób chêtnych do pomocy przy sadzeniu kwiatów pod moim blokiem.", AddDate = new DateTime(2016, 6, 15),
                    DifficultyId = 1, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "arkadiusz@gmail.com").FirstOrDefault().Id,
                    LocationId = 1, IsFinished = false, Categories = new List<Category>() },

                new Advertisement { Title = "Naprawa samochodu", Content = "Potrzebujê pomocy przy naprawie samochodu.", AddDate = new DateTime(2016, 6, 20),
                    DifficultyId = 3, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "sebastian@gmail.com").FirstOrDefault().Id,
                    LocationId = 1, IsFinished = false, Categories = new List<Category>() },

                new Advertisement { Title = "K³opotliwa przeprowadzka", Content = "Szukam pomocy przy przewo¿eniu mebli.", AddDate = new DateTime(2016, 6, 10),
                    DifficultyId = 3, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "kamil@gmail.com").FirstOrDefault().Id,
                    LocationId = 2, IsFinished = false, Categories = new List<Category>() },

                new Advertisement { Title = "Remont ³awek", Content = "Chcia³bym odmalowaæ ³aweczki pod blokiem, ka¿da pomoc mile widziana.", AddDate = new DateTime(2016, 6, 5),
                    DifficultyId = 2, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "michal@gmail.com").FirstOrDefault().Id,
                    LocationId = 2, IsFinished = false },

                new Advertisement { Title = "Wyjœcie do lekarza", Content = "Chcia³bym prosiæ o pomoc w dotarciu do lekarza.", AddDate = new DateTime(2016, 6, 12),
                    DifficultyId = 1, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "robert@gmail.com").FirstOrDefault().Id,
                    LocationId = 3, IsFinished = false },

                new Advertisement { Title = "Pomoc przy zakupach", Content = "Nie jestem ju¿ w stanie sam robiæ zakupów. Doceniê ka¿da pomoc.", AddDate = new DateTime(2016, 6, 4),
                    DifficultyId = 1, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "arkadiusz@gmail.com").FirstOrDefault().Id,
                    LocationId = 3, IsFinished = false },

                new Advertisement { Title = "Wyprzeda¿ gara¿owa", Content = "Organizujê wyprzeda¿ gara¿ow¹. Zapraszam wszystkich chêtnych do wziêcia udzia³u.", AddDate = new DateTime(2016, 6, 8),
                    DifficultyId = 1, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "sebastian@gmail.com").FirstOrDefault().Id,
                    LocationId = 4, IsFinished = false },

                new Advertisement { Title = "Warsztaty w domu dziecka", Content = "Szukam osób chêtnych do zorganizowania warsztatów w domu dziecka.", AddDate = new DateTime(2016, 6, 10),
                    DifficultyId = 2, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "kamil@gmail.com").FirstOrDefault().Id,
                    LocationId = 4, IsFinished = false },

                new Advertisement { Title = "Wolontariat w schronisku", Content = "Szukam osób, które by³yby zainteresowane pomoc¹ w schronisku.", AddDate = new DateTime(2016, 6, 14),
                    DifficultyId = 3, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "michal@gmail.com").FirstOrDefault().Id,
                    LocationId = 5, IsFinished = false },

                new Advertisement { Title = "Dojazd na koncert", Content = "Szukam osoby, która pomog³aby mi dojechaæ na koncert.", AddDate = new DateTime(2016, 6, 2),
                    DifficultyId = 2, PerformanceId = 1,
                    AuthorId = context.Set<ApplicationUser>().Where(u => u.UserName == "robert@gmail.com").FirstOrDefault().Id,
                    LocationId = 5, IsFinished = false }
            };

            adverisements.ForEach(s => context.Advertisements.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            AddOrUpdateCategories(context, "Kwiaty pod blokiem", "Ogrodnictwo");
            AddOrUpdateCategories(context, "Naprawa samochodu", "Inne");
            AddOrUpdateCategories(context, "K³opotliwa przeprowadzka", "Inne");
            AddOrUpdateCategories(context, "K³opotliwa przeprowadzka", "Transport");
            AddOrUpdateCategories(context, "Remont ³awek", "Remont");
            AddOrUpdateCategories(context, "Wyjœcie do lekarza", "Transport");
            AddOrUpdateCategories(context, "Pomoc przy zakupach", "Inne");
            AddOrUpdateCategories(context, "Pomoc przy zakupach", "Wolontariat");
            AddOrUpdateCategories(context, "Wyprzeda¿ gara¿owa", "Inne");
            AddOrUpdateCategories(context, "Warsztaty w domu dziecka", "Inne");
            AddOrUpdateCategories(context, "Warsztaty w domu dziecka", "Wolontariat");
            AddOrUpdateCategories(context, "Wolontariat w schronisku", "Wolontariat");
            AddOrUpdateCategories(context, "Dojazd na koncert", "Inne");
            AddOrUpdateCategories(context, "Dojazd na koncert", "Transport");
            AddOrUpdateCategories(context, "Dojazd na koncert", "Wolontariat");
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