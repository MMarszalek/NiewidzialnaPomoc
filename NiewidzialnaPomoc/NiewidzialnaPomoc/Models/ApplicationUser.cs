using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace NiewidzialnaPomoc.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.MyAdvertisements = new HashSet<Advertisement>();
            this.HelpAdvertisements = new HashSet<Advertisement>();
        }

        [Display(Name = "Imię:")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko:")]
        public string LastName { get; set; }

        [Display(Name = "Liczba zdobytych punktów:")]
        public int Points { get; set; }

        //ogloszenia, ktorych uzytkownik jest autorem
        public virtual ICollection<Advertisement> MyAdvertisements { get; private set; }

        //ogloszenia, przy ktorych uzytkownik pomagal
        public virtual ICollection<Advertisement> HelpAdvertisements { get; private set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}