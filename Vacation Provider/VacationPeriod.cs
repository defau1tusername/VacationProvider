public class VacationPeriod
{
    public readonly DateTime Start;
    public readonly DateTime End;

    public VacationPeriod(DateTime start, DateTime end)
    {
        if (end < start) throw new ArgumentException("Конец периода не может быть раньше начала периода");
        Start = start;
        End = end;
    }

    public bool IsIntersectWith(VacationPeriod other) =>
        Start <= other.End && other.Start <= End;  
}

