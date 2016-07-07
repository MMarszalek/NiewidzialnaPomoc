using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Repository.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.PersonalAdvertisements = new HashSet<Advertisement>();
            this.RewardedAdvertisements = new HashSet<Advertisement>();
            this.RewardCodes = new HashSet<RewardCode>();
        }

        [Display(Name = "Użytkownik:")]
        override public string UserName { get; set; }

        [Display(Name = "Imię:")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko:")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Pan/Pani:")]
        public string FullName { get { return FirstName + " " + LastName; } }

        [Display(Name = "Liczba zdobytych punktów:")]
        public int Points { get; set; }

        //ogloszenia, ktorych uzytkownik jest autorem
        [InverseProperty("Author")]
        public virtual ICollection<Advertisement> PersonalAdvertisements { get; set; }

        //ogloszenia, przy ktorych uzytkownik pomagal
        [InverseProperty("Helpers")]
        public virtual ICollection<Advertisement> RewardedAdvertisements { get; set; }

        public virtual ICollection<RewardCode> RewardCodes { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}