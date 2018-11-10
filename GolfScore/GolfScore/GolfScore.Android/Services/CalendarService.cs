using System;
using System.Globalization;
using Android.Content;
using Android.Provider;
using GolfScore.Contracts;
using GolfScore.Droid.Services;
using GregorianCalendar = Java.Util.GregorianCalendar;

[assembly: Xamarin.Forms.Dependency(typeof(CalendarService))]

namespace GolfScore.Droid.Services
{
    public class CalendarService: ICalendarService
    {
        public void AddEventToCalendar(CultureInfo ci, DateTime @from, DateTime until, string name, string location)
        {
            
            var intent = new Intent(Intent.ActionInsert);
            var dateFrom = new GregorianCalendar(from.Year, from.Month - 1, from.Day, 0,0);
            var dateUntil = new GregorianCalendar(until.Year, until.Month - 1, until.Day, 23,59);

            try
            {
                intent.PutExtra(CalendarContract.Events.InterfaceConsts.Title, name);
                intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventLocation, location);
                intent.PutExtra(CalendarContract.ExtraEventAllDay, true);
                intent.PutExtra(CalendarContract.ExtraEventBeginTime, dateFrom.TimeInMillis);
                intent.PutExtra(CalendarContract.ExtraEventEndTime, dateUntil.TimeInMillis);
                intent.SetData(CalendarContract.Events.ContentUri);
                MainActivity.Instance.StartActivity(intent);
            }
            catch (Exception)
            {
                // shht
            }
            
        }
    }
}