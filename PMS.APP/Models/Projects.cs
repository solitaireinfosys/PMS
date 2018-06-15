using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMS.APP.Models
{
    public class Projects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Client { get; set; }
        [Required]
        public decimal EstimatedCost { get; set; }
        [Required]
        public bool PaymentReceived { get; set; }
        public Nullable<DateTime> DatePaymentReceived { get; set; }
        [Required]
        public DateTime DateAssigned { get; set; }
        public Nullable<DateTime> DateCompleted { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string TechnologyStack { get; set; }
        [Required]
        public string ProjectType { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string Status { get; set; }

        private const string DEFAULT = "";
        private string _notes = DEFAULT;
        [DefaultValue(DEFAULT)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public ICollection<Milestones> Milestones { get; set; }
    }
}
