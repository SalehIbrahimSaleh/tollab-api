using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector.Core
{
    public interface IProtectedUser 
    {
        string FirstName { get; set; }        
        string LastName { get; set; }
        DateTime? JoinDate { get; set; }
        string UserName { get; set; }

        string PhoneNumber { get; set; }
        string Email { get; set; }
        long? BusinessRoleId { get; set; }
         long? BusinessUserId { get; set; }
       
         
      //  long? CustomerId { get; set; }

        [NotMapped]
        string IdentityId { get; set; }

        string Id { get; set; }
    }
}
