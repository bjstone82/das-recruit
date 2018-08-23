using System;

namespace Esfa.Recruit.Vacancies.Client.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        private const string DisplayDateFormat = "dd MMM yyyy";
        
        public static string AsGdsDate(this DateTime date)
        {
            return date.ToString(DisplayDateFormat);
        }

        public static string ToMonthYearString(this DateTime date)
        {
            return date.Year == DateTime.MinValue.Year ? "Current" : date.ToString("MMM yyyy");
        }
    }
}