using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBank.Models
{
    public class CurrencyItem
    {
        public int CurrencyItemId { get; set; }
        public string CurrencyShort { get; set; }

        public string CurrencyLong { get; set; }
    }
}
