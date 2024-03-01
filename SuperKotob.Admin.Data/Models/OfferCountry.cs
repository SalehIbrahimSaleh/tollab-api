using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.Data.Models
{
    [Table("OfferCountry")]
    public class OfferCountry : DataModel
    {
        public long OfferId { get; set; }
        public long CountryId { get; set; }
    }
}
