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
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricsRepository repository;

        public HddMetricsController(IHddMetricsRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] HddMetricCreateRequest request)
        {
            repository.Create(new HddMetric
            {
                Left = request.Left
            });

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricDto { Left = metric.Left, Id = metric.Id });
            }

            return Ok(response);
        }
    }
}
