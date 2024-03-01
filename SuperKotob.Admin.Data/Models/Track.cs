using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Track")]
    public class Track : DataModel
    {
        public Track()
        {
            this.TrackPromotions = new List<TrackPromotion>();
        }
        [Display(Name = "Track-Subject")]
        public string TrackSubject { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

        [Display(Name = "Teacher")]
        [Required]
        public long? TeacherId { get; set; }

        [Display(Name = "Subject")]
        [Required]
        public long? SubjectId { get; set; }

        [Display(Name = "Track Cover")]
        public string Image { get; set; }
        [Display(Name = "Home Cover")]
        public string ImageHomeCover { get; set; }
        [Display(Name = "Subscription Duration")]
        public int? SubscriptionDuration { get; set; }

        [Display(Name = "Subscription Current Price")]
        public decimal? SubscriptionCurrentPrice { get; set; }

        [Display(Name = "Subscription Old Price")]
        public decimal? SubscriptionOldPrice { get; set; }

        [Display(Name = "Order Number")]
        public int? OrderNumber { get; set; }

        [Required]
        [Display(Name = "By Subscription")]
        public bool? BySubscription { get; set; }


        [Display(Name = "Show Water Mark")]
        public bool? ShowWaterMark { get; set; }
        public string TrackCode { get; set; }


        [Display(Name = "SKU Number")]
        public string SKUNumber { get; set; }
        [Display(Name = "SKU Price")]
        public decimal? SKUPrice { get; set; }
        [Display(Name = "Old SKU Price")]
        public decimal? OldSKUPrice { get; set; }
        
        [Display(Name = "Whatsup Group Link")]
        public string WhatsupGroupLink { get; set; }
        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }
        public decimal? CachedPrice { get; set; }
       public string DiscountType { get; set; }
        public float? DiscountValue { get; set; }
        public Teacher Teacher { get; set; }

        public Subject Subject { get; set; }
        public List<TrackPromotion> TrackPromotions { get; set; }
        public List<Course> Courses{ get; set; }
    }
}
