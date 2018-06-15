using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Models
{
    public class MPayment
    {
        [Required]
        public int MilestoneId { get; set; }
        [Required]
        public Nullable<bool> PaymentReceived { get; set; }
        [Required]
        public decimal RecievedAmount { get; set; }
    }
}
