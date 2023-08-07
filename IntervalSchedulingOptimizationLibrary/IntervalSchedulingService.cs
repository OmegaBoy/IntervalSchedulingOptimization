namespace IntervalSchedulingOptimization
{
    public class IntervalSchedulingService
    {
        public static List<List<Interval>> ScheduleIntervals(List<Interval> intervals, List<List<Interval>>? preDefinedIntervalSets = null, List<Interval>? restrictionIntervals = null)
        {
            // Sort intervals by end time
            intervals.Sort((a, b) => a.End.CompareTo(b.End));

            List<List<Interval>> sets = new();
            if (preDefinedIntervalSets?.Any() ?? false) sets.AddRange(preDefinedIntervalSets);
            if (restrictionIntervals?.Any() ?? false) sets.ForEach(s => restrictionIntervals.ForEach(ri => s.Add(ri)));

            List<Interval> rejectedIntervals = new();

            foreach (var intervalByPriority in intervals.GroupBy(i => i.Priority).OrderByDescending(gi => gi.Key))
            {
                foreach (Interval interval in intervalByPriority)
                {
                    bool isScheduled = false;

                    // Check if the current interval can be added to an existing set
                    foreach (List<Interval> set in sets)
                    {
                        bool isOverlap = set.Any(i => i.Start <= interval.End && i.End >= interval.Start);

                        if (!isOverlap)
                        {
                            set.Add(interval);
                            isScheduled = true;
                            break;
                        }
                    }

                    // If the interval couldn't be added to an existing set, create a new set
                    if (!isScheduled)
                    {
                        // Check if interval is in the restricted intervals
                        bool isOverlap = restrictionIntervals?.Any(i => i.Start <= interval.End && i.End >= interval.Start) ?? false;
                        if (isOverlap)
                        {
                            rejectedIntervals.Add(interval);
                        }
                        else
                        {
                            List<Interval> newSet = new() { interval };
                            if (restrictionIntervals?.Any() ?? false) newSet.AddRange(restrictionIntervals);
                            sets.Add(newSet);
                        }
                    }
                }
            }

            return sets;
        }

        public static List<Interval> CompleteIntervals(List<Interval> partialIntervals, int maxEndTime)
        {
            // Find the minimum start time from the provided partial intervals
            int minStartTime = partialIntervals.Min(i => i.Start);

            // Generate missing intervals from minStartTime to maxEndTime
            List<Interval> missingIntervals = new List<Interval>();
            for (int i = minStartTime; i < maxEndTime; i++)
            {
                bool isMissing = partialIntervals.All(interval => interval.Start > i || interval.End <= i);
                if (isMissing)
                {
                    int priority = partialIntervals.Any(interval => interval.Start <= i && interval.End > i) ? 1 : 0;
                    missingIntervals.Add(new Interval(i, i + 1) { Priority = priority });
                }
            }

            return partialIntervals.Concat(missingIntervals).ToList();
        }
    }
}
