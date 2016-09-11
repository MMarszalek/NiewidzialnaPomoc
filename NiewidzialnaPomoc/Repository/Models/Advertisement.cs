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
            this.AdvertisementPhotos = new HashSet<AdvertisementPhoto>();
        }

        [Required]
        public int Id { get; set; }

        [Display(Name = "Tytuł")]
        [Required]
        [MaxLength(72)]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        [Required]
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Data dodania")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime AddDate { get; set; }

        [Display(Name = "Trudność")]
        [Required]
        public int DifficultyId { get; set; }

        [Display(Name = "Wykonanie")]
        public int PerformanceId { get; set; }

        [Display(Name = "Autor")]
        public string AuthorId { get; set; }

        [Display(Name = "Miejscowość")]
        [Required]
        public int LocationId { get; set; }

        public bool IsFinished { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        [Display(Name = "Pomocnicy")]
        public virtual ICollection<ApplicationUser> Helpers { get; set; }

        public virtual ICollection<AdvertisementPhoto> AdvertisementPhotos { get; set; }

        public virtual Difficulty Difficulty { get; set; }

        public virtual Performance Performance { get; set; }

        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }

        public virtual Location Location { get; set; }
    }
}