using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Models
{
    public class DotNetMetric
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public TimeSpan FromTime { get; set; }

        public TimeSpan ToTime { get; set; }
    }
}
