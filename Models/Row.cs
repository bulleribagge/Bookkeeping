using System;

namespace Bookkeeping.Models
{
    public class Row
    {
        public DateTime RegisterDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CurrencyDate { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Reference { get; set; }
        public string PhoneNumber { get; set; }
        public decimal AmountExludingVAT => Amount / 1.25M;
    }
}
