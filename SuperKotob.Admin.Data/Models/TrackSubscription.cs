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
    [Table("dbo.TrackSubscription")]
    public class TrackSubscription:DataModel
    {
        [Required]
        [Display(Name = "Track")]
        public long TrackId { get; set; }

        [Required]
        [Display(Name = "Student")]
        public long StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        [Display(Name = "Duration Expiration")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DurationExpiration { get; set; }


        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }

        public string ReferenceNumber { get; set; }

        public virtual  Track Track { get; set; }

    }
}
