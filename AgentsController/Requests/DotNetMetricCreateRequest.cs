using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Requests
{
    public class DotNetMetricCreateRequest
    {
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public int Count { get; set; }
    }
}
