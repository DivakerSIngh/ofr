using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public abstract class BaseGraphModel
    {
        public int NumberOfDays { get; set; }

        public string sType { get; set; }

        public int TotalCompetitors { get; set; }
    }


    public class eBookingOverviewGraphModel : BaseGraphModel
    {
        public eBookingOverviewGraphModel()
        {
            TotalViews_Ours = new BookingOverviewGraphData();
            TotalViews_Competitors = new BookingOverviewGraphData();

            TotalBookings_Ours = new BookingOverviewGraphData();
            TotalBookings_Competitors = new BookingOverviewGraphData();

            TotalConversions_Ours = new BookingOverviewGraphData();
            TotalConversions_Competitors = new BookingOverviewGraphData();
        }

        public BookingOverviewGraphData TotalViews_Ours { get; set; }
        public BookingOverviewGraphData TotalViews_Competitors { get; set; }

        public BookingOverviewGraphData TotalBookings_Ours { get; set; }
        public BookingOverviewGraphData TotalBookings_Competitors { get; set; }

        public BookingOverviewGraphData TotalConversions_Ours { get; set; }
        public BookingOverviewGraphData TotalConversions_Competitors { get; set; }

    }


    public class eNegotiationOverviewGraphModel : BaseGraphModel
    {
        public eNegotiationOverviewGraphModel()
        {
            TotalAccepted_Ours = new NegotiationOverviewGraphData();
            TotalAccepted_Competitors = new NegotiationOverviewGraphData();

            TotalNegotiation_Ours = new NegotiationOverviewGraphData();
            TotalNegotiation_Competitors = new NegotiationOverviewGraphData();
        }

        public NegotiationOverviewGraphData TotalNegotiation_Ours { get; set; }
        public NegotiationOverviewGraphData TotalNegotiation_Competitors { get; set; }

        public NegotiationOverviewGraphData TotalAccepted_Ours { get; set; }
        public NegotiationOverviewGraphData TotalAccepted_Competitors { get; set; }

    }


    public class ePerformanceOverviewGraphModel : BaseGraphModel
    {
        public ePerformanceOverviewGraphModel()
        {
            RoomNights = new List<PerformaceGraphData>();
            Revenue = new List<PerformaceGraphData>();
            AvgDailyRate = new List<PerformaceGraphData>();
        }

        public List<PerformaceGraphData> RoomNights { get; set; }

        public List<PerformaceGraphData> Revenue { get; set; }

        public List<PerformaceGraphData> AvgDailyRate { get; set; }
    }


    public class eBookingInsightsGraphModel : BaseGraphModel
    {
        public eBookingInsightsGraphModel()
        {
            LeadTime = new List<BookingInsightsGraphData>();
            NoOfRooms = new List<BookingInsightsGraphData>();
            LengthOfStay = new List<BookingInsightsGraphData>();
            DayOfWeek = new List<BookingInsightsGraphData>();
            RoomProductivities = new List<RoomProductivityGraphData>();
        }

        public List<BookingInsightsGraphData> LeadTime { get; set; }
        public List<BookingInsightsGraphData> NoOfRooms { get; set; }
        public List<BookingInsightsGraphData> LengthOfStay { get; set; }
        public List<BookingInsightsGraphData> DayOfWeek { get; set; }
        public List<RoomProductivityGraphData> RoomProductivities { get; set; }
    }


    public class PerformaceGraphData
    {
        public string YearType { get; set; }
        public int Bids { get; set; }
        public int Buy { get; set; }
        public int Negotiations { get; set; }
        public int Corporate { get; set; }
        public int OverAll { get; set; }
    }

    public class BookingOverviewGraphData
    {
        public int Bookings { get; set; }

        public int Negotiations { get; set; }

        public int Bids { get; set; }

        public int Corporate { get; set; }

    }

    public class NegotiationOverviewGraphData
    {
        public int Accepted { get; set; }

        public int Rejected { get; set; }

        public int CounterOffer { get; set; }

        public int NoAction { get; set; }
    }

    public class BookingInsightsGraphData
    {
        public string DataFor { get; set; }
        public int Y0 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
        public int Y3 { get; set; }
        public int Y4 { get; set; }
        public int Y5 { get; set; }
        public int Y6 { get; set; }
        public int Y7 { get; set; }
        public int Y8 { get; set; }
        public int Y9 { get; set; }

        public int Y0_Overall { get; set; }
        public int Y1_Overall { get; set; }
        public int Y2_Overall { get; set; }
        public int Y3_Overall { get; set; }
        public int Y4_Overall { get; set; }
        public int Y5_Overall { get; set; }
        public int Y6_Overall { get; set; }
        public int Y7_Overall { get; set; }
        public int Y8_Overall { get; set; }
        public int Y9_Overall { get; set; }
    }

    public class RoomProductivityGraphData
    {
        public int PropId { get; set; }

        public string RoomName { get; set; }

        public int Value { get; set; }
    }


    public class HotelRank
    {
        public int MyRank { get; set; }

        public int Hotels { get; set; }
    }
}