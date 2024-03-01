using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("VideoQuestion")]
    public class VideoQuestion : DataModel
    {
        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }

        [Display(Name = "Minute")]
        [Required]
        public double? Minute { get; set; }

         [Display(Name = "Image")]
        public string Image { get; set; }

        [Display(Name = "View My Account")]
        [Required]
        public bool? ViewMyAccount { get; set; }

        [Display(Name = "Content")]
        public long? ContentId { get; set; }

        [Display(Name = "Live")]
        public long? LiveId { get; set; }

        [Display(Name = "Creation Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate
        {
            get;

            set;
        }
        [NotMapped]
        public DateTime? CreationDate2
        {
            get
            {
                if (CreationDate != null)
                {
                    return CreationDate.Value.AddHours(3);
                }
                return CreationDate;
            }
        }
        [Display(Name = "Student")]
        [Required]
        public long? StudentId { get; set; }

        [Display(Name = "Voice")]
        public string Voice { get; set; }

        public Content Content { get; set; }

        public Student Student { get; set; }
        public Live Live { get; set; }
        [NotMapped]
        public string RepliesCount { get
            {
                if (Replies.Count>0)
                {
                    return "There are  " + Replies.Count + " replies";
                }
                return "No replies";
            }
        }
        public virtual ICollection<Reply>  Replies { get; set; }
    }
}
