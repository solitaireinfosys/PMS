using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Models
{
    public class Pagination
    {
        const int maxPageSize = 10;

        public int pageNo { get; set; } = 1;

        public int _pageSize { get; set; } = 10;

        public int pageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string sort { get; set; } = "date";

        public string by { get; set; } = "desc";

        public string filter { get; set; } = "";

        public int month { get; set; } = 0;

        public int year { get; set; } = 0;

        public string tech { get; set; } = "";

        public int maxMonth { get; set; } = 0;

        public string dateFrom { get; set; } = "";

        public string dateTo { get; set; } = "";

    }
    
    
}
