namespace IntervalSchedulingOptimization
{
    public class Interval
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Priority { get; set; }
        public bool Restricted { get; set; }

        public Interval(int start, int end, int priority = 1, bool restricted = false)
        {
            Start = start;
            End = end;
            Priority = priority;
            Restricted = restricted;
        }
    }
}
