public class VacationPeriod
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public VacationPeriod(DateTime start, DateTime end)
    {
        if (end < start) throw new ArgumentException("Конец периода не может быть раньше начала периода");
        Start = start;
        End = end;
    }

    public bool IsIntersectsWith(VacationPeriod other) =>
        Start <= other.End && other.Start <= End;  
}

