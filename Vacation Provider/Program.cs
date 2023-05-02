using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PracticTask1
{
    class Program
    {
        private static Random random = new Random();

        static void Main()
        {
            var employees = new string[]
            {
                "Иванов Иван Иванович",
                "Петров Петр Петрович",
                "Юлина Юлия Юлиановна",
                "Сидоров Сидор Сидорович",
                "Павлов Павел Павлович",
                "Георгиев Георг Георгиевич"
            };
            var currentYear = DateTime.Now.Year;
            var vacationDays = new int[] { 7, 14 };
            var totalVacationDays = 28;
            var employeeAndVacations = GetVacationPeriods(employees, currentYear, vacationDays, totalVacationDays);

            OutputInformationToConsole(employeeAndVacations);
            Console.ReadKey();
        }

        public static Dictionary<string, List<VacationPeriod>> GetVacationPeriods(string[] employees, int year, int[] vacationDays, int totalVacationDays)
        {
            var employeeAndVacations = new Dictionary<string, List<VacationPeriod>>();
            var vacationsOfAllEmployees = new List<VacationPeriod>();
            foreach (var employee in employees)
            {
                var vacationPeriods = new List<VacationPeriod>();
                var vacationDaysCount = totalVacationDays;
                while (vacationDaysCount > 0)
                {
                    var vacationPeriod = GetRandomVacationPeriod(vacationDaysCount, year, vacationDays);

                    if (!IsOverlapsByThreeDays(vacationsOfAllEmployees, vacationPeriod)
                        && IsOneMonthBetweenVacations(vacationPeriods, vacationPeriod))
                    {
                        vacationPeriods.Add(vacationPeriod);
                        vacationsOfAllEmployees.Add(vacationPeriod);
                        vacationDaysCount -= (vacationPeriod.End - vacationPeriod.Start).Days;
                    }
                }
                vacationPeriods = vacationPeriods.OrderBy(x => x.Start).ToList();
                employeeAndVacations.Add(employee, vacationPeriods);
            }
            return employeeAndVacations;
        }
        private static VacationPeriod GetRandomVacationPeriod(int vacationDaysCount, int year, int[] vacationDays)
        {
            while (true)
            {
                var startDate = GetRandomWorkDate(year);
                var endDate = vacationDaysCount <= 7
                    ? startDate.AddDays(vacationDaysCount)
                    : startDate.AddDays(GetRandomElement(vacationDays));
                if (endDate.Year == year) 
                    return new VacationPeriod(startDate, endDate);
            }
        }

        private static DateTime GetRandomWorkDate(int year)
        {
            var randomDate = (DateTime?)null;
            var daysPerYear = DateTime.IsLeapYear(year) ? 366 : 365;
            while (randomDate == null || !IsWorkDay(randomDate.Value.DayOfWeek))
            {
                var randomDay = random.Next(daysPerYear);
                randomDate = new DateTime(year, 1, 1).AddDays(randomDay);
            }
            return randomDate.Value;

            bool IsWorkDay(DayOfWeek dayOfWeek) =>
            dayOfWeek != DayOfWeek.Sunday && dayOfWeek != DayOfWeek.Saturday;
        }

        private static bool IsOverlapsByThreeDays(List<VacationPeriod> vacationPeriods,
            VacationPeriod vacationPeriod)
        {
            var extendedPeriod = new VacationPeriod(vacationPeriod.Start.AddDays(-3), vacationPeriod.End.AddDays(3));
            return extendedPeriod.IsAnyIntersect(vacationPeriods);
        }

        private static bool IsOneMonthBetweenVacations(List<VacationPeriod> vacationPeriods, VacationPeriod currentPeriod) =>
                vacationPeriods.All(period => period.IsMonthApart(currentPeriod));

        private static void OutputInformationToConsole(Dictionary<string, List<VacationPeriod>> employeesAndVacations)
        {
            foreach (var employeeAndVacations in employeesAndVacations)
            {
                Console.WriteLine("Дни отпуска " + employeeAndVacations.Key + " : ");
                foreach (var date in GetVacationDates(employeeAndVacations.Value))
                    Console.WriteLine(date);

            }

            IEnumerable<DateTime> GetVacationDates(List<VacationPeriod> vacationsPeriods)
            {
                foreach (var period in vacationsPeriods)
                    for (var date = period.Start; date < period.End; date = date.AddDays(1))
                        yield return date;
            }
        }

        private static int GetRandomElement(int[] elements) =>
            elements[random.Next(elements.Length)];
    }
}

