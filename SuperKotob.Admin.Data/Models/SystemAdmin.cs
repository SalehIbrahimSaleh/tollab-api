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
    [Table("SystemAdmin")]
    public class SystemAdmin:DataModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string IdentityId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int? Type { get; set; }

        [NotMapped]
        public string IsType
        {
            get
            {
                if (Type == null)
                    return "Not Set";
                if (Type == 1)
                    return "Admin";
                //if (Type == 2)
                //    return "Delivery Manager";

                return "Nothing";
            }
            set
            {
            }
        }
        //not mapped 
        [NotMapped]
        [Display(Name = "password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }
}
