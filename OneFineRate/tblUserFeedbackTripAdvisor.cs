//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OneFineRate
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblUserFeedbackTripAdvisor
    {
        public long iUserId { get; set; }
        public long iBookingId { get; set; }
        public Nullable<System.DateTime> dtReminderSentOn { get; set; }
        public Nullable<System.DateTime> dtFeedbackGivenAt { get; set; }
    }
}