using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class SwishRow : Row
    {
        public string Avsändarnummer { get; set; }
        public string Avsändarnamn { get; set; }
        public decimal BeloppUtanMoms => Belopp / 1.25M;
    }
}
