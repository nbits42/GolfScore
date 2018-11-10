using System;
using System.Globalization;

namespace GolfScore.Contracts
{
    public interface ICalendarService
    {
        void AddEventToCalendar(CultureInfo ci, DateTime @from, DateTime until, string name, string location);
    }
}
