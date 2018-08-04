using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class Row
    {
        public DateTime Bokföringsdag { get; set; }
        public DateTime Transaktionsdag { get; set; }
        public DateTime Valutadag { get; set; }
        public decimal Belopp { get; set; }
    }
}
