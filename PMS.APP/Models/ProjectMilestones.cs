using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Models
{
    public class ProjectMilestones
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public decimal EstimatedCost { get; set; }
        public bool PaymentReceived { get; set; }
        public Nullable<DateTime> DatePaymentReceived { get; set; }
        public DateTime DateAssigned { get; set; }
        public Nullable<DateTime> DateCompleted { get; set; }
        public bool IsActive { get; set; }
        public string TechnologyStack { get; set; }
        public string ProjectType { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        //public string Feedback { get; set; }

        private const string DEFAULT = "";
        private string _notes = DEFAULT;
        [DefaultValue(DEFAULT)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public int TotalMilestones { get; set; }
        public decimal TotalMilestoneCost { get; set; }
        public decimal TotalMilestoneCostReceived { get; set; }

        public virtual ICollection<Milestones> Milestones { get; set; }
    }
}
