using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Utils.Helpers
{
    public class DateHelper
    {
        public static string FormatElapsedTime(string datetime)
        {
            DateTime dt;
            if (!DateTime.TryParse(datetime, out dt))
                return datetime;

            return FormatElapsedTime(dt);

        }
        public static string FormatElapsedTime(DateTime datetime)
        {
            TimeSpan ts = DateTimeOffset.Now.Subtract(datetime);

            int years = ts.Days / 365;
            int months = ts.Days / 30;
            int weeks = ts.Days / 52;

            if (years == 1) // one year ago
                return "A year ago";

            if (years > 1) // greater than one year
            {
                if (ts.Days % 365 == 0) // even year
                    return (int)(ts.TotalDays / 365) + " years ago";
                else // not really entire years
                    return "About " + (int)(ts.TotalDays / 365) + " years ago";
            }

            if (months == 1) // one month
                return "About a month ago";

            if (months > 1) // more than one month
                return "About " + months + " months ago";

            if (weeks == 1) // a week ago
                return "About a week ago";

            if (weeks > 1) // more than a week ago, but less than a month ago
                return "About " + weeks + " weeks ago";

            if (ts.Days == 1) // one day ago
                return "Yesterday";

            if (ts.Days > 1) //  more than one day ago, but less than one week ago
                return ts.Days + " days ago";

            if (ts.Hours == 1) // An hour ago
                return "About an hour ago";

            if (ts.Hours > 1 && ts.Hours <= 24) // More than an hour ago, but less than a day ago
                return "About " + ts.Hours + " hours ago";

            if (ts.Minutes == 1)
                return "About a minute ago";

            if (ts.Minutes == 0)
                return ts.Seconds + " seconds ago";

            return ts.Minutes + " minutes ago";
        }
    }
}
