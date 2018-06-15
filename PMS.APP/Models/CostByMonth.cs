using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Models
{
    public class CostByMonth
    {
        public string month { get; set; }
        public int year { get; set; }
        public int projectCount { get; set; }
        public decimal EstimatedCost { get; set; }
    }

    public class CostByMonthList
    {
        public string tech { get; set; }
        public List<CostByMonth> data { get; set; }
    }
}
