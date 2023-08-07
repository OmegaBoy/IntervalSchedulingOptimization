using IntervalSchedulingOptimization;

namespace IntervalSchedulingOptimizationAPI.Controllers
{
    public class IntervalOptimizerDTO
    {
        public List<Interval> Intervals { get; set; }
        public List<List<Interval>>? PreDefinedIntervalSets { get; set; } = null;
        public List<Interval>? RestrictionIntervals { get; set; } = null;
    }
}
