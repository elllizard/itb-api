using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using itb_api.Models.Database;
using itb_api.Services.Statistic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace itb_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IStatisticService _statisticsService;

        public StatisticsController(
            ILogger<StatisticsController> logger,
            IStatisticService statisticsService
        )
        {
            _logger = logger;
            _statisticsService = statisticsService;
        }

        [HttpGet]
        [Route("{username1}/{username2?}")]
        [Produces("application/json")]
        public async Task<IActionResult> ReadStatistic(string username1, string? username2 = "")
        {
            _logger.LogInformation(
                $"Received read request for statistic with username '{username1}' and username '{username2}'.");
            try
            {
                List<StatisticsCollection> statistics = await _statisticsService.ReadBulkAsync(username1, username2);
                if (statistics.Count > 0)
                {
                    return Ok(statistics);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}