using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Models
{
    public class ProjectByTech
    {
        public string tech { get; set; }
        public decimal TotalEstimatedCost { get; set; }
        public decimal TotalReceived { get; set; }
        public List<ProjectMilestones> projects { get; set; }

    }
}
