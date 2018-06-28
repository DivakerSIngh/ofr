using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class ePendingChanges
    {
        public long ID { get; set; }
        public string sID { get; set; }
        public DateTime StayFrom { get; set; }
        public DateTime StayTo { get; set; }
        public Nullable<System.Decimal> NegotiateRate { get; set; }
        public Nullable<System.Decimal> ActualAmount { get; set; }
        public Nullable<System.Decimal> Discount { get; set; }
        public Nullable<System.Decimal> BidRate { get; set; }
        public DateTime Timer1 { get; set; }
        public DateTime CurrentTime { get; set; }
        public DateTime ActionDate { get; set; }
        public string RatePlanRoomType { get; set; }
        public string Timer { get; set; }
        public int Attempts { get; set; }
        public int NoOfRooms { get; set; }
        public string Symbol { get; set; }
        public string Code { get; set; }
    }

    public class eDataForNegotiationActionMail
    {
        public long ID { get; set; }
        public DateTime StayFrom { get; set; }
        public DateTime StayTo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int NoOfRooms { get; set; }
    }
}