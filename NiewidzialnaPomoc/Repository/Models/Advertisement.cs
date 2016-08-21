using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class Advertisement
    {
        public Advertisement()
        {
            this.Categories = new HashSet<Category>();
            this.Helpers = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }

        [Display(Name = "Tytuł")]
        [MaxLength(72)]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        [MaxLength(500)]
        public string Content { get; set; }

        [Display(Name = "Data dodania")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime AddDate { get; set; }

        [Display(Name = "Trudność")]
        public int DifficultyId { get; set; }

        [Display(Name = "Wykonanie")]
        public int PerformanceId { get; set; }

        //[NotMapped]
        //[Display(Name = "Punkty")]
        //public int Points { get { return Performance.Points + Difficulty.Points; } }

        [Display(Name = "Autor")]
        public string AuthorId { get; set; }

        [Display(Name = "Miejscowość")]
        public int LocationId { get; set; }

        public bool IsFinished { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        [Display(Name = "Pomocnicy")]
        public virtual ICollection<ApplicationUser> Helpers { get; set; }
        public virtual Difficulty Difficulty { get; set; }
        public virtual Performance Performance { get; set; }
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }
        public virtual Location Location { get; set; }
    }
}