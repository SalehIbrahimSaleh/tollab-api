using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("Content")]
    public class Content : DataModel
    {
     


        [Required]
        [Display(Name = "English Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameLT { get; set; }

        [Display(Name = "Youtube Link")]
        public string YoutubeLink { get; set; }

        [Display(Name = "Path")]
        public string Path { get; set; }
        
        [NotMapped]
        [Display(Name = "DailyMotion/VdoCipher Path")]
        public string NewPathTemp
        {    set;
            get;
            //set; 
        }
        [Display(Name = "ProviderType")]
        public string ProviderType { get; set; }

        [Required]
        [Display(Name = "Group ")]
        public long? GroupId { get; set; }

        [Required]
        [Display(Name = "Duration")]
        public double? Duration { get; set; }

        [Required]
        [Display(Name = "Content Type")]
        public long? ContentTypeId { get; set; }
        [NotMapped]
        public string IntroVideoTemp { get; set; }
        [NotMapped]
        public string NewPath
        {
            get {
                if(Path != null)
                {
                    if (ContentTypeId == 1 && Path.Contains("vimeo"))
                    {
                        return Path;
                    }
                    else if (ContentTypeId == 1 && !Path.Contains("vimeo"))
                    {
                        return "https://www.dailymotion.com/partner/x2p07qx/media/video/edit/" + Path;
                    }
                    else if (ContentTypeId == 2)
                    {
                        return "http://tollab.com/ws/CourseVideos/" + Path;
                    }
                    else return "#";
                }
                else return "#";
            }
                
        }
        [Required]
        [Display(Name = "Is Free")]
        public bool? IsFree { get; set; }

        public Group Group { get; set; }

        [Required]
        [Display(Name = "Order Number")]

        public int? OrderNumber { get; set; }
        public ContentType ContentType { get; set; }
        public List<StudentContent> StudentContents { get; set; }
        public List<VideoQuestion> VideoQuestions { get; set; }
        public string VideoUri { get; set; }
    }
}
