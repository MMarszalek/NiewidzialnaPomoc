using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NiewidzialnaPomoc.Models
{
    public class Location
    {
        public Location()
        {
            this.ApplicationUser = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUser { get; private set; }
    }
}