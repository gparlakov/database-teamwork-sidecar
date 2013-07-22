using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Models
{
    public class VendorExpence
    {
        public int Id { get; set; }

        public int VendorId { get; set; }

        public string Month { get; set; }

        public decimal Ammount { get; set; }
    }
}
