using System;
using Android.App;
using Android.Text.Format;

namespace Android.Dialog
{
    public class DateTimeElement : StringElement
    {
        public int MinuteInterval { get; set; }

        public DateTime? DateValue
        {
            get
            {
                DateTime dt;
                if (DateTime.TryParse(Value, out dt))
                    return dt;
                return null;
            }
            set { Value = Format(value); }
        }
        public DateTimeElement(string caption, DateTime? date)
            : base(caption)
        {
            Click = delegate { EditDate(); };
            DateValue = date;
        }

        public DateTimeElement(string caption, DateTime? date, int layoutId)
            : base(caption, layoutId)
        {
            Click = delegate { EditDate(); };
            DateValue = date;
        }

        public virtual string Format(DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToShortDateString() + " " + dt.Value.ToShortTimeString();

            return string.Empty;
        }

        protected void EditDate()
        {
            var context = GetContext();
            if (context == null)
            {
                Util.Log.Warn("DateElement", "No Context for Edit");
                return;
            }
            var val = DateValue.HasValue ? DateValue.Value : DateTime.Now;
            new DatePickerDialog(context, DateCallback ?? OnDateTimeSet, val.Year, val.Month - 1, val.Day).Show();
        }

        protected void EditTime()
        {
            var context = GetContext();
            if (context == null)
            {
                Util.Log.Warn("TimeElement", "No Context for Edit");
                return;
            }
            DateTime val = DateValue.HasValue ? DateValue.Value : DateTime.Now;
            new TimePickerDialog(context, OnTimeSet, val.Hour, val.Minute, DateFormat.Is24HourFormat(context)).Show();
        }

        // the event received when the user "sets" the date in the dialog
        protected void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            DateTime current = DateValue.HasValue ? DateValue.Value : DateTime.Now;
            DateValue = new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0);
        }

        // the event received when the user "sets" the date in the dialog
        protected void OnDateTimeSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            DateTime current = DateValue.HasValue ? DateValue.Value : DateTime.Now;
            DateValue = new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0);
            EditTime();
        }

        // the event received when the user "sets" the time in the dialog
        protected void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            DateTime current = DateValue.HasValue ? DateValue.Value : DateTime.Now;
            DateValue = new DateTime(current.Year, current.Month, current.Day, e.HourOfDay, e.Minute, 0);
        }

        protected EventHandler<DatePickerDialog.DateSetEventArgs> DateCallback = null;
    }

    public class DateElement : DateTimeElement
    {
        public DateElement(string caption, DateTime? date)
            : base(caption, date)
        {
            DateCallback = OnDateSet;
        }

        public DateElement(string caption, DateTime? date, int layoutId)
            : base(caption, date, layoutId)
        {
            DateCallback = OnDateSet;
        }

        public override string Format(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToShortDateString() : string.Empty;
        }
    }

    public class TimeElement : DateTimeElement
    {
        public TimeElement(string caption, DateTime? date)
            : base(caption, date)
        {
            Click = delegate { EditTime(); };
        }

        public TimeElement(string caption, DateTime? date, int layoutId)
            : base(caption, date, layoutId)
        {
            Click = delegate { EditTime(); };
        }

        public override string Format(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToShortTimeString() : string.Empty;
        }
    }
}