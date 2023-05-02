public static class VacationPeriodExtensions
{
    public static bool IsAnyIntersect(this VacationPeriod currentVacation, List<VacationPeriod> othersVacations) =>
        othersVacations.Any(vacation => vacation.IsIntersectsWith(currentVacation));

    public static bool IsMonthApart(this VacationPeriod period, VacationPeriod otherPeriod) =>
        period.Start < otherPeriod.Start
        ? otherPeriod.Start > period.End.AddMonths(1)
        : period.Start > otherPeriod.End.AddMonths(1);
}

