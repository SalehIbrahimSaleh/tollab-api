using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SuperMatjar.WebProtector.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMatjar.WebProtector
{
    public class ProtectedUser : IdentityUser, IProtectedUser, IUser<string>
    {
      //  [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

    //    public byte?  { get; set; }
        

        public DateTime? JoinDate { get; set; }

        public long? BusinessRoleId { get; set; }
        public long? BusinessUserId { get; set; }
       
       // public long? CustomerId { get;  set; }

        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public string IdentityId { get; set; }
    }
}
