using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class TransactionRow : Row
    {
        public string Referens { get; set; }
        public string Text { get; set; }
    }
}
