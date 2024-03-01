using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tollab.Admin.Core.Enums;

namespace Tollab.Admin.Data.Models
{
    [Table("Offer")]
    public class Offer : DataModel
    {
        

        [MaxLength]
        [Required]
        [Display(Name = "English Tilte")]
        public string Tilte { get; set; }

        [MaxLength]
        [Required]
        [Display(Name = "Arabic Title")]
        public string TitleLT { get; set; }

        [MaxLength]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [Display(Name = "Course")]
        public long? CourseId { get; set; }

        [Display(Name = "Offer content type")]
        public OfferContentTypeEnum? OfferContentTypeId { get; set; }

        [Display(Name = "Video")]
        public string VideoURL { get; set; }

        [Display(Name = "VideoURI")]
        public string VideoURI { get; set; }

        [MaxLength]
        [Display(Name = "Video Thumbnail")]
        public string VideoThumbnail { get; set; }

        [Display(Name = "Offer link type")]
        public OfferLinkTypeEnum? OfferLinkTypeId { get; set; }

        [Display(Name = "External link")]
        public string ExternalLink { get; set; }

        [Display(Name = "Order")]
        public int? OrderNumber { get; set; }

        [Display(Name = "Track")]
        public long? TrackId { get; set; }

        [Display(Name = "End Offer Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndOfferDate {
            get;

            set;
        }
        [NotMapped]
        [Display(Name = "Countries")]
        public IEnumerable<long> Countries { get; set; }

        [NotMapped]
        public DateTime? EndOfferDate2
        {
            get
            {
                if (EndOfferDate != null)
                {
                    return EndOfferDate.Value.AddHours(3);
                }
                return EndOfferDate;
            }
        }


        public Course Course { get; set; }
        public Track Track { get; set; }
        public List<OfferCountry> OfferCountries{ get; set; }
    }
}
