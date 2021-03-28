﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        [HttpGet("available/{available}")]
        public IActionResult GetMetricsFromAgent([FromRoute] float available)
        {
            return Ok();
        }
    }
}
