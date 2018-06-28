using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eDashboardModel
    {
        public int iPropId { get; set; }
        public eBookingOverviewGraphModel BookingOverviewChartModel { get; set; }
    }

    public class eDashBoardNotifications
    {
        public List<Noti_Dates> lstOne { get; set; }
        public List<Noti_Dates> lstTwo { get; set; }
        public List<Noti_Dates> lstThree { get; set; }
        public List<string> lstAll { get; set; }
    }

    public class Noti_Dates
    {
        public string sDate { get; set; }
    }
    //public class Notification
    //{
    //    public string sNotification { get; set; }
    //}
}