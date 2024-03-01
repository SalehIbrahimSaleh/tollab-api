using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("TrackPromotion")]
    public  class TrackPromotion : DataModel
    {
        public TrackPromotion()
        {
            this.TrackPromotionCourses = new List<TrackPromotionCourse>();
        }
        public string Image { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string CoursesConcat { get; set; }
        public long? TrackId { get; set; }
        public DateTime  PromotionEndDate { get; set; }
        public DateTime PromotionStartDate { get; set; }
        public bool IsShowInMobile { get; set; }
        public bool IsPromoFullyUpdateByNewCourses { get; set; }
        public string DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? ChachedPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? SkuPrice { get; set; }
        public decimal? SkuNumber { get; set; }
        public string Description { get; set; }
        public Track Track{ get; set; }
 
        public List<TrackPromotionCourse> TrackPromotionCourses { get; set; }
    }
}
