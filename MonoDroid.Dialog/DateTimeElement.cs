using System;
using Android.Content;

namespace MonoDroid.Dialog
{
    public class DateTimeElement : StringElement
    {
        public DateTime DateValue;
        private Context _context;

        public DateTimeElement(Context context, string caption, DateTime date)
            : base(context, caption)
        {
            _context = context;
            DateValue = date;
            Value = FormatDate(date);
        }

        public virtual string FormatDate(DateTime dt)
        {
            ///FIXME
            // return fmt.ToString(dt) + " " + dt.ToLocalTime().ToShortTimeString();
            return dt.ToString();
        }

        public override void Selected()
        {
            
        }
    }

    public class DateElement : DateTimeElement
    {
        public DateElement(Context context, string caption, DateTime date) : base(context, caption, date)
        {
        }
    }

    public class TimeElement : DateTimeElement
    {
        public TimeElement(Context context, string caption, DateTime date) : base(context, caption, date)
        {
        }
    }
}