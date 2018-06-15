using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Models
{
    public class ProjectByDeptt
    {
        public string deptt { get; set; }
        public decimal totalProjectsCost { get; set; }
        public decimal totalReceivedCost { get; set; }
        public List<ProjectByTech> data { get; set; }

    }
}
