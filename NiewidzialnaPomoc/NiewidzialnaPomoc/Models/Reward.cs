using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NiewidzialnaPomoc.Models
{
    public class Reward
    {
        public Reward()
        {
            this.RewardCodes = new HashSet<RewardCode>();
        }

        public int Id { get; set; }

        [Display(Name = "Nazwa nagrody:")]
        public string Name { get; set; }

        [Display(Name = "Cena:")]
        public int Price { get; set; }

        public virtual ICollection<RewardCode> RewardCodes { get; private set; }
    }
}