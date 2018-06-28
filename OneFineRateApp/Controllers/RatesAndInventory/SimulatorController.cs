using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.RatesAndInventory
{
    public class JsonModelSimulatorItem
    {
        public string value { get; set; }

        [JsonProperty(PropertyName = "class")]
        public string _class { get; set; }
    }

    public class JsonModelSimulator
    {
        public List<JsonModelSimulatorItem> Items { get; set; }
    }


    public class SimulatorController : BaseController
    {
        [Route("Simulator")]
        public ActionResult Index(string BookingDate, string ArrivalDate, string LOS, string Rooms, string Occupancy, string Type)
        {
            ViewBag.BookingDate = BookingDate;
            ViewBag.ArrivalDate = ArrivalDate;
            ViewBag.LOS = LOS;
            ViewBag.Rooms = Rooms;
            ViewBag.Occupancy = Occupancy;
            ViewBag.Type = Type;

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BookingDate"></param>
        /// <param name="ArrivalDate"></param>
        /// <param name="LOS"></param>
        /// <param name="Rooms"></param>
        /// <param name="Occupancy"></param>
        /// <param name="Type">0: Both , 1: Public, 2:Corporate</param>
        /// <returns></returns>
        public string BindGrid(string BookingDate, string ArrivalDate, string LOS, string Rooms, string Occupancy, string Type)
        {
            object result = null;
            string strResultJson = string.Empty;
            try
            {
                var type = Convert.ToInt32(Type);

                DataSet publicDataset = new DataSet();
                DataSet corporateDataset = new DataSet();

                string publicValues = string.Empty;
                string publicErrors = string.Empty;

                string corporateValues = string.Empty;
                string corporateErrors = string.Empty;

                string overallValues = string.Empty;
                string overallErrors = string.Empty;


                if (string.IsNullOrEmpty(BookingDate))
                {
                    throw new ArgumentNullException("BookingDate");
                }

                if (string.IsNullOrEmpty(ArrivalDate))
                {
                    throw new ArgumentNullException("ArrivalDate");
                }

                var propertyId = Convert.ToInt32(Session["PropId"]);

                //if (type == 0)
                //{
                //    Parallel.Invoke(() =>
                //    {
                //        publicDataset = BL_tblPropertyBasicBiddingMap.GetDataforBidSimulator(propertyId, BookingDate, ArrivalDate, LOS, Rooms, Occupancy, 1);
                //    },
                //     () =>
                //     {

                //         corporateDataset = BL_tblPropertyBasicBiddingMap.GetDataforBidSimulator(propertyId, BookingDate, ArrivalDate, LOS, Rooms, Occupancy, 2);
                //     });

                //    DataTable dtcorporateValue = corporateDataset.Tables[0];
                //    DataTable dtCorporateError = publicDataset.Tables[3];
                //    corporateValues = dtcorporateValue.Rows[0]["A"].ToString();
                //    corporateErrors = getErrorsJson(dtCorporateError);

                //}
                //else
                //{
                //    publicDataset = BL_tblPropertyBasicBiddingMap.GetDataforBidSimulator(propertyId, BookingDate, ArrivalDate, LOS, Rooms, Occupancy, type);
                //}

                string sType = type == 1 ? "P" : type == 2 ? "C" : "B";

                publicDataset = BL_tblPropertyBasicBiddingMap.ListSimulatorDataV2(propertyId, BookingDate, ArrivalDate, LOS, Rooms, Occupancy, sType);

                DataTable dtPublicValue = publicDataset.Tables[0];
                DataTable dtdays = publicDataset.Tables[1];
                DataTable dtPublicRooms = publicDataset.Tables[2];
                DataTable dtPublicError = publicDataset.Tables[3];

                string days = getweekdaysJson(dtdays);
                int roomcount = dtPublicRooms.Rows.Count + 12;
                string rooms = getroomsJson(dtPublicRooms);

                if (sType == "B")
                {
                    roomcount += 10;
                }

                publicValues = dtPublicValue.Rows[0]["A"].ToString();

                publicErrors = getErrorsJson(dtPublicError);

                result = new
                {
                    st = 1,
                    rooms = rooms,
                    days = days,
                    count = roomcount,
                    publicValues = publicValues,
                    publicErrors = publicErrors,
                    both = type == 0
                };
            }
            catch (Exception)
            {
            }

            strResultJson = OneFineRateAppUtil.clsUtils.ConvertToJson(result);

            return strResultJson;
        }
        public string GetAmenitiesList()
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                List<eAmenity> AM = new List<eAmenity>();
                AM = BL_Amenity.GetAllAmenities();

                strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(AM);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public string GetApplicabilitiesList()
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                List<eApplicability> AM = new List<eApplicability>();
                AM = BL_Amenity.GetAllApplicabilities();

                strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(AM);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public static string getweekdaysJson(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");
                string jsonval = "";
                jsonval = "\"name\" : \"" + dr[0] + "\" ,";
                jsonval += "\"cdate\" : \"" + dr[1] + "\" ,";
                jsonval += "\"myear\" : \"" + dr[2] + "\" ";
                str.Append(jsonval);
                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);
            return str.ToString();
        }

        public static string getroomsJson(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");
                string jsonval = "";
                jsonval = "\"name\" : \"" + dr[1].ToString() + "\" ,";
                jsonval += "\"id\" : \"_" + dr[0].ToString() + "\" ";
                str.Append(jsonval);
                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);

            if (str.ToString() == "[")
                str.Append("]");
            return str.ToString();
        }

        public static string getErrorsJson(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");
                string jsonval = "";
                jsonval = "\"Date\" : \"" + dr[0].ToString() + "\" ,";
                jsonval += "\"Type\" : \"" + dr[1].ToString() + "\" ,";
                jsonval += "\"Error\" : \"" + dr[2].ToString() + "\" ";
                str.Append(jsonval);
                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);

            if (str.ToString() == "[")
                str.Append("]");
            return str.ToString();
        }
    }
}