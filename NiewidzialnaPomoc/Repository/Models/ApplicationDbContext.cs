using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Repository.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<RewardCode> RewardCodes { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<AdvertisementPhoto> AdvertisementPhotos { get; set; }
    }
}