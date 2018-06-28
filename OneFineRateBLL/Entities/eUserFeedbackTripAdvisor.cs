using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblUserFeedbackTripAdvisor
    {
        public long iUserId { get; set; }
        public long iBookingId { get; set; }
        public Nullable<System.DateTime> dtReminderSentOn { get; set; }
        public Nullable<System.DateTime> dtFeedbackGivenAt { get; set; }
    }
}