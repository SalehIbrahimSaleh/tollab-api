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
    [Table("TrackPromotionCourse")]
    public  class TrackPromotionCourse : DataModel
    {
        public long? TrackPromotionId { get; set; }
        public long? CourseId { get; set; }
        public Course Course{ get; set; }
        public TrackPromotion TrackPromotion{ get; set; }
    }
}
