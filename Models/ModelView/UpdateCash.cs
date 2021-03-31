using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBank.Models
{
    public class UpdateCash
    {
        public int UpdateRecordId { get; set; }

        public double DeltaCash { get; set; }
    }
}
