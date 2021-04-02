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
    public class RamMetricsController : ControllerBase
    {
        private IRamMetricsRepository repository;

        public RamMetricsController(IRamMetricsRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            repository.Create(new RamMetric
            {
                Available = request.Available
            });

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricDto { Available = metric.Available, Id = metric.Id });
            }

            return Ok(response);
        }
    }
}
