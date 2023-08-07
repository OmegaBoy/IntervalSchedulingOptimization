using IntervalSchedulingOptimization;
using Microsoft.AspNetCore.Mvc;

namespace IntervalSchedulingOptimizationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntervalOptimizerController : ControllerBase
    {
        private readonly ILogger<IntervalOptimizerController> _logger;

        public IntervalOptimizerController(ILogger<IntervalOptimizerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IEnumerable<IEnumerable<Interval>> Optimize(IntervalOptimizerDTO intervalOptimizerDTO)
        {
            _logger.Log(LogLevel.Debug, "test");
            return IntervalSchedulingService.ScheduleIntervals(intervalOptimizerDTO.Intervals, intervalOptimizerDTO.PreDefinedIntervalSets, intervalOptimizerDTO.RestrictionIntervals);
        }
    }
}