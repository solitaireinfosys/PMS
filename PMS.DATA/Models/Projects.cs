using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PMS.DATA.Models
{
    public class Projects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public decimal EstimatedCost { get; set; }
        public bool PaymentReceived { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime DateCompleted { get; set; }
        public bool IsActive { get; set; }
        public string TechnologyStack { get; set; }
        public string ProjectType { get; set; }
        public bool IsCompleted { get; set; }
    }
}
