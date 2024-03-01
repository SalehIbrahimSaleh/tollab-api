using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Models
{
    public class LookupItem
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        private string _Text;

        public string Title { get; set; }

        public string Text
        {
            get
            { 
                return ""+Title.ToString();
            }

            set
            {
                _Text = value;
            }
        }
    }
}
