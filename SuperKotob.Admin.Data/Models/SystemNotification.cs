using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models
{
    [Table("SystemNotification")]
  public  class SystemNotification:DataModel
    {
        [Display(Name = "Notification Type")]
       // [Required]
        public long NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Message")]
        [Required]
        public string Message { get; set; }

        [Display(Name = "For who?")]
        public string Note { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate
        {
            get;

            set;
        }


        //not maped fields
        [NotMapped]
        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        [NotMapped]
        [Display(Name = "Course")]
        public long? CourseId { get; set; }


        [NotMapped]
        [Display(Name = "Track")]
        public long? TrackId { get; set; }

        [NotMapped]
        [Display(Name = "Subject")]
        public long? SubjectId { get; set; }

        [NotMapped]
        [Display(Name = "Sub Category")]
        public long? SubCategoryId { get; set; }

    }
}
