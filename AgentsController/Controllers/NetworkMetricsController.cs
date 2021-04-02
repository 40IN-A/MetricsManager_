using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DTO;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private INetworkMetricsRepository repository;

        public NetworkMetricsController(INetworkMetricsRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
        {
            repository.Create(new NetworkMetric
            {
                FromTime = request.FromTime,
                ToTime = request.ToTime,
            });

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetricDto { FromTime = metric.FromTime, ToTime = metric.ToTime, Id = metric.Id });
            }

            return Ok(response);
        }
    }
}
