using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal
{
    public class NotificationException : Exception
    {
        public NotificationException(string message)
            : base(message)
        { }
    }
}
