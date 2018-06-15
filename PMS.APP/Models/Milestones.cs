using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMS.APP.Models
{
    public class Milestones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MilestoneId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [DisplayName("TotalAmount")]
        public decimal Amount { get; set; }
        [Required]
        public decimal RecievedAmount { get; set; }
        public bool PaymentReceived { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public Nullable<DateTime> DatePaymentReceived { get; set; }
        [ForeignKey("Projects")]
        [Required]
        public int ProjectId { get; set; }
        //public Projects Projects { get; set; }

        private const string DEFAULT = "";
        private string _notes = DEFAULT;
        [DefaultValue(DEFAULT)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
    }
}
