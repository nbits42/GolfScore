using System;
using System.Globalization;
using EventKit;
using EventKitUI;
using Foundation;
using GolfScore.Contracts;
using GolfScore.iOS.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CalendarService))]

namespace GolfScore.iOS.Services
{
    public class CalendarService: ICalendarService
    {
        protected EKEventStore EventStore;
        //protected CreateEventEditViewDelegate eventControllerDelegate; 
        public CalendarService()
        {
            EventStore = new EKEventStore();

        }

        public void AddEventToCalendar(CultureInfo ci, DateTime from, DateTime until, string name, string location)
        {
            try
            {
                EventStore.RequestAccess(EKEntityType.Event,
                    (granted, e) =>
                    {
                        if (granted)
                        {
                            UIApplication.SharedApplication.InvokeOnMainThread(() =>
                            {
                                var eventController = new EKEventEditViewController
                                {
                                    EventStore = EventStore
                                };

                                var eventControllerDelegate = new CreateEventEditViewDelegate(eventController);
                                eventController.EditViewDelegate = eventControllerDelegate;

                                var newEvent = EKEvent.FromStore(EventStore);
                                newEvent.StartDate = DateTimeToNsDate(from);
                                newEvent.EndDate = DateTimeToNsDate(until);
                                newEvent.AllDay = true;
                                newEvent.Location = location;
                                newEvent.Title = name;
                                newEvent.Calendar = EventStore.DefaultCalendarForNewEvents;

                                eventController.Event = newEvent;

                                UIApplication.SharedApplication.Windows[0].RootViewController
                                    .PresentViewController(eventController, true, null);
                            });
                        }
                    });
            }
            catch (Exception)
            {
                // shht
            }
        }

        public DateTime NsDateToDateTime(NSDate date)
        {
            // NSDate has a wider range than DateTime, so clip
            // the converted date to DateTime.Min|MaxValue.
            var secs = date.SecondsSinceReferenceDate;
            if (secs < -63113904000)
                return DateTime.MinValue;
            if (secs > 252423993599)
                return DateTime.MaxValue;
            return (DateTime)date;
        }

        public NSDate DateTimeToNsDate(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Local);// or DateTimeKind.Utc, this depends on each app */)
            return (NSDate)date;
        }
    }

    public class CreateEventEditViewDelegate : EKEventEditViewDelegate
    {
        // we need to keep a reference to the controller so we can dismiss it
        private readonly EKEventEditViewController _eventController;

        private bool _success;

        public bool Success => _success;

        public CreateEventEditViewDelegate(EKEventEditViewController eventController)
        {
            // save our controller reference
            this._eventController = eventController;
        }

        // completed is called when a user eith
        public override void Completed(EKEventEditViewController controller, EKEventEditViewAction action)
        {
            _eventController.DismissViewController(true, null);
            switch (action)
            {
                case EKEventEditViewAction.Canceled:
                    _success = false;
                    break;
                case EKEventEditViewAction.Saved:
                    _success = true;
                    break;
                case EKEventEditViewAction.Deleted:
                    _success = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}