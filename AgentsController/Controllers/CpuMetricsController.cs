using MetricsAgent.DAL;
using MetricsAgent.Model;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository repository;
        private readonly ILogger<CpuMetricsController> _logger;

        public CpuMetricsController(ICpuMetricsRepository repository, ILogger<CpuMetricsController> logger)
        {
            this.repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
        }

        //public CpuMetricsController(ICpuMetricsRepository repository)
        //{
        //    this.repository = repository;
        //}

        //private readonly ILogger<CpuMetricsController> _logger;
        //public CpuMetricsController(ILogger<CpuMetricsController> logger)
        //{
        //    _logger = logger;
        //    _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
        //}

        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            repository.Create(new CpuMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            _logger.LogInformation($"Create: Time - {request.Time}, Value - {request.Value}");

            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto { Time = metric.Time.Minutes, Value = metric.Value, Id = metric.Id });
                _logger.LogInformation($"GetAll: Time - {metric.Time.Minutes}, Value - {metric.Value}");
            }            

            return Ok(response);
        }
    }
}