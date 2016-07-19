using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class Difficulty
    {
        public Difficulty()
        {
            this.Advertisements = new HashSet<Advertisement>();
        }

        public int Id { get; set; }

        [Display(Name = "Trudność")]
        public string Name { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}