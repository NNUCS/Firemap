using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremapHelper
{
    public class FiremapData
    {
        public int? Iteration { get; set; }
        public decimal? ROS { get; set; }
        public decimal? FL { get; set; }
        public decimal? FLIN { get; set; }
        public decimal? HPA { get; set; }
        public decimal? WAF { get; set; }
        public int? ModelNumber { get; set; }

        public int? ErrorNumber { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
