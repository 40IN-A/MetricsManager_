using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DTO;

namespace MetricsAgent.Responses
{
    public class AllDotNetMetricsResponse
    {
        public List<DotNetMetricDto> Metrics { get; set; }
    }
}
