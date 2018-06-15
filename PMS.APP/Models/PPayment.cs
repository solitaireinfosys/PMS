using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Models
{
    public class PPayment
    {
        public int ProjectId { get; set; }
        public Nullable<bool> PaymentReceived { get; set; }
    }
}
