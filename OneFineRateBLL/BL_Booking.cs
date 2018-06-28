using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Data.SqlClient;
using OneFineRateAppUtil;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;
using OneFineRateBLL.WebUserEntities;
using System.Net.Mail;
using System.Data.Entity;

namespace OneFineRateBLL
{
    public class BL_Booking
    {

        public static int AddBookingForBid(etblBookingTx obj, etblBookingTrakerTx objTrck, List<etblBIDRoomAdultsTx> lst)
        {

            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    OneFineRate.tblBookingTx bdbooking = (OneFineRate.tblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(obj, new OneFineRate.tblBookingTx());
                    db.tblBookingTxes.Add(bdbooking);
                    db.SaveChanges();

                    obj.iBookingId = bdbooking.iBookingId;
                    objTrck.iBookingId = obj.iBookingId;

                    foreach (var RoomObj in lst)
                    {
                        RoomObj.iBookingId = obj.iBookingId;
                        OneFineRate.tblBIDRoomAdultsTx dbBookDetails = (OneFineRate.tblBIDRoomAdultsTx)OneFineRateAppUtil.clsUtils.ConvertToObject(RoomObj, new OneFineRate.tblBIDRoomAdultsTx());
                        db.tblBIDRoomAdultsTxes.Add(dbBookDetails);
                    }

                    OneFineRate.tblBookingTrakerTx tbtrck = (OneFineRate.tblBookingTrakerTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objTrck, new OneFineRate.tblBookingTrakerTx());
                    db.tblBookingTrakerTxes.Add(tbtrck);
                    db.SaveChanges();
                    retval = Convert.ToInt32(obj.iBookingId);


                }
                catch (Exception)
                {
                    throw;
                }

            }
            return retval;
        }

        public static int AddBooking(PropDetailsM obj, etblBookingNegotiationTx objNego, etblBookingTrakerTx objTrck, List<etblBookingDetailsTx> lstBookDetails, List<etblBookingGuestMap> lst, List<etblBookingCancellationPolicyMap> lstCancelPolicy, List<etblBookedDayWiseTaxAmountDetails> lstDayTaxes, etblOriginalBookingPrice objOrgBook, List<etblBookedDayWiseTaxAmountDetailsAll> lstDayTaxesAll = null)
        {

            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var oldBooking = obj.objBooking.iBookingId;
                    var propertyDetails = db.tblPropertyMs.Where(x => x.iPropId == obj.iPropId).SingleOrDefault();
                    var propertyStateId = 0;
                    if (propertyDetails != null)
                    {
                        propertyStateId = propertyDetails.iStateId;
                    }
                    OneFineRate.tblBookingTx bdbooking = (OneFineRate.tblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(obj.objBooking, new OneFineRate.tblBookingTx());
                    if (bdbooking.iBookingId > 0)
                    {
                        db.tblBookingTxes.Attach(bdbooking);
                        db.Entry(bdbooking).State = EntityState.Modified;
                    }
                    else
                    {
                        db.tblBookingTxes.Add(bdbooking);
                    }
                    db.SaveChanges();
                    obj.objBooking.iBookingId = bdbooking.iBookingId;
                    objTrck.iBookingId = obj.objBooking.iBookingId;

                    foreach (var objBookDetails in lstBookDetails)
                    {
                        objBookDetails.iBookingId = obj.objBooking.iBookingId;
                        OneFineRate.tblBookingDetailsTx dbBookDetails = (OneFineRate.tblBookingDetailsTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objBookDetails, new OneFineRate.tblBookingDetailsTx());
                        if (oldBooking <= 0)
                        {
                            db.tblBookingDetailsTxes.Add(dbBookDetails); db.SaveChanges();
                        }
                        objBookDetails.iBookingDetailsID = dbBookDetails.iBookingDetailsID;

                        //if (obj.objBooking.cBookingType == "N")
                        //{
                        if (oldBooking <= 0)
                        {
                            var dlist = lstDayTaxes.Distinct(new TaxesComparator()).ToList();

                            foreach (var objTaxes in dlist.Where(u => u.RoomID.ToString() == objBookDetails.iRoomId && u.RPID.ToString() == objBookDetails.iRPId && u.iOccupancy == objBookDetails.iOccupancy).AsQueryable())
                            {

                                objTaxes.iBookingDetailsID = Convert.ToInt32(dbBookDetails.iBookingDetailsID);
                                objTaxes.iBookingID = Convert.ToInt32(obj.objBooking.iBookingId);
                                OneFineRate.tblBookedDayWiseTaxAmountDetail dbDayTaxes = (OneFineRate.tblBookedDayWiseTaxAmountDetail)OneFineRateAppUtil.clsUtils.ConvertToObject(objTaxes, new OneFineRate.tblBookedDayWiseTaxAmountDetail());
                                db.tblBookedDayWiseTaxAmountDetails.Add(dbDayTaxes);

                            }
                            if (lstDayTaxesAll != null && lstDayTaxesAll.Count > 0)
                            {
                                var datewisetaxlist = lstDayTaxesAll.Distinct(new SeprateTaxesComparator()).ToList();

                                foreach (var objTaxes in datewisetaxlist.Where(u => u.RoomID.ToString() == objBookDetails.iRoomId && u.RPID.ToString() == objBookDetails.iRPId && u.iOccupancy == objBookDetails.iOccupancy).AsQueryable())
                                {
                                    if (propertyStateId == obj.iStateId && objTaxes.sTaxName != "IGST")
                                    {
                                        objTaxes.iBookingDetailsID = Convert.ToInt32(dbBookDetails.iBookingDetailsID);
                                        objTaxes.iBookingID = Convert.ToInt32(obj.objBooking.iBookingId);
                                        OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail dbDayTaxes = (OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail)OneFineRateAppUtil.clsUtils.ConvertToObject(objTaxes, new OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail());
                                        db.tblBookedDayWiseSeparateTaxAmountDetails.Add(dbDayTaxes);
                                    }
                                    else if (propertyStateId != obj.iStateId && objTaxes.sTaxName != "CGST" || propertyStateId != obj.iStateId && objTaxes.sTaxName != "SGST")
                                    {
                                        objTaxes.iBookingDetailsID = Convert.ToInt32(dbBookDetails.iBookingDetailsID);
                                        objTaxes.iBookingID = Convert.ToInt32(obj.objBooking.iBookingId);
                                        OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail dbDayTaxes = (OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail)OneFineRateAppUtil.clsUtils.ConvertToObject(objTaxes, new OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail());
                                        db.tblBookedDayWiseSeparateTaxAmountDetails.Add(dbDayTaxes);
                                    }
                                }
                            }
                            //}

                            //foreach (var objGuests in lst.Where(u => u.iRoomId.ToString() == objBookDetails.iRoomId).AsQueryable())
                            //{
                            //    objGuests.iBookingDetailsID = objBookDetails.iBookingDetailsID;
                            //    OneFineRate.tblBookingGuestMap dbGuestDetails = (OneFineRate.tblBookingGuestMap)OneFineRateAppUtil.clsUtils.ConvertToObject(objGuests, new OneFineRate.tblBookingGuestMap());
                            //    db.tblBookingGuestMaps.Add(dbGuestDetails);
                            //    // db.SaveChanges();
                            //}

                            var dlist_Cancel = lstCancelPolicy.Distinct(new CancellationComparator()).ToList();
                            foreach (var objCancel in dlist_Cancel.Where(u => u.iRPId.ToString() == objBookDetails.iRPId).AsQueryable())
                            {
                                objCancel.iBookingDetailsID = objBookDetails.iBookingDetailsID;
                                OneFineRate.tblBookingCancellationPolicyMap dbCancelPolicy = (OneFineRate.tblBookingCancellationPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(objCancel, new OneFineRate.tblBookingCancellationPolicyMap());
                                db.tblBookingCancellationPolicyMaps.Add(dbCancelPolicy);
                            }
                        }

                    }


                    if (obj.sActionType == "N")
                    {
                        objNego.iBookingId = obj.objBooking.iBookingId;

                        OneFineRate.tblBookingNegotiationTx tbNego = (OneFineRate.tblBookingNegotiationTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objNego, new OneFineRate.tblBookingNegotiationTx());
                        db.tblBookingNegotiationTxes.Add(tbNego);
                    }
                    OneFineRate.tblBookingTrakerTx tbtrck = (OneFineRate.tblBookingTrakerTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objTrck, new OneFineRate.tblBookingTrakerTx());
                    if (oldBooking<= 0)
                    {
                        db.tblBookingTrakerTxes.Add(tbtrck);
                        db.SaveChanges();
                    }
                    


                    if (objOrgBook != null&&oldBooking<=0)
                    {
                        objOrgBook.iBookingId = Convert.ToInt32(obj.objBooking.iBookingId);
                        OneFineRate.tblOriginalBookingPrice tbOrgBook = (OneFineRate.tblOriginalBookingPrice)OneFineRateAppUtil.clsUtils.ConvertToObject(objOrgBook, new OneFineRate.tblOriginalBookingPrice());
                        db.tblOriginalBookingPrices.Add(tbOrgBook);
                        db.SaveChanges();
                    }

                    retval = Convert.ToInt32(obj.objBooking.iBookingId);



                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return retval;
        }

        public static int? GetPropIdByBookingId(string sBookingId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var ibookingId = 0;
                int.TryParse(sBookingId, out ibookingId);

                return db.tblBookingTxes.Where(x => x.iBookingId == ibookingId).Select(x => x.iPropId).FirstOrDefault();
            }
        }

        public static long AddTGBooking(PropDetailsM obj, etblBookingTrakerTx objTrck, List<etblBookingDetailsTx> lstBookDetails, List<etblBookingCancellationPolicyMap> lstCancelPolicy, List<etblBookingGuestMap> lst)
        {
            long bookingId = 0;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    OneFineRate.tblBookingTx bdbooking = (OneFineRate.tblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(obj.objBooking, new OneFineRate.tblBookingTx());
                    db.tblBookingTxes.Add(bdbooking);
                    db.SaveChanges();

                    obj.objBooking.iBookingId = bdbooking.iBookingId;
                    objTrck.iBookingId = obj.objBooking.iBookingId;

                    foreach (var objBookDetails in lstBookDetails)
                    {
                        objBookDetails.iBookingId = obj.objBooking.iBookingId;
                        OneFineRate.tblBookingDetailsTx dbBookDetails = (OneFineRate.tblBookingDetailsTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objBookDetails, new OneFineRate.tblBookingDetailsTx());
                        db.tblBookingDetailsTxes.Add(dbBookDetails);
                        foreach (var objCancel in lstCancelPolicy.Where(u => u.iRPId.ToString() == objBookDetails.iRPId).AsQueryable())
                        {
                            objCancel.iBookingDetailsID = objBookDetails.iBookingDetailsID;
                            OneFineRate.tblBookingCancellationPolicyMap dbCancelPolicy = (OneFineRate.tblBookingCancellationPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(objCancel, new OneFineRate.tblBookingCancellationPolicyMap());
                            db.tblBookingCancellationPolicyMaps.Add(dbCancelPolicy);
                        }

                        db.SaveChanges();
                    }


                    OneFineRate.tblBookingTrakerTx tbtrck = (OneFineRate.tblBookingTrakerTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objTrck, new OneFineRate.tblBookingTrakerTx());
                    db.tblBookingTrakerTxes.Add(tbtrck);
                    db.SaveChanges();

                    bookingId = bdbooking.iBookingId;

                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {

                    }
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return bookingId;
        }


        /// <summary>
        /// Update the Gust information after confirm booking 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lst"></param>
        /// <param name="objTrck"></param>
        /// <param name="lsttblBookDetails"></param>
        /// <returns></returns>
        public static int UpdateBooking_AddGuestInformation(etblBookingTx obj, List<etblBookingGuestMap> lst, etblBookingTrakerTx objTrck, List<etblBookingDetailsTx> lsttblBookDetails)
        {

            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    OneFineRate.tblBookingTx bdbooking = (OneFineRate.tblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(obj, new OneFineRate.tblBookingTx());
                    bdbooking.sCountryPhoneCode = lst.Select(code => code.sCountryPhoneCode).FirstOrDefault();
                    db.tblBookingTxes.Attach(bdbooking);

                    db.Entry(bdbooking).State = System.Data.Entity.EntityState.Modified;

                    var dlist = lsttblBookDetails.Distinct(new BookingDetailsComparator()).ToList();
                    foreach (var objBookDetails in dlist)
                    {
                        var lt = db.tblBookingGuestMaps.Where(x => x.iBookingDetailsID == objBookDetails.iBookingDetailsID).ToList();

                        db.tblBookingGuestMaps.RemoveRange(lt);
                        db.SaveChanges();
                        int Count = 0;
                        foreach (var objGuests in lst.Where(u => u.iBookingDetailsID == objBookDetails.iBookingDetailsID).AsQueryable())
                        {
                            Count++;
                            if (Count == 1)
                                objGuests.bIsPrimary = true;
                            OneFineRate.tblBookingGuestMap dbGuestDetails = (OneFineRate.tblBookingGuestMap)OneFineRateAppUtil.clsUtils.ConvertToObject(objGuests, new OneFineRate.tblBookingGuestMap());
                            db.tblBookingGuestMaps.Add(dbGuestDetails);
                            // db.SaveChanges();
                        }
                    }

                    OneFineRate.tblBookingTrakerTx tbtrck = (OneFineRate.tblBookingTrakerTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objTrck, new OneFineRate.tblBookingTrakerTx());
                    db.tblBookingTrakerTxes.Add(tbtrck);
                    db.SaveChanges();
                    retval = 1;


                }
                catch (Exception)
                {
                    retval = 0;
                    throw;
                }

            }
            return retval;
        }

        // Booking List
        public static List<eBooking> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, int PropId, string datetype, string fromdate, string todate, string type, string bookid, string firstname, string lastname, string email, string status, int displayLength = 10)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eBooking> user = new List<eBooking>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                List<eBooking> bookLst = new List<eBooking>();
                List<eBooking> objResult = new List<eBooking>();


                int pagesize = param.iDisplayStart == 0 ? 1 : (param.iDisplayStart / 10) + 1;

                SqlParameter[] MyParam = new SqlParameter[14];
                MyParam[0] = new SqlParameter("@propid", PropId);
                MyParam[1] = new SqlParameter("@datetype", datetype);
                MyParam[2] = new SqlParameter("@checkin", fromdate);
                MyParam[3] = new SqlParameter("@checkout", todate);
                MyParam[4] = new SqlParameter("@bookingtype", type);
                MyParam[5] = new SqlParameter("@confirmationno", bookid);
                MyParam[6] = new SqlParameter("@firstname", firstname);
                MyParam[7] = new SqlParameter("@lastname", lastname);
                MyParam[8] = new SqlParameter("@email", email);
                MyParam[9] = new SqlParameter("@status", status);
                MyParam[10] = new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D");
                MyParam[11] = new SqlParameter("@SortBy", param.iSortingCols == 2 ? "cBookingType" : "BookingId");
                MyParam[12] = new SqlParameter("@startRowIndex", pagesize);
                MyParam[13] = new SqlParameter("@maximumRows", displayLength);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspBookingList", MyParam);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    eBooking objres = new eBooking();
                    objres.Sno = Convert.ToInt64(dr["Sno"]);
                    objres.ConfirmationNo = Convert.ToInt64(dr["ConfirmationNo"]);
                    objres.BookingDate = Convert.ToDateTime(dr["BookingDate"]);
                    objres.GuestComment = Convert.ToString(dr["GuestComment"]);
                    objres.GuestName = Convert.ToString(dr["GuestName"]);
                    objres.Booker = Convert.ToString(dr["Booker"]);
                    objres.CheckIn = Convert.ToDateTime(dr["CheckIn"]);
                    objres.CheckOut = Convert.ToDateTime(dr["CheckOut"]);
                    objres.Status = Convert.ToString(dr["Status"]);
                    objres.Total = Convert.ToString(dr["Total"]);
                    objres.Commission = Convert.ToString(dr["Commission"]);
                    objres.EmailId = Convert.ToString(dr["EmailId"]);
                    objres.Type = Convert.ToString(dr["Type"]);
                    objres.ShowEdit = Convert.ToString(dr["ShowEdit"]);
                    objResult.Add(objres);
                }
                //objResult = db.Database.SqlQuery<eBooking>("uspBookingList @propid", MyParam).ToList();

                var result = objResult.AsQueryable();//Where(a => a.Booker.Contains(param.sSearch.ToLower()) || Convert.ToString(a.GuestName).Contains(param.sSearch.ToLower())).AsQueryable();
                //get Total Count for show total records 
                TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);//result.Count();

                //for sorting
                //switch (param.iSortingCols)
                //{
                //    case 2:
                //        result = param.sortDirection == "asc" ? result.OrderBy(u => u.BookingDate) : result.OrderByDescending(u => u.BookingDate);
                //        break;

                //}

                ////implemented paging
                //var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in result)
                {
                    item.sBookingId = clsUtils.Encode(item.ConfirmationNo.ToString());
                    bookLst.Add((eBooking)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eBooking()));
                }

                return bookLst;
            }
        }

        // Get Modify Booking Details
        public static eBookingModify GetBookingModifyDetails(long BookingId)
        {
            eBookingModify objBookingModify = new eBookingModify();
            List<eBookingRoomM> objBookingRoomArr = new List<eBookingRoomM>();
            List<eBookingRatePlan> objBookingRatePlanArr = new List<eBookingRatePlan>();
            try
            {
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@bookingid", BookingId);
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetBookingDetailsByBookingId", MyParam);

                if (ds.Tables[1].Rows.Count > 0)
                {
                    objBookingModify.PropId = Convert.ToInt32(ds.Tables[1].Rows[0]["iPropId"]);
                    objBookingModify.BookingId = Convert.ToString(ds.Tables[1].Rows[0]["iBookingId"]);
                    objBookingModify.ReservationDate = Convert.ToString(ds.Tables[1].Rows[0]["dtReservationDate"]);
                    objBookingModify.BookingStatus = Convert.ToString(ds.Tables[1].Rows[0]["Status"]);
                    objBookingModify.ccType = Convert.ToString(ds.Tables[1].Rows[0]["ccType"]);
                    objBookingModify.Reservation = Convert.ToString(ds.Tables[1].Rows[0]["Reservation"]);
                    objBookingModify.CheckIn = Convert.ToString(ds.Tables[1].Rows[0]["dtCheckIn"]);
                    objBookingModify.ChekOut = Convert.ToString(ds.Tables[1].Rows[0]["dtChekOut"]);
                    objBookingModify.Booker = Convert.ToString(ds.Tables[1].Rows[0]["Booker"]);
                    objBookingModify.EmailOFR = Convert.ToString(ds.Tables[1].Rows[0]["sEmailOFR"]);
                    objBookingModify.MobileOFR = Convert.ToString(ds.Tables[1].Rows[0]["sMobileOFR"]);
                    objBookingModify.CountryPhoneCode = Convert.ToString(ds.Tables[1].Rows[0]["sCountryPhoneCode"]);
                    objBookingModify.HotelName = Convert.ToString(ds.Tables[1].Rows[0]["sHotelName"]);
                    objBookingModify.RatingImageUrl = Convert.ToString(ds.Tables[1].Rows[0]["sRatingImageURl"]);
                    objBookingModify.RatingString = Convert.ToString(ds.Tables[1].Rows[0]["sRankingString"]);
                    objBookingModify.StreetAddress = Convert.ToString(ds.Tables[1].Rows[0]["StreetAddress"]);
                    objBookingModify.Address = Convert.ToString(ds.Tables[1].Rows[0]["Address"]);
                    objBookingModify.StarCategoryId = Convert.ToString(ds.Tables[1].Rows[0]["iStarCategoryId"]);
                    objBookingModify.AvgAmount = Convert.ToString(ds.Tables[1].Rows[0]["AvgAmount"]);
                    objBookingModify.TotalAmount = Convert.ToString(ds.Tables[1].Rows[0]["TotalAmount"]);

                    objBookingModify.Tax = Convert.ToString(ds.Tables[1].Rows[0]["Tax"]);
                    objBookingModify.dTaxesForHotel = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[1].Rows[0]["Tax"])) ? Convert.ToDecimal(ds.Tables[1].Rows[0]["Tax"]) : 0;
                    objBookingModify.TaxPerNight = Convert.ToString(ds.Tables[1].Rows[0]["TaxPerNight"]);
                    objBookingModify.dOfrServiceCharge = Convert.ToString(ds.Tables[1].Rows[0]["OFRServiceCharge"]);
                    objBookingModify.dOfrGSTONServiceCharge = Convert.ToString(ds.Tables[1].Rows[0]["GSTOFRServiceCharge"]);
                    objBookingModify.dValueType = Convert.ToString(ds.Tables[1].Rows[0]["GSTValueType"]);
                    objBookingModify.dValue = Convert.ToString(ds.Tables[1].Rows[0]["GSTValue"]);
                    objBookingModify.GST_Same_State =!string.IsNullOrEmpty(Convert.ToString(ds.Tables[1].Rows[0]["GST_Same_State"]))? Convert.ToBoolean(ds.Tables[1].Rows[0]["GST_Same_State"]):false;
                    objBookingModify.dTotalGuestBidRate_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dTotalGuestBidRate_Bid"]);
                    objBookingModify.SGST_Per = Convert.ToString(ds.Tables[1].Rows[0]["SGST_Per"]);
                    objBookingModify.SGST_Val = Convert.ToString(ds.Tables[1].Rows[0]["SGST_Val"]);

                    objBookingModify.dGstOnCommission = Convert.ToString(ds.Tables[1].Rows[0]["dGstOnCommission"]);
                    objBookingModify.dGstOnCommissionPercent = Convert.ToString(ds.Tables[1].Rows[0]["dGstOnCommissionPercent"]);
                    objBookingModify.dTotalCommission = Convert.ToString(ds.Tables[1].Rows[0]["dTotalCommission"]);
                    objBookingModify.dCommissionPer = Convert.ToString(ds.Tables[1].Rows[0]["dCommissionPer"]);

                    objBookingModify.ExtraBedAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExtraBedAmount"]);
                    objBookingModify.ExtraBedAmountAsPolicy = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExtraBedAmountAsPolicy"]);
                    objBookingModify.Comm = Convert.ToString(ds.Tables[1].Rows[0]["Comm"]);
                    objBookingModify.SubTotal = Convert.ToString(ds.Tables[1].Rows[0]["SubTotal"]);
                    objBookingModify.dccPrice = Convert.ToDecimal(ds.Tables[1].Rows[0]["dccPrice"]);
                    objBookingModify.dccDiscount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dccDiscount"]);
                    objBookingModify.dBidAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dBidAmount"]);
                    objBookingModify.sExtra2 = Convert.ToDecimal(ds.Tables[1].Rows[0]["sExtra2"]);
                    objBookingModify.sExtra3 = ds.Tables[1].Rows[0]["sExtra3"] != DBNull.Value ? Convert.ToString(ds.Tables[1].Rows[0]["sExtra3"]) : "";
                    
                    objBookingModify.SpecialRequest = Convert.ToString(ds.Tables[1].Rows[0]["sSpecialRequests"]);
                    objBookingModify.ExpectedCheckIn = Convert.ToString(ds.Tables[1].Rows[0]["sExpectedCheckIn"]);
                    objBookingModify.SpecialOccasion = Convert.ToString(ds.Tables[1].Rows[0]["SpecialOccasion"]);

                    objBookingModify.Pickup = Convert.ToString(ds.Tables[1].Rows[0]["spref_pickup"]);
                    objBookingModify.FlightNumber = Convert.ToString(ds.Tables[1].Rows[0]["sFlightNumber"]);
                    objBookingModify.EstimateArrivalTime = Convert.ToString(ds.Tables[1].Rows[0]["sEstArrivalTime"]);
                    objBookingModify.Landing = Convert.ToString(ds.Tables[1].Rows[0]["sLandingAt"]);
                    objBookingModify.TypeOfCar = Convert.ToString(ds.Tables[1].Rows[0]["sTypeOfCar"]);
                    objBookingModify.NoOfCar = Convert.ToString(ds.Tables[1].Rows[0]["iNoOFCars"]);

                    objBookingModify.SingleLady = Convert.ToString(ds.Tables[1].Rows[0]["SingleLady"]);
                    objBookingModify.SmokingRoom = Convert.ToString(ds.Tables[1].Rows[0]["SmokingRoom"]);
                    objBookingModify.Elevator = Convert.ToString(ds.Tables[1].Rows[0]["Elevator"]);
                    objBookingModify.QuietRoom = Convert.ToString(ds.Tables[1].Rows[0]["QuietRoom"]);

                    objBookingModify.sCheckInHH = Convert.ToString(ds.Tables[1].Rows[0]["sCheckInHH"]);
                    objBookingModify.sCheckInMM = Convert.ToString(ds.Tables[1].Rows[0]["sCheckInMM"]);
                    objBookingModify.sCheckoutHH = Convert.ToString(ds.Tables[1].Rows[0]["sCheckoutHH"]);
                    objBookingModify.sCheckoutMM = Convert.ToString(ds.Tables[1].Rows[0]["sCheckoutMM"]);
                    objBookingModify.iMinCheckInAge = Convert.ToByte(ds.Tables[1].Rows[0]["iMinCheckInAge"]);
                    objBookingModify.dExtraBedCharges = Convert.ToDecimal(ds.Tables[1].Rows[0]["dExtraBedCharges"]);
                    objBookingModify.ExtraBedChargesAsPolicy = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExtraBedChargesAsPolicy"]);
                    objBookingModify.iExtraBedRequiredFrom = Convert.ToInt32(ds.Tables[1].Rows[0]["iExtraBedRequiredFrom"]);
                    objBookingModify.iComplimentaryStayAge = Convert.ToInt32(ds.Tables[1].Rows[0]["iComplimentaryStayAge"]);
                    objBookingModify.bAlcoholAllowedOnsite = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAlcoholAllowedOnsite"]);
                    objBookingModify.bAlcoholServedOnsite = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAlcoholServedOnsite"]);
                    objBookingModify.bVisitorsAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bVisitorsAllowed"]);
                    objBookingModify.bPetsAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bPetsAllowed"]);
                    objBookingModify.bPartiesAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bPartiesAllowed"]);
                    objBookingModify.sImgUrl = Convert.ToString(ds.Tables[1].Rows[0]["sImgUrl"]);
                    objBookingModify.Symbol = Convert.ToString(ds.Tables[1].Rows[0]["Symbol"]);
                    objBookingModify.sCurrencyCode = Convert.ToString(ds.Tables[1].Rows[0]["sCurrencyCode"]);
                    objBookingModify.sCity = Convert.ToString(ds.Tables[1].Rows[0]["sCity"]);
                    objBookingModify.dLatitude = Convert.ToDecimal(ds.Tables[1].Rows[0]["dLatitude"]);
                    objBookingModify.dLongitude = Convert.ToDecimal(ds.Tables[1].Rows[0]["dLongitude"]);
                    objBookingModify.dTotalNegotiateAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dTotalNegotiateAmount"]);
                    objBookingModify.dAdvNegotiateAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dAdvNegotiateAmount"]);
                    objBookingModify.dBalanceAmt = Convert.ToDecimal(ds.Tables[1].Rows[0]["dBalanceAmt"]);
                    objBookingModify.cBookingType = Convert.ToString(ds.Tables[1].Rows[0]["cBookingType"]);
                    objBookingModify.cBookingStatus = Convert.ToString(ds.Tables[1].Rows[0]["cBookingStatus"]);
                    objBookingModify.dSubTotalNego = Convert.ToDecimal(ds.Tables[1].Rows[0]["dSubTotalNego"]);
                    objBookingModify.dOrginalSubTotalNego = Convert.ToDecimal(ds.Tables[1].Rows[0]["dOrginalSubTotalNego"]);
                    objBookingModify.dCounterOffer = Convert.ToDecimal(ds.Tables[1].Rows[0]["dCounterOffer"]);
                    objBookingModify.dSubTotalCounter = Convert.ToDecimal(ds.Tables[1].Rows[0]["dSubTotalCounter"]);
                    objBookingModify.dCustomerPayable = ds.Tables[1].Rows[0]["dCustomerPayable"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dCustomerPayable"]) : 0;
                    objBookingModify.ActualBookingType = Convert.ToString(ds.Tables[1].Rows[0]["ActualBookingType"]);
                    objBookingModify.b24HrsCheckIn = Convert.ToBoolean(ds.Tables[1].Rows[0]["b24HrsCheckIn"]);
                    objBookingModify.bEarlyCheckInChargeable = Convert.ToBoolean(ds.Tables[1].Rows[0]["bEarlyCheckInChargeable"]);
                    objBookingModify.b24HrsCheckout = Convert.ToBoolean(ds.Tables[1].Rows[0]["b24HrsCheckout"]);
                    objBookingModify.bEarlyCheckoutChargeable = Convert.ToBoolean(ds.Tables[1].Rows[0]["bEarlyCheckoutChargeable"]);
                    objBookingModify.dDiscountedBidPrice = ds.Tables[1].Rows[0]["dDiscountedBidPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dDiscountedBidPrice"]) : 0;
                    objBookingModify.iLinkedBookingId = Convert.ToInt32(ds.Tables[1].Rows[0]["iLinkedBookingId"]);
                    //objBookingModify.bPromoDiscount = ds.Tables[1].Rows[0]["bPromoDiscount"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[0]["bPromoDiscount"]) : false;
                    //objBookingModify.bPromoAmenity = Convert.ToString(ds.Tables[1].Rows[0]["bPromoAmenity"]);
                    objBookingModify.sExtra1 = ds.Tables[1].Rows[0]["sExtra1"] != DBNull.Value ? Convert.ToString(ds.Tables[1].Rows[0]["sExtra1"]) : "";

                    objBookingModify.dHotelGrossCharges = ds.Tables[1].Rows[0]["HotelGrossCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["HotelGrossCharges"].ToString()) : 0;
                    objBookingModify.dTotalPayableToHotel = Convert.ToString(ds.Tables[1].Rows[0]["dTotalPayableToHotel"]);
                    objBookingModify.dHotelRatePerNight = Convert.ToString(ds.Tables[1].Rows[0]["dHotelRatePerNight"]);


                    objBookingModify.dCustomerGrossCharges = ds.Tables[1].Rows[0]["dCustomerGrossCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dCustomerGrossCharges"].ToString()) : 0;
                    objBookingModify.dCustomerTotalCharges = ds.Tables[1].Rows[0]["dCustomerTotalCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dCustomerTotalCharges"].ToString()) : 0;

                    objBookingModify.sProvisionalBookingIdTG = Convert.ToString(ds.Tables[1].Rows[0]["sProvisionalBookingIdTG"]);
                    objBookingModify.sProvisionalTrxnTypeTG = Convert.ToString(ds.Tables[1].Rows[0]["sProvisionalTrxnTypeTG"]);
                    objBookingModify.sFinalBookingIdTG = Convert.ToString(ds.Tables[1].Rows[0]["sFinalBookingIdTG"]);
                    objBookingModify.sFinalTrxnTypeTG = Convert.ToString(ds.Tables[1].Rows[0]["sFinalTrxnTypeTG"]);
                    objBookingModify.sVendorId = Convert.ToString(ds.Tables[1].Rows[0]["iVendorId"]);


                    objBookingModify.dHotelPublicRatePerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dHotelPublicRatePerNight_Bid"]);
                    objBookingModify.dDiscount_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dDiscount_Bid"]);
                    objBookingModify.dDiscountedBidPrice = Convert.ToDecimal(ds.Tables[1].Rows[0]["dDiscountedBidPrice"]);
                    objBookingModify.dDiscountRatePerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dDiscountRatePerNight_Bid"]);
                    objBookingModify.dTotalDiscountedRate_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dTotalDiscountedRate_Bid"]);
                    objBookingModify.dGuestBidAmountPerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dGuestBidAmountPerNight_Bid"]);

                    objBookingModify.EntityName = ds.Tables[1].Rows[0]["sGstnEntityName"].ToString();
                    objBookingModify.TypeOfEntity = ds.Tables[1].Rows[0]["sGstnEntityType"].ToString();
                    objBookingModify.GstnNumber = ds.Tables[1].Rows[0]["sGstnNumber"].ToString();
                    objBookingModify.EntityAddress = ds.Tables[1].Rows[0]["sGstnEntityAddress"].ToString();
                    objBookingModify.State = ds.Tables[1].Rows[0]["State_Customer"].ToString();
                    objBookingModify.StateCode = ds.Tables[1].Rows[0]["sStateCode_Customer"].ToString();

                    switch (objBookingModify.TypeOfEntity.ToLower())
                    {
                        case "p":
                            objBookingModify.TypeOfEntity = GSTEntityType.p;
                            break;
                        case "c":
                            objBookingModify.TypeOfEntity = GSTEntityType.c;
                            break;
                        case "h":
                            objBookingModify.TypeOfEntity = GSTEntityType.h;
                            break;
                        case "f":
                            objBookingModify.TypeOfEntity = GSTEntityType.f;
                            break;
                        case "a":
                            objBookingModify.TypeOfEntity = GSTEntityType.a;
                            break;
                        case "t":
                            objBookingModify.TypeOfEntity = GSTEntityType.t;
                            break;
                        case "b":
                            objBookingModify.TypeOfEntity = GSTEntityType.b;
                            break;
                        case "l":
                            objBookingModify.TypeOfEntity = GSTEntityType.l;
                            break;
                        case "j":
                            objBookingModify.TypeOfEntity = GSTEntityType.j;
                            break;
                        case "g":
                            objBookingModify.TypeOfEntity = GSTEntityType.g;
                            break;

                        default:
                            break;
                    }



                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        objBookingModify.sMessage = Convert.ToString(ds.Tables[2].Rows[0]["Msg"]);
                    }

                    foreach (DataRow drR in ds.Tables[0].Rows)
                    {
                        eBookingRoomM objBookingRoom = new eBookingRoomM();
                        objBookingRoom.iBookingDetailsId = Convert.ToString(drR["iBookingDetailsID"]);
                        objBookingRoom.RoomId = Convert.ToString(drR["iRoomId"]);
                        objBookingRoom.RPId = Convert.ToString(drR["iRPId"]);
                        objBookingRoom.RoomName = Convert.ToString(drR["sRoomName"]);
                        objBookingRoom.RatePlan = Convert.ToString(drR["sRPName"]);
                        objBookingRoom.Adult = Convert.ToString(drR["iAdults"]);
                        objBookingRoom.Children = Convert.ToString(drR["iChildren"]);
                        objBookingRoom.sChildrenAge = Convert.ToString(drR["sChildrenAge"]);
                        objBookingRoom.Occupancy = Convert.ToString(drR["Occupancy"]);
                        objBookingRoom.BedPrefer = Convert.ToString(drR["BedPref"]);
                        objBookingRoom.ExtraBed = Convert.ToString(drR["ExtraBeds"]);
                        objBookingRoom.GuestName = Convert.ToString(drR["GuestName"]);
                        objBookingRoom.PolicyName = Convert.ToString(drR["sPolicyName"]);
                        objBookingRoom.AmenityRatePlan = Convert.ToString(drR["sAmenityRatePlan"]);
                        objBookingRoom.RoomNo = Convert.ToString(drR["Room"]);
                        objBookingRoom.RoomRate = drR["dRoomRate"] != DBNull.Value ? Convert.ToDecimal(drR["dRoomRate"]) : 0;
                        objBookingRoom.dTaxes = drR["dTaxes"] != DBNull.Value ? Convert.ToDecimal(drR["dTaxes"]) : 0;
                        objBookingRoom.dExtraBedRate = drR["dExtraBedRate"] != DBNull.Value ? Convert.ToDecimal(drR["dExtraBedRate"]) : 0;


                        objBookingRoomArr.Add(objBookingRoom);
                    }

                    //Fetch credit card images
                    string[] arrCreditCards = new string[1000];
                    string CreditCards = Convert.ToString(ds.Tables[1].Rows[0]["sCreditCardId"]);
                    if (CreditCards != "")
                    {
                        arrCreditCards = CreditCards.Split(',');
                        List<CreditCards> lstcreditcards = new List<CreditCards>();
                        foreach (var item in arrCreditCards)
                        {
                            lstcreditcards.Add(new CreditCards()
                            {
                                sImg = item
                            });
                        }
                        objBookingModify.lstCreditCards = lstcreditcards;
                    }

                    objBookingModify.slargeMapURL = "https://www.google.com/maps/place/" + objBookingModify.HotelName + "+-+" + objBookingModify.sCity.Trim() + "/@" + objBookingModify.dLatitude + "," + objBookingModify.dLongitude + ",17z?hl=en-US";
                }

                objBookingModify.BookingRoomDetails = objBookingRoomArr;


                //objBookingModify.BookingRatePlans = objBookingRatePlanArr;
            }
            catch (Exception ex)
            {
            }
            return objBookingModify;
        }

        // Get Modify Booking Details for Notifications
        public static eBookingModify GetBookingModifyDetails_Notifications(long BookingId, bool CurrencyType = false)
        {
            eBookingModify objBookingModify = new eBookingModify();
            List<eBookingRoomM> objBookingRoomArr = new List<eBookingRoomM>();
            List<eBookingRatePlan> objBookingRatePlanArr = new List<eBookingRatePlan>();
            try
            {
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@bookingid", BookingId);
                MyParam[1] = new SqlParameter("@CurrencyType", CurrencyType);
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetBookingDetailsByBookingId_ForNotifications", MyParam);

                if (ds.Tables[1].Rows.Count > 0)
                {
                    objBookingModify.PropId = Convert.ToInt32(ds.Tables[1].Rows[0]["iPropId"]);
                    objBookingModify.BookingId = Convert.ToString(ds.Tables[1].Rows[0]["iBookingId"]);
                    objBookingModify.ReservationDate = Convert.ToString(ds.Tables[1].Rows[0]["dtReservationDate"]);
                    objBookingModify.BookingStatus = Convert.ToString(ds.Tables[1].Rows[0]["Status"]);
                    objBookingModify.ccType = Convert.ToString(ds.Tables[1].Rows[0]["ccType"]);
                    objBookingModify.Reservation = Convert.ToString(ds.Tables[1].Rows[0]["Reservation"]);
                    objBookingModify.CheckIn = Convert.ToString(ds.Tables[1].Rows[0]["dtCheckIn"]);
                    objBookingModify.ChekOut = Convert.ToString(ds.Tables[1].Rows[0]["dtChekOut"]);
                    objBookingModify.Booker = Convert.ToString(ds.Tables[1].Rows[0]["Booker"]);
                    objBookingModify.EmailOFR = Convert.ToString(ds.Tables[1].Rows[0]["sEmailOFR"]);
                    objBookingModify.MobileOFR = Convert.ToString(ds.Tables[1].Rows[0]["sMobileOFR"]);
                    objBookingModify.CountryPhoneCode = Convert.ToString(ds.Tables[1].Rows[0]["sCountryPhoneCode"]);
                    objBookingModify.HotelName = Convert.ToString(ds.Tables[1].Rows[0]["sHotelName"]);
                    objBookingModify.RatingImageUrl = Convert.ToString(ds.Tables[1].Rows[0]["sRatingImageURl"]);
                    objBookingModify.RatingString = Convert.ToString(ds.Tables[1].Rows[0]["sRankingString"]);
                    objBookingModify.StreetAddress = Convert.ToString(ds.Tables[1].Rows[0]["StreetAddress"]);
                    objBookingModify.Address = Convert.ToString(ds.Tables[1].Rows[0]["Address"]);
                    objBookingModify.StarCategoryId = Convert.ToString(ds.Tables[1].Rows[0]["iStarCategoryId"]);
                    objBookingModify.AvgAmount = Convert.ToString(ds.Tables[1].Rows[0]["AvgAmount"]);
                    objBookingModify.TotalAmount = Convert.ToString(ds.Tables[1].Rows[0]["TotalAmount"]);
                    objBookingModify.Tax = Convert.ToString(ds.Tables[1].Rows[0]["Tax"]);
                    objBookingModify.ExtraBedAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExtraBedAmount"]);
                    objBookingModify.ExtraBedAmountAsPolicy = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExtraBedAmountAsPolicy"]);
                    objBookingModify.Comm = Convert.ToString(ds.Tables[1].Rows[0]["Comm"]);
                    objBookingModify.SubTotal = Convert.ToString(ds.Tables[1].Rows[0]["SubTotal"]);
                    objBookingModify.dccPrice = Convert.ToDecimal(ds.Tables[1].Rows[0]["dccPrice"]);
                    objBookingModify.dccDiscount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dccDiscount"]);
                    objBookingModify.dBidAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dBidAmount"]);
                    objBookingModify.sExtra2 = Convert.ToDecimal(ds.Tables[1].Rows[0]["sExtra2"]);
                    objBookingModify.dTotalGuestBidRate_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dTotalGuestBidRate_Bid"]);
                    objBookingModify.SpecialRequest = Convert.ToString(ds.Tables[1].Rows[0]["sSpecialRequests"]);
                    objBookingModify.ExpectedCheckIn = Convert.ToString(ds.Tables[1].Rows[0]["sExpectedCheckIn"]);
                    objBookingModify.SpecialOccasion = Convert.ToString(ds.Tables[1].Rows[0]["SpecialOccasion"]);

                    objBookingModify.Pickup = Convert.ToString(ds.Tables[1].Rows[0]["spref_pickup"]);
                    objBookingModify.FlightNumber = Convert.ToString(ds.Tables[1].Rows[0]["sFlightNumber"]);
                    objBookingModify.EstimateArrivalTime = Convert.ToString(ds.Tables[1].Rows[0]["sEstArrivalTime"]);
                    objBookingModify.Landing = Convert.ToString(ds.Tables[1].Rows[0]["sLandingAt"]);
                    objBookingModify.TypeOfCar = Convert.ToString(ds.Tables[1].Rows[0]["sTypeOfCar"]);
                    objBookingModify.NoOfCar = Convert.ToString(ds.Tables[1].Rows[0]["iNoOFCars"]);

                    objBookingModify.SingleLady = Convert.ToString(ds.Tables[1].Rows[0]["SingleLady"]);
                    objBookingModify.SmokingRoom = Convert.ToString(ds.Tables[1].Rows[0]["SmokingRoom"]);
                    objBookingModify.Elevator = Convert.ToString(ds.Tables[1].Rows[0]["Elevator"]);
                    objBookingModify.QuietRoom = Convert.ToString(ds.Tables[1].Rows[0]["QuietRoom"]);

                    objBookingModify.sCheckInHH = Convert.ToString(ds.Tables[1].Rows[0]["sCheckInHH"]);
                    objBookingModify.sCheckInMM = Convert.ToString(ds.Tables[1].Rows[0]["sCheckInMM"]);
                    objBookingModify.sCheckoutHH = Convert.ToString(ds.Tables[1].Rows[0]["sCheckoutHH"]);
                    objBookingModify.sCheckoutMM = Convert.ToString(ds.Tables[1].Rows[0]["sCheckoutMM"]);
                    objBookingModify.iMinCheckInAge = Convert.ToByte(ds.Tables[1].Rows[0]["iMinCheckInAge"]);
                    objBookingModify.dExtraBedCharges = Convert.ToDecimal(ds.Tables[1].Rows[0]["dExtraBedCharges"]);
                    objBookingModify.ExtraBedChargesAsPolicy = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExtraBedChargesAsPolicy"]);
                    objBookingModify.iExtraBedRequiredFrom = Convert.ToInt32(ds.Tables[1].Rows[0]["iExtraBedRequiredFrom"]);
                    objBookingModify.iComplimentaryStayAge = Convert.ToInt32(ds.Tables[1].Rows[0]["iComplimentaryStayAge"]);
                    objBookingModify.bAlcoholAllowedOnsite = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAlcoholAllowedOnsite"]);
                    objBookingModify.bAlcoholServedOnsite = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAlcoholServedOnsite"]);
                    objBookingModify.bVisitorsAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bVisitorsAllowed"]);
                    objBookingModify.bPetsAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bPetsAllowed"]);
                    objBookingModify.bPartiesAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bPartiesAllowed"]);
                    objBookingModify.sImgUrl = Convert.ToString(ds.Tables[1].Rows[0]["sImgUrl"]);
                    objBookingModify.Symbol = Convert.ToString(ds.Tables[1].Rows[0]["Symbol"]);
                    objBookingModify.sCurrencyCode = Convert.ToString(ds.Tables[1].Rows[0]["sCurrencyCode"]);
                    objBookingModify.sCity = Convert.ToString(ds.Tables[1].Rows[0]["sCity"]);
                    objBookingModify.dLatitude = Convert.ToDecimal(ds.Tables[1].Rows[0]["dLatitude"]);
                    objBookingModify.dLongitude = Convert.ToDecimal(ds.Tables[1].Rows[0]["dLongitude"]);
                    objBookingModify.dTotalNegotiateAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dTotalNegotiateAmount"]);
                    objBookingModify.dAdvNegotiateAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dAdvNegotiateAmount"]);
                    objBookingModify.dBalanceAmt = Convert.ToDecimal(ds.Tables[1].Rows[0]["dBalanceAmt"]);
                    objBookingModify.cBookingType = Convert.ToString(ds.Tables[1].Rows[0]["cBookingType"]);
                    objBookingModify.cBookingStatus = Convert.ToString(ds.Tables[1].Rows[0]["cBookingStatus"]);
                    objBookingModify.dSubTotalNego = Convert.ToDecimal(ds.Tables[1].Rows[0]["dSubTotalNego"]);
                    objBookingModify.dOrginalSubTotalNego = Convert.ToDecimal(ds.Tables[1].Rows[0]["dOrginalSubTotalNego"]);
                    objBookingModify.dCounterOffer = Convert.ToDecimal(ds.Tables[1].Rows[0]["dCounterOffer"]);
                    objBookingModify.dSubTotalCounter = Convert.ToDecimal(ds.Tables[1].Rows[0]["dSubTotalCounter"]);
                    objBookingModify.dCustomerPayable = ds.Tables[1].Rows[0]["dCustomerPayable"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dCustomerPayable"]) : 0;
                    objBookingModify.ActualBookingType = Convert.ToString(ds.Tables[1].Rows[0]["ActualBookingType"]);
                    objBookingModify.b24HrsCheckIn = Convert.ToBoolean(ds.Tables[1].Rows[0]["b24HrsCheckIn"]);
                    objBookingModify.bEarlyCheckInChargeable = Convert.ToBoolean(ds.Tables[1].Rows[0]["bEarlyCheckInChargeable"]);
                    objBookingModify.b24HrsCheckout = Convert.ToBoolean(ds.Tables[1].Rows[0]["b24HrsCheckout"]);
                    objBookingModify.bEarlyCheckoutChargeable = Convert.ToBoolean(ds.Tables[1].Rows[0]["bEarlyCheckoutChargeable"]);
                    objBookingModify.dDiscountedBidPrice = ds.Tables[1].Rows[0]["dDiscountedBidPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dDiscountedBidPrice"]) : 0;
                    objBookingModify.iLinkedBookingId = Convert.ToInt32(ds.Tables[1].Rows[0]["iLinkedBookingId"]);
                    //objBookingModify.bPromoDiscount = ds.Tables[1].Rows[0]["bPromoDiscount"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[0]["bPromoDiscount"]) : false;
                    //objBookingModify.bPromoAmenity = Convert.ToString(ds.Tables[1].Rows[0]["bPromoAmenity"]);
                    objBookingModify.sExtra1 = ds.Tables[1].Rows[0]["sExtra1"] != DBNull.Value ? Convert.ToString(ds.Tables[1].Rows[0]["sExtra1"]) : "";
                    objBookingModify.sExtra4 = Convert.ToString(ds.Tables[1].Rows[0]["sExtra4"]);

                    // Used for RewardPoint Indentification in Db as well as on Various Page
                    objBookingModify.sExtra3 = ds.Tables[1].Rows[0]["sExtra3"] != DBNull.Value ? Convert.ToString(ds.Tables[1].Rows[0]["sExtra3"]) : "";

                    objBookingModify.TaxPerNight = Convert.ToString(ds.Tables[1].Rows[0]["TaxPerNight"]);
                    objBookingModify.dGstOnCommission = Convert.ToString(ds.Tables[1].Rows[0]["dGstOnCommission"]);
                    objBookingModify.dGstOnCommissionPercent = Convert.ToString(ds.Tables[1].Rows[0]["dGstOnCommissionPercent"]);
                    objBookingModify.dTotalCommission = Convert.ToString(ds.Tables[1].Rows[0]["dTotalCommission"]);

                    objBookingModify.dOfrServiceCharge = ds.Tables[1].Rows[0]["OFRServiceCharge"].ToString();
                    objBookingModify.CompanyId = Convert.ToInt32(ds.Tables[1].Rows[0]["iCompanyId"]);
                    objBookingModify.dOfrGSTONServiceCharge = ds.Tables[1].Rows[0]["GSTOFRServiceCharge"].ToString();
                    objBookingModify.dOfrGSTONServiceChargePercent = ds.Tables[1].Rows[0]["GSTOFRServiceCharge_Per"].ToString();

                    objBookingModify.dHotelGrossCharges = ds.Tables[1].Rows[0]["HotelGrossCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["HotelGrossCharges"].ToString()) : 0;
                    objBookingModify.dTotalPayableToHotel = Convert.ToString(ds.Tables[1].Rows[0]["dTotalPayableToHotel"]);
                    objBookingModify.dHotelRatePerNight = Convert.ToString(ds.Tables[1].Rows[0]["dHotelRatePerNight"]);

                    objBookingModify.dCustomerGrossCharges = ds.Tables[1].Rows[0]["dCustomerGrossCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dCustomerGrossCharges"].ToString()) : 0;
                    objBookingModify.dCustomerTotalCharges = ds.Tables[1].Rows[0]["dCustomerTotalCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dCustomerTotalCharges"].ToString()) : 0;
                   
                    
                    objBookingModify.GST_Same_State = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["GST_Same_State"].ToString()) ? Convert.ToBoolean(ds.Tables[1].Rows[0]["GST_Same_State"].ToString()) : false;
                    objBookingModify.SGST_Per = ds.Tables[1].Rows[0]["SGST_Per"].ToString();
                    objBookingModify.SGST_Val = ds.Tables[1].Rows[0]["SGST_Val"].ToString();

                    objBookingModify.EntityName = ds.Tables[1].Rows[0]["sGstnEntityName"].ToString();
                    objBookingModify.TypeOfEntity = ds.Tables[1].Rows[0]["sGstnEntityType"].ToString();
                    objBookingModify.GstnNumber = ds.Tables[1].Rows[0]["sGstnNumber"].ToString();
                    objBookingModify.EntityAddress = ds.Tables[1].Rows[0]["sGstnEntityAddress"].ToString();
                    objBookingModify.State = ds.Tables[1].Rows[0]["State_Customer"].ToString();
                    objBookingModify.StateCode = ds.Tables[1].Rows[0]["sStateCode_Customer"].ToString();

                    objBookingModify.sProvisionalBookingIdTG = Convert.ToString(ds.Tables[1].Rows[0]["sProvisionalBookingIdTG"]);
                    objBookingModify.sProvisionalTrxnTypeTG = Convert.ToString(ds.Tables[1].Rows[0]["sProvisionalTrxnTypeTG"]);
                    objBookingModify.sFinalBookingIdTG = Convert.ToString(ds.Tables[1].Rows[0]["sFinalBookingIdTG"]);
                    objBookingModify.sFinalTrxnTypeTG = Convert.ToString(ds.Tables[1].Rows[0]["sFinalTrxnTypeTG"]);
                    objBookingModify.sVendorId = Convert.ToString(ds.Tables[1].Rows[0]["iVendorId"]);
                    objBookingModify.NoOfRooms = Convert.ToInt32(ds.Tables[1].Rows[0]["Rooms"]);
                    objBookingModify.dTaxesForHotel = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[1].Rows[0]["HotelTax"])) ? Convert.ToDecimal(ds.Tables[1].Rows[0]["HotelTax"]) : 0;
                    objBookingModify.dTaxes = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[1].Rows[0]["Tax"])) ? Convert.ToDecimal(ds.Tables[1].Rows[0]["Tax"]) : 0;

                    objBookingModify.NoOfNight = Convert.ToInt32(ds.Tables[1].Rows[0]["NoOfNight"]);
                    objBookingModify.dCommissionPer = Convert.ToString(ds.Tables[1].Rows[0]["dCommissionPer"]);

                    objBookingModify.dHotelPublicRatePerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dHotelPublicRatePerNight_Bid"]);
                    objBookingModify.dDiscount_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dDiscount_Bid"]);
                    objBookingModify.dDiscountRatePerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dDiscountRatePerNight_Bid"]);
                    objBookingModify.dGuestRoomRatePerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dGuestRoomRatePerNight_Bid"]);
                    objBookingModify.dTotalDiscountedRate_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dTotalDiscountedRate_Bid"]);
                    objBookingModify.dGuestBidAmountPerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dGuestBidAmountPerNight_Bid"]);

                    switch (objBookingModify.TypeOfEntity.ToLower())
                    {
                        case "p":
                            objBookingModify.TypeOfEntity = GSTEntityType.p;
                            break;
                        case "c":
                            objBookingModify.TypeOfEntity = GSTEntityType.c;
                            break;
                        case "h":
                            objBookingModify.TypeOfEntity = GSTEntityType.h;
                            break;
                        case "f":
                            objBookingModify.TypeOfEntity = GSTEntityType.f;
                            break;
                        case "a":
                            objBookingModify.TypeOfEntity = GSTEntityType.a;
                            break;
                        case "t":
                            objBookingModify.TypeOfEntity = GSTEntityType.t;
                            break;
                        case "b":
                            objBookingModify.TypeOfEntity = GSTEntityType.b;
                            break;
                        case "l":
                            objBookingModify.TypeOfEntity = GSTEntityType.l;
                            break;
                        case "j":
                            objBookingModify.TypeOfEntity = GSTEntityType.j;
                            break;
                        case "g":
                            objBookingModify.TypeOfEntity = GSTEntityType.g;
                            break;

                        default:
                            break;
                    }



                    foreach (DataRow drR in ds.Tables[0].Rows)
                    {
                        eBookingRoomM objBookingRoom = new eBookingRoomM();
                        objBookingRoom.iBookingDetailsId = Convert.ToString(drR["iBookingDetailsID"]);
                        objBookingRoom.RoomId = Convert.ToString(drR["iRoomId"]);
                        objBookingRoom.RPId = Convert.ToString(drR["iRPId"]);
                        objBookingRoom.RoomName = Convert.ToString(drR["sRoomName"]);
                        objBookingRoom.RatePlan = Convert.ToString(drR["sRPName"]);
                        objBookingRoom.Adult = Convert.ToString(drR["iAdults"]);
                        objBookingRoom.Children = Convert.ToString(drR["iChildren"]);
                        objBookingRoom.sChildrenAge = Convert.ToString(drR["sChildrenAge"]);
                        objBookingRoom.Occupancy = Convert.ToString(drR["Occupancy"]);
                        objBookingRoom.BedPrefer = Convert.ToString(drR["BedPref"]);
                        objBookingRoom.ExtraBed = Convert.ToString(drR["ExtraBeds"]);
                        objBookingRoom.GuestName = Convert.ToString(drR["GuestName"]);
                        objBookingRoom.PolicyName = Convert.ToString(drR["sPolicyName"]);
                        objBookingRoom.AmenityRatePlan = Convert.ToString(drR["sAmenityRatePlan"]);
                        objBookingRoom.RoomNo = Convert.ToString(drR["Room"]);
                        objBookingRoom.RoomRate = drR["dRoomRate"] != DBNull.Value ? Convert.ToDecimal(drR["dRoomRate"]) : 0;
                        objBookingRoom.dTaxes = drR["dTaxes"] != DBNull.Value ? Convert.ToDecimal(drR["dTaxes"]) : 0;
                        objBookingRoom.dExtraBedRate = drR["dExtraBedRate"] != DBNull.Value ? Convert.ToDecimal(drR["dExtraBedRate"]) : 0;


                        objBookingRoomArr.Add(objBookingRoom);
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        objBookingModify.sMessage = Convert.ToString(ds.Tables[2].Rows[0]["Msg"]);
                        objBookingModify.RefundAmount = Convert.ToString(ds.Tables[2].Rows[0]["RefundAmount"]);
                    }

                    //Fetch credit card images
                    string[] arrCreditCards = new string[1000];
                    string CreditCards = Convert.ToString(ds.Tables[1].Rows[0]["sCreditCardId"]);
                    if (CreditCards != "")
                    {
                        arrCreditCards = CreditCards.Split(',');
                        List<CreditCards> lstcreditcards = new List<CreditCards>();
                        foreach (var item in arrCreditCards)
                        {
                            lstcreditcards.Add(new CreditCards()
                            {
                                sImg = item
                            });
                        }
                        objBookingModify.lstCreditCards = lstcreditcards;
                    }

                    objBookingModify.slargeMapURL = "https://www.google.com/maps/place/" + objBookingModify.HotelName + "+-+" + objBookingModify.sCity.Trim() + "/@" + objBookingModify.dLatitude + "," + objBookingModify.dLongitude + ",17z?hl=en-US";
                }

                objBookingModify.BookingRoomDetails = objBookingRoomArr;
                objBookingModify.sRatePlanName = string.Join(",", objBookingRoomArr.Select(x => x.RatePlan).ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objBookingModify;
        }
        public static List<string> GetBookingGuestDetails(long bookingId)
        {
            var guestDetails = new List<string>();

            using (var db = new OneFineRateEntities())
            {
                var guestDetails1 = (from guestMap in db.tblBookingGuestMaps
                                     join bookingDetails in db.tblBookingDetailsTxes on guestMap.iBookingDetailsID equals bookingDetails.iBookingDetailsID
                                     join bookings in db.tblBookingTxes on bookingDetails.iBookingId equals bookings.iBookingId
                                     where bookings.iBookingId == bookingId
                                     select new { guestEmail = guestMap.sGuestEmail }).Distinct().ToList();

                guestDetails = guestDetails1.Where(x => !string.IsNullOrEmpty(x.guestEmail)).Select(x => x.guestEmail).ToList();
            }
            return guestDetails;


        }

        public static string GetBidRateInclusions(long bookingId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var resultSet = db.tblBookedBidAmenities.Where(x => x.iBookingId == bookingId).ToList();
                if (resultSet.Count > 0)
                {
                    var inclusionsList = resultSet.Select(x => x.sAmenity + " " + x.sApplicability).Distinct().ToList();
                    string inclusions = string.Join(",", inclusionsList);
                    return inclusions;
                }
                return string.Empty;
            }
        }

        // Get Modify Booking Details for Notifications
        //public static eBookingModify GetBookingModifyDetails_Notifications_RM(long BookingId, bool CurrencyType = false)
        //{
        //    eBookingModify objBookingModify = new eBookingModify();
        //    List<eBookingRoomM> objBookingRoomArr = new List<eBookingRoomM>();
        //    List<eBookingRatePlan> objBookingRatePlanArr = new List<eBookingRatePlan>();
        //    try
        //    {
        //        SqlParameter[] MyParam = new SqlParameter[2];
        //        MyParam[0] = new SqlParameter("@bookingid", BookingId);
        //        MyParam[1] = new SqlParameter("@CurrencyType", CurrencyType);
        //        DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetBookingDetailsByBookingId_ForNotifications", MyParam);

        //        if (ds.Tables[1].Rows.Count > 0)
        //        {
        //            objBookingModify.PropId = Convert.ToInt32(ds.Tables[1].Rows[0]["iPropId"]);
        //            objBookingModify.BookingId = Convert.ToString(ds.Tables[1].Rows[0]["iBookingId"]);
        //            objBookingModify.ReservationDate = Convert.ToString(ds.Tables[1].Rows[0]["dtReservationDate"]);
        //            objBookingModify.BookingStatus = Convert.ToString(ds.Tables[1].Rows[0]["Status"]);
        //            objBookingModify.ccType = Convert.ToString(ds.Tables[1].Rows[0]["ccType"]);
        //            objBookingModify.Reservation = Convert.ToString(ds.Tables[1].Rows[0]["Reservation"]);
        //            objBookingModify.CheckIn = Convert.ToString(ds.Tables[1].Rows[0]["dtCheckIn"]);
        //            objBookingModify.ChekOut = Convert.ToString(ds.Tables[1].Rows[0]["dtChekOut"]);
        //            objBookingModify.Booker = Convert.ToString(ds.Tables[1].Rows[0]["Booker"]);
        //            objBookingModify.EmailOFR = Convert.ToString(ds.Tables[1].Rows[0]["sEmailOFR"]);
        //            objBookingModify.CountryPhoneCode = Convert.ToString(ds.Tables[1].Rows[0]["sCountryPhoneCode"]);
        //            objBookingModify.MobileOFR = Convert.ToString(ds.Tables[1].Rows[0]["sMobileOFR"]);
        //            objBookingModify.HotelName = Convert.ToString(ds.Tables[1].Rows[0]["sHotelName"]);
        //            objBookingModify.StreetAddress = Convert.ToString(ds.Tables[1].Rows[0]["StreetAddress"]);
        //            objBookingModify.Address = Convert.ToString(ds.Tables[1].Rows[0]["Address"]);
        //            objBookingModify.StarCategoryId = Convert.ToString(ds.Tables[1].Rows[0]["iStarCategoryId"]);
        //            objBookingModify.AvgAmount = Convert.ToString(ds.Tables[1].Rows[0]["AvgAmount"]);
        //            objBookingModify.TotalAmount = Convert.ToString(ds.Tables[1].Rows[0]["TotalAmount"]);
        //            objBookingModify.Tax = Convert.ToString(ds.Tables[1].Rows[0]["Tax"]);
        //            objBookingModify.ExtraBedAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExtraBedAmount"]);
        //            objBookingModify.Comm = Convert.ToString(ds.Tables[1].Rows[0]["Comm"]);
        //            objBookingModify.SubTotal = Convert.ToString(ds.Tables[1].Rows[0]["SubTotal"]);
        //            objBookingModify.dccPrice = Convert.ToDecimal(ds.Tables[1].Rows[0]["dccPrice"]);
        //            objBookingModify.dccDiscount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dccDiscount"]);
        //            objBookingModify.dBidAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dBidAmount"]);
        //            objBookingModify.sExtra2 = Convert.ToDecimal(ds.Tables[1].Rows[0]["sExtra2"]);

        //            objBookingModify.SpecialRequest = Convert.ToString(ds.Tables[1].Rows[0]["sSpecialRequests"]);
        //            objBookingModify.ExpectedCheckIn = Convert.ToString(ds.Tables[1].Rows[0]["sExpectedCheckIn"]);
        //            objBookingModify.SpecialOccasion = Convert.ToString(ds.Tables[1].Rows[0]["SpecialOccasion"]);

        //            objBookingModify.Pickup = Convert.ToString(ds.Tables[1].Rows[0]["spref_pickup"]);
        //            objBookingModify.FlightNumber = Convert.ToString(ds.Tables[1].Rows[0]["sFlightNumber"]);
        //            objBookingModify.EstimateArrivalTime = Convert.ToString(ds.Tables[1].Rows[0]["sEstArrivalTime"]);
        //            objBookingModify.Landing = Convert.ToString(ds.Tables[1].Rows[0]["sLandingAt"]);
        //            objBookingModify.TypeOfCar = Convert.ToString(ds.Tables[1].Rows[0]["sTypeOfCar"]);
        //            objBookingModify.NoOfCar = Convert.ToString(ds.Tables[1].Rows[0]["iNoOFCars"]);

        //            objBookingModify.SingleLady = Convert.ToString(ds.Tables[1].Rows[0]["SingleLady"]);
        //            objBookingModify.SmokingRoom = Convert.ToString(ds.Tables[1].Rows[0]["SmokingRoom"]);
        //            objBookingModify.Elevator = Convert.ToString(ds.Tables[1].Rows[0]["Elevator"]);
        //            objBookingModify.QuietRoom = Convert.ToString(ds.Tables[1].Rows[0]["QuietRoom"]);

        //            objBookingModify.sCheckInHH = Convert.ToString(ds.Tables[1].Rows[0]["sCheckInHH"]);
        //            objBookingModify.sCheckInMM = Convert.ToString(ds.Tables[1].Rows[0]["sCheckInMM"]);
        //            objBookingModify.sCheckoutHH = Convert.ToString(ds.Tables[1].Rows[0]["sCheckoutHH"]);
        //            objBookingModify.sCheckoutMM = Convert.ToString(ds.Tables[1].Rows[0]["sCheckoutMM"]);
        //            objBookingModify.iMinCheckInAge = Convert.ToByte(ds.Tables[1].Rows[0]["iMinCheckInAge"]);
        //            objBookingModify.dExtraBedCharges = Convert.ToDecimal(ds.Tables[1].Rows[0]["dExtraBedCharges"]);
        //            objBookingModify.iExtraBedRequiredFrom = Convert.ToInt32(ds.Tables[1].Rows[0]["iExtraBedRequiredFrom"]);
        //            objBookingModify.iComplimentaryStayAge = Convert.ToInt32(ds.Tables[1].Rows[0]["iComplimentaryStayAge"]);
        //            objBookingModify.bAlcoholAllowedOnsite = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAlcoholAllowedOnsite"]);
        //            objBookingModify.bAlcoholServedOnsite = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAlcoholServedOnsite"]);
        //            objBookingModify.bVisitorsAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bVisitorsAllowed"]);
        //            objBookingModify.bPetsAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bPetsAllowed"]);
        //            objBookingModify.bPartiesAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bPartiesAllowed"]);
        //            objBookingModify.sImgUrl = Convert.ToString(ds.Tables[1].Rows[0]["sImgUrl"]);
        //            objBookingModify.Symbol = Convert.ToString(ds.Tables[1].Rows[0]["Symbol"]);
        //            objBookingModify.sCurrencyCode = Convert.ToString(ds.Tables[1].Rows[0]["sCurrencyCode"]);
        //            objBookingModify.sCity = Convert.ToString(ds.Tables[1].Rows[0]["sCity"]);
        //            objBookingModify.dLatitude = Convert.ToDecimal(ds.Tables[1].Rows[0]["dLatitude"]);
        //            objBookingModify.dLongitude = Convert.ToDecimal(ds.Tables[1].Rows[0]["dLongitude"]);
        //            objBookingModify.dTotalNegotiateAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dTotalNegotiateAmount"]);
        //            objBookingModify.dAdvNegotiateAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dAdvNegotiateAmount"]);
        //            objBookingModify.dBalanceAmt = Convert.ToDecimal(ds.Tables[1].Rows[0]["dBalanceAmt"]);
        //            objBookingModify.cBookingType = Convert.ToString(ds.Tables[1].Rows[0]["cBookingType"]);
        //            objBookingModify.cBookingStatus = Convert.ToString(ds.Tables[1].Rows[0]["cBookingStatus"]);
        //            objBookingModify.dSubTotalNego = Convert.ToDecimal(ds.Tables[1].Rows[0]["dSubTotalNego"]);
        //            objBookingModify.dOrginalSubTotalNego = Convert.ToDecimal(ds.Tables[1].Rows[0]["dOrginalSubTotalNego"]);
        //            objBookingModify.dCounterOffer = Convert.ToDecimal(ds.Tables[1].Rows[0]["dCounterOffer"]);
        //            objBookingModify.dSubTotalCounter = Convert.ToDecimal(ds.Tables[1].Rows[0]["dSubTotalCounter"]);
        //            objBookingModify.dCustomerPayable = ds.Tables[1].Rows[0]["dCustomerPayable"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dCustomerPayable"]) : 0;
        //            objBookingModify.ActualBookingType = Convert.ToString(ds.Tables[1].Rows[0]["ActualBookingType"]);
        //            objBookingModify.b24HrsCheckIn = Convert.ToBoolean(ds.Tables[1].Rows[0]["b24HrsCheckIn"]);
        //            objBookingModify.bEarlyCheckInChargeable = Convert.ToBoolean(ds.Tables[1].Rows[0]["bEarlyCheckInChargeable"]);
        //            objBookingModify.b24HrsCheckout = Convert.ToBoolean(ds.Tables[1].Rows[0]["b24HrsCheckout"]);
        //            objBookingModify.bEarlyCheckoutChargeable = Convert.ToBoolean(ds.Tables[1].Rows[0]["bEarlyCheckoutChargeable"]);
        //            objBookingModify.dDiscountedBidPrice = ds.Tables[1].Rows[0]["dDiscountedBidPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dDiscountedBidPrice"]) : 0;
        //            objBookingModify.iLinkedBookingId = Convert.ToInt32(ds.Tables[1].Rows[0]["iLinkedBookingId"]);
        //            //objBookingModify.bPromoDiscount = ds.Tables[1].Rows[0]["bPromoDiscount"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[0]["bPromoDiscount"]) : false;
        //            //objBookingModify.bPromoAmenity = Convert.ToString(ds.Tables[1].Rows[0]["bPromoAmenity"]);
        //            objBookingModify.sExtra1 = ds.Tables[1].Rows[0]["sExtra1"] != DBNull.Value ? Convert.ToString(ds.Tables[1].Rows[0]["sExtra1"]) : "";


        //            objBookingModify.dTaxesForHotel = Convert.ToDecimal(ds.Tables[1].Rows[0]["HotelTax"]);
        //            objBookingModify.TaxPerNight = Convert.ToString(ds.Tables[1].Rows[0]["TaxPerNight"]);

        //            objBookingModify.dHotelGrossCharges = ds.Tables[1].Rows[0]["HotelGrossCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["HotelGrossCharges"]) : 0;
        //            objBookingModify.dTotalPayableToHotel = Convert.ToString(ds.Tables[1].Rows[0]["dTotalPayableToHotel"]);

        //            objBookingModify.dGstOnCommission = Convert.ToString(ds.Tables[1].Rows[0]["dGstOnCommission"]);
        //            objBookingModify.dGstOnCommissionPercent = Convert.ToString(ds.Tables[1].Rows[0]["dGstOnCommissionPercent"]);
        //            objBookingModify.dTotalCommission = Convert.ToString(ds.Tables[1].Rows[0]["dTotalCommission"]);

        //            objBookingModify.dOfrServiceCharge = ds.Tables[1].Rows[0]["OFRServiceCharge"].ToString();
        //            objBookingModify.dOfrGSTONServiceCharge = ds.Tables[1].Rows[0]["GSTOFRServiceCharge"].ToString();
        //            objBookingModify.dOfrGSTONServiceChargePercent = ds.Tables[1].Rows[0]["GSTOFRServiceCharge_Per"].ToString();

        //            objBookingModify.GST_Same_State = ds.Tables[1].Rows[0]["GST_Same_State"].ToString() == "0" ? false : true;
        //            objBookingModify.SGST_Per = ds.Tables[1].Rows[0]["SGST_Per"].ToString();
        //            objBookingModify.SGST_Val = ds.Tables[1].Rows[0]["SGST_Val"].ToString();

        //            objBookingModify.EntityName = ds.Tables[1].Rows[0]["sGstnEntityName"].ToString();
        //            objBookingModify.TypeOfEntity = ds.Tables[1].Rows[0]["sGstnEntityType"].ToString();
        //            objBookingModify.GstnNumber = ds.Tables[1].Rows[0]["sGstnNumber"].ToString();
        //            objBookingModify.EntityAddress = ds.Tables[1].Rows[0]["sGstnEntityAddress"].ToString();
        //            objBookingModify.State = ds.Tables[1].Rows[0]["State_Customer"].ToString();
        //            objBookingModify.StateCode = ds.Tables[1].Rows[0]["sStateCode_Customer"].ToString();


        //            objBookingModify.sProvisionalBookingIdTG = Convert.ToString(ds.Tables[1].Rows[0]["sProvisionalBookingIdTG"]);
        //            objBookingModify.sProvisionalTrxnTypeTG = Convert.ToString(ds.Tables[1].Rows[0]["sProvisionalTrxnTypeTG"]);
        //            objBookingModify.sFinalBookingIdTG = Convert.ToString(ds.Tables[1].Rows[0]["sFinalBookingIdTG"]);
        //            objBookingModify.sFinalTrxnTypeTG = Convert.ToString(ds.Tables[1].Rows[0]["sFinalTrxnTypeTG"]);
        //            objBookingModify.sVendorId = Convert.ToString(ds.Tables[1].Rows[0]["iVendorId"]);
        //            objBookingModify.NoOfRooms = Convert.ToInt32(ds.Tables[1].Rows[0]["Rooms"]);
        //            objBookingModify.NoOfNight = Convert.ToInt32(ds.Tables[1].Rows[0]["NoOfNight"]);


        //            objBookingModify.dHotelPublicRatePerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dHotelPublicRatePerNight_Bid"]);
        //            objBookingModify.dDiscount_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dDiscount_Bid"]);
        //            objBookingModify.dDiscountRatePerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dDiscountRatePerNight_Bid"]);
        //            objBookingModify.dTotalDiscountedRate_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dTotalDiscountedRate_Bid"]);
        //            objBookingModify.dGuestBidAmountPerNight_Bid = Convert.ToString(ds.Tables[1].Rows[0]["dGuestBidAmountPerNight_Bid"]);

        //            objBookingModify.dCommissionPer = Convert.ToString(ds.Tables[1].Rows[0]["dCommissionPer"]);


        //            switch (objBookingModify.TypeOfEntity.ToLower())
        //            {
        //                case "p":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.p;
        //                    break;
        //                case "c":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.c;
        //                    break;
        //                case "h":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.h;
        //                    break;
        //                case "f":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.f;
        //                    break;
        //                case "a":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.a;
        //                    break;
        //                case "t":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.t;
        //                    break;
        //                case "b":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.b;
        //                    break;
        //                case "l":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.l;
        //                    break;
        //                case "j":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.j;
        //                    break;
        //                case "g":
        //                    objBookingModify.TypeOfEntity = GSTEntityType.g;
        //                    break;

        //                default:
        //                    break;
        //            }


        //            foreach (DataRow drR in ds.Tables[0].Rows)
        //            {
        //                eBookingRoomM objBookingRoom = new eBookingRoomM();
        //                objBookingRoom.iBookingDetailsId = Convert.ToString(drR["iBookingDetailsID"]);
        //                objBookingRoom.RoomId = Convert.ToString(drR["iRoomId"]);
        //                objBookingRoom.RPId = Convert.ToString(drR["iRPId"]);
        //                objBookingRoom.RoomName = Convert.ToString(drR["sRoomName"]);
        //                objBookingRoom.RatePlan = Convert.ToString(drR["sRPName"]);
        //                objBookingRoom.Adult = Convert.ToString(drR["iAdults"]);
        //                objBookingRoom.Children = Convert.ToString(drR["iChildren"]);
        //                objBookingRoom.sChildrenAge = Convert.ToString(drR["sChildrenAge"]);
        //                objBookingRoom.Occupancy = Convert.ToString(drR["Occupancy"]);
        //                objBookingRoom.BedPrefer = Convert.ToString(drR["BedPref"]);
        //                objBookingRoom.ExtraBed = Convert.ToString(drR["ExtraBeds"]);
        //                objBookingRoom.GuestName = Convert.ToString(drR["GuestName"]);
        //                objBookingRoom.PolicyName = Convert.ToString(drR["sPolicyName"]);
        //                objBookingRoom.AmenityRatePlan = Convert.ToString(drR["sAmenityRatePlan"]);
        //                objBookingRoom.RoomNo = Convert.ToString(drR["Room"]);
        //                objBookingRoom.RoomRate = drR["dRoomRate"] != DBNull.Value ? Convert.ToDecimal(drR["dRoomRate"]) : 0;
        //                objBookingRoom.dTaxes = drR["dTaxes"] != DBNull.Value ? Convert.ToDecimal(drR["dTaxes"]) : 0;
        //                objBookingRoom.dExtraBedRate = drR["dExtraBedRate"] != DBNull.Value ? Convert.ToDecimal(drR["dExtraBedRate"]) : 0;


        //                objBookingRoomArr.Add(objBookingRoom);
        //            }

        //            if (ds.Tables[2].Rows.Count > 0)
        //            {
        //                objBookingModify.sMessage = Convert.ToString(ds.Tables[2].Rows[0]["Msg"]);
        //            }

        //            //Fetch credit card images
        //            string[] arrCreditCards = new string[1000];
        //            string CreditCards = Convert.ToString(ds.Tables[1].Rows[0]["sCreditCardId"]);
        //            if (CreditCards != "")
        //            {
        //                arrCreditCards = CreditCards.Split(',');
        //                List<CreditCards> lstcreditcards = new List<CreditCards>();
        //                foreach (var item in arrCreditCards)
        //                {
        //                    lstcreditcards.Add(new CreditCards()
        //                    {
        //                        sImg = item
        //                    });
        //                }
        //                objBookingModify.lstCreditCards = lstcreditcards;
        //            }

        //            objBookingModify.slargeMapURL = "https://www.google.com/maps/place/" + objBookingModify.HotelName + "+-+" + objBookingModify.sCity.Trim() + "/@" + objBookingModify.dLatitude + "," + objBookingModify.dLongitude + ",17z?hl=en-US";
        //        }

        //        objBookingModify.BookingRoomDetails = objBookingRoomArr;
        //        objBookingModify.sRatePlanName = string.Join(",", objBookingRoomArr.Select(x => x.RatePlan).ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return objBookingModify;
        //}

        public static string GetCompanyNameByUserEmail(long userId, int? companyId = null)
        {
            if (userId == 0 && companyId.HasValue)
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    return db.tblCorporateCompanyMs.Where(x => x.iCompanyId == companyId.Value).Select(x => x.sCompanyName).SingleOrDefault();
                }
            }

            var user = BL_WebsiteUser.CheckCorporateCustomerById(userId);
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                if (user == null)
                {
                    throw new ArgumentNullException("user");
                }
                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new ArgumentNullException("Email");
                }

                MailAddress mail_address_personal = new MailAddress(user.Email);

                string domainName_personal = mail_address_personal.Host;

                MailAddress mail_address_corporate = new MailAddress(user.CorporateEmail);

                string domainName_corporate = mail_address_corporate.Host;

                var blackListFound = db.tblBlackListedDomains.FirstOrDefault(x =>
                    x.sDomainName == user.Email
                    || x.sDomainName == domainName_personal
                    || x.sDomainName == user.CorporateEmail
                    || x.sDomainName == domainName_corporate);

                var corporateDomainFound = db.tblCorporateDomainMaps.FirstOrDefault(x =>
                    x.sDomainName == user.CorporateEmail
                    || x.sDomainName == domainName_corporate
                    || x.sDomainName == user.Email
                    || x.sDomainName == domainName_personal);
                var companyName = (from cropCompany in db.tblCorporateCompanyMs
                                   join corporatedomain in db.tblCorporateDomainMaps on cropCompany.iCompanyId equals corporatedomain.iCompanyId
                                   where
                                       corporatedomain.sDomainName == corporateDomainFound.sDomainName
                                   select cropCompany.sCompanyName).FirstOrDefault();
                return companyName;

            }

        }

        // Get Modify Booking Details by Email or Phone number
        public static eBookingModify GetBookingDetailsByEmailOrPhone(long BookingId, string phone, string email)
        {
            eBookingModify objBookingModify = new eBookingModify();
            List<eBookingRoomM> objBookingRoomArr = new List<eBookingRoomM>();
            List<eBookingRatePlan> objBookingRatePlanArr = new List<eBookingRatePlan>();
            try
            {
                SqlParameter[] MyParam = new SqlParameter[3];
                MyParam[0] = new SqlParameter("@bookingid", BookingId);
                MyParam[1] = new SqlParameter("@sEmail", email);
                MyParam[2] = new SqlParameter("@sPhone", phone);
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetBookingDetailsByBookingIdPhoneEmail", MyParam);

                if (ds.Tables[1].Rows.Count > 0)
                {
                    objBookingModify.PropId = Convert.ToInt32(ds.Tables[1].Rows[0]["iPropId"]);
                    objBookingModify.BookingId = Convert.ToString(ds.Tables[1].Rows[0]["iBookingId"]);
                    objBookingModify.ReservationDate = Convert.ToString(ds.Tables[1].Rows[0]["dtReservationDate"]);
                    objBookingModify.BookingStatus = Convert.ToString(ds.Tables[1].Rows[0]["Status"]);
                    objBookingModify.ccType = Convert.ToString(ds.Tables[1].Rows[0]["ccType"]);
                    objBookingModify.Reservation = Convert.ToString(ds.Tables[1].Rows[0]["Reservation"]);
                    objBookingModify.CheckIn = Convert.ToString(ds.Tables[1].Rows[0]["dtCheckIn"]);
                    objBookingModify.ChekOut = Convert.ToString(ds.Tables[1].Rows[0]["dtChekOut"]);
                    objBookingModify.Booker = Convert.ToString(ds.Tables[1].Rows[0]["Booker"]);
                    objBookingModify.EmailOFR = Convert.ToString(ds.Tables[1].Rows[0]["sEmailOFR"]);
                    objBookingModify.MobileOFR = Convert.ToString(ds.Tables[1].Rows[0]["sMobileOFR"]);
                    objBookingModify.HotelName = Convert.ToString(ds.Tables[1].Rows[0]["sHotelName"]);
                    objBookingModify.StreetAddress = Convert.ToString(ds.Tables[1].Rows[0]["StreetAddress"]);
                    objBookingModify.Address = Convert.ToString(ds.Tables[1].Rows[0]["Address"]);
                    objBookingModify.StarCategoryId = Convert.ToString(ds.Tables[1].Rows[0]["iStarCategoryId"]);
                    objBookingModify.AvgAmount = Convert.ToString(ds.Tables[1].Rows[0]["AvgAmount"]);
                    objBookingModify.TotalAmount = Convert.ToString(ds.Tables[1].Rows[0]["TotalAmount"]);
                    objBookingModify.Tax = Convert.ToString(ds.Tables[1].Rows[0]["Tax"]);
                    objBookingModify.ExtraBedAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExtraBedAmount"]);
                    objBookingModify.Comm = Convert.ToString(ds.Tables[1].Rows[0]["Comm"]);
                    objBookingModify.SubTotal = Convert.ToString(ds.Tables[1].Rows[0]["SubTotal"]);
                    objBookingModify.dccPrice = Convert.ToDecimal(ds.Tables[1].Rows[0]["dccPrice"]);
                    objBookingModify.dccDiscount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dccDiscount"]);
                    objBookingModify.sExtra2 = Convert.ToDecimal(ds.Tables[1].Rows[0]["sExtra2"]);

                    objBookingModify.SpecialRequest = Convert.ToString(ds.Tables[1].Rows[0]["sSpecialRequests"]);
                    objBookingModify.ExpectedCheckIn = Convert.ToString(ds.Tables[1].Rows[0]["sExpectedCheckIn"]);
                    objBookingModify.SpecialOccasion = Convert.ToString(ds.Tables[1].Rows[0]["SpecialOccasion"]);

                    objBookingModify.Pickup = Convert.ToString(ds.Tables[1].Rows[0]["spref_pickup"]);
                    objBookingModify.FlightNumber = Convert.ToString(ds.Tables[1].Rows[0]["sFlightNumber"]);
                    objBookingModify.EstimateArrivalTime = Convert.ToString(ds.Tables[1].Rows[0]["sEstArrivalTime"]);
                    objBookingModify.Landing = Convert.ToString(ds.Tables[1].Rows[0]["sLandingAt"]);
                    objBookingModify.TypeOfCar = Convert.ToString(ds.Tables[1].Rows[0]["sTypeOfCar"]);
                    objBookingModify.NoOfCar = Convert.ToString(ds.Tables[1].Rows[0]["iNoOFCars"]);

                    objBookingModify.SingleLady = Convert.ToString(ds.Tables[1].Rows[0]["SingleLady"]);
                    objBookingModify.SmokingRoom = Convert.ToString(ds.Tables[1].Rows[0]["SmokingRoom"]);
                    objBookingModify.Elevator = Convert.ToString(ds.Tables[1].Rows[0]["Elevator"]);
                    objBookingModify.QuietRoom = Convert.ToString(ds.Tables[1].Rows[0]["QuietRoom"]);

                    objBookingModify.sCheckInHH = Convert.ToString(ds.Tables[1].Rows[0]["sCheckInHH"]);
                    objBookingModify.sCheckInMM = Convert.ToString(ds.Tables[1].Rows[0]["sCheckInMM"]);
                    objBookingModify.sCheckoutHH = Convert.ToString(ds.Tables[1].Rows[0]["sCheckoutHH"]);
                    objBookingModify.sCheckoutMM = Convert.ToString(ds.Tables[1].Rows[0]["sCheckoutMM"]);
                    objBookingModify.iMinCheckInAge = Convert.ToByte(ds.Tables[1].Rows[0]["iMinCheckInAge"]);
                    objBookingModify.dExtraBedCharges = Convert.ToDecimal(ds.Tables[1].Rows[0]["dExtraBedCharges"]);
                    objBookingModify.iExtraBedRequiredFrom = Convert.ToInt32(ds.Tables[1].Rows[0]["iExtraBedRequiredFrom"]);
                    objBookingModify.iComplimentaryStayAge = Convert.ToInt32(ds.Tables[1].Rows[0]["iComplimentaryStayAge"]);
                    objBookingModify.bAlcoholAllowedOnsite = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAlcoholAllowedOnsite"] != DBNull.Value);
                    objBookingModify.bAlcoholServedOnsite = Convert.ToBoolean(ds.Tables[1].Rows[0]["bAlcoholServedOnsite"] != DBNull.Value);
                    objBookingModify.bVisitorsAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bVisitorsAllowed"] != DBNull.Value);
                    objBookingModify.bPetsAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bPetsAllowed"] != DBNull.Value);
                    objBookingModify.bPartiesAllowed = Convert.ToBoolean(ds.Tables[1].Rows[0]["bPartiesAllowed"] != DBNull.Value);
                    objBookingModify.sImgUrl = Convert.ToString(ds.Tables[1].Rows[0]["sImgUrl"]);
                    objBookingModify.Symbol = Convert.ToString(ds.Tables[1].Rows[0]["Symbol"]);
                    objBookingModify.sCurrencyCode = Convert.ToString(ds.Tables[1].Rows[0]["sCurrencyCode"]);
                    objBookingModify.sCity = Convert.ToString(ds.Tables[1].Rows[0]["sCity"]);
                    objBookingModify.dLatitude = Convert.ToDecimal(ds.Tables[1].Rows[0]["dLatitude"]);
                    objBookingModify.dLongitude = Convert.ToDecimal(ds.Tables[1].Rows[0]["dLongitude"]);
                    objBookingModify.dTotalNegotiateAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dTotalNegotiateAmount"]);
                    objBookingModify.dAdvNegotiateAmount = Convert.ToDecimal(ds.Tables[1].Rows[0]["dAdvNegotiateAmount"]);
                    objBookingModify.dBalanceAmt = Convert.ToDecimal(ds.Tables[1].Rows[0]["dBalanceAmt"]);
                    objBookingModify.cBookingType = Convert.ToString(ds.Tables[1].Rows[0]["cBookingType"]);
                    objBookingModify.cBookingStatus = Convert.ToString(ds.Tables[1].Rows[0]["cBookingStatus"]);
                    objBookingModify.dSubTotalNego = Convert.ToDecimal(ds.Tables[1].Rows[0]["dSubTotalNego"]);
                    objBookingModify.dOrginalSubTotalNego = Convert.ToDecimal(ds.Tables[1].Rows[0]["dOrginalSubTotalNego"]);
                    objBookingModify.dCounterOffer = Convert.ToDecimal(ds.Tables[1].Rows[0]["dCounterOffer"]);
                    objBookingModify.dSubTotalCounter = Convert.ToDecimal(ds.Tables[1].Rows[0]["dSubTotalCounter"]);
                    objBookingModify.dCustomerPayable = ds.Tables[1].Rows[0]["dCustomerPayable"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dCustomerPayable"]) : 0;
                    objBookingModify.ActualBookingType = Convert.ToString(ds.Tables[1].Rows[0]["ActualBookingType"]);
                    objBookingModify.b24HrsCheckIn = Convert.ToBoolean(ds.Tables[1].Rows[0]["b24HrsCheckIn"]);
                    objBookingModify.bEarlyCheckInChargeable = Convert.ToBoolean(ds.Tables[1].Rows[0]["bEarlyCheckInChargeable"]);
                    objBookingModify.b24HrsCheckout = Convert.ToBoolean(ds.Tables[1].Rows[0]["b24HrsCheckout"]);
                    objBookingModify.bEarlyCheckoutChargeable = Convert.ToBoolean(ds.Tables[1].Rows[0]["bEarlyCheckoutChargeable"]);
                    objBookingModify.dDiscountedBidPrice = ds.Tables[1].Rows[0]["dDiscountedBidPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[1].Rows[0]["dDiscountedBidPrice"]) : 0;
                    objBookingModify.iLinkedBookingId = Convert.ToInt32(ds.Tables[1].Rows[0]["iLinkedBookingId"]);
                    //objBookingModify.bPromoDiscount = ds.Tables[1].Rows[0]["bPromoDiscount"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[1].Rows[0]["bPromoDiscount"]) : false;
                    //objBookingModify.bPromoAmenity = Convert.ToString(ds.Tables[1].Rows[0]["bPromoAmenity"]);

                    foreach (DataRow drR in ds.Tables[0].Rows)
                    {
                        eBookingRoomM objBookingRoom = new eBookingRoomM();
                        objBookingRoom.iBookingDetailsId = Convert.ToString(drR["iBookingDetailsID"]);
                        objBookingRoom.RoomId = Convert.ToString(drR["iRoomId"]);
                        objBookingRoom.RPId = Convert.ToString(drR["iRPId"]);
                        objBookingRoom.RoomName = Convert.ToString(drR["sRoomName"]);
                        objBookingRoom.RatePlan = Convert.ToString(drR["sRPName"]);
                        objBookingRoom.Adult = Convert.ToString(drR["iAdults"]);
                        objBookingRoom.Children = Convert.ToString(drR["iChildren"]);
                        objBookingRoom.Occupancy = Convert.ToString(drR["Occupancy"]);
                        objBookingRoom.BedPrefer = Convert.ToString(drR["BedPref"]);
                        objBookingRoom.ExtraBed = Convert.ToString(drR["ExtraBeds"]);
                        objBookingRoom.GuestName = Convert.ToString(drR["GuestName"]);
                        objBookingRoom.PolicyName = Convert.ToString(drR["sPolicyName"]);
                        objBookingRoom.AmenityRatePlan = Convert.ToString(drR["sAmenityRatePlan"]);
                        objBookingRoom.RoomNo = Convert.ToString(drR["Room"]);
                        objBookingRoom.RoomRate = drR["dRoomRate"] != DBNull.Value ? Convert.ToDecimal(drR["dRoomRate"]) : 0;


                        objBookingRoomArr.Add(objBookingRoom);
                    }

                    //Fetch credit card images
                    string[] arrCreditCards = new string[1000];
                    string CreditCards = Convert.ToString(ds.Tables[1].Rows[0]["sCreditCardId"]);
                    if (CreditCards != "")
                    {
                        arrCreditCards = CreditCards.Split(',');
                        List<CreditCards> lstcreditcards = new List<CreditCards>();
                        foreach (var item in arrCreditCards)
                        {
                            lstcreditcards.Add(new CreditCards()
                            {
                                sImg = item
                            });
                        }
                        objBookingModify.lstCreditCards = lstcreditcards;
                    }

                    objBookingModify.slargeMapURL = "https://www.google.com/maps/place/" + objBookingModify.HotelName + "+-+" + objBookingModify.sCity.Trim() + "/@" + objBookingModify.dLatitude + "," + objBookingModify.dLongitude + ",17z?hl=en-US";
                }

                objBookingModify.BookingRoomDetails = objBookingRoomArr;
                //objBookingModify.BookingRatePlans = objBookingRatePlanArr;
            }
            catch (Exception)
            {
            }
            return objBookingModify;
        }

        // Get Cancellation charge for cancel Booking
        public static eCancelBookingResponse GetCancellationCharge(long BookingId, decimal timZone, DataTable dtTblBookingDetailIds)
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            eCancelBookingResponse response = new eCancelBookingResponse();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlParameter[] MyParams = new SqlParameter[3];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iBookingId", BookingId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@BigIds", dtTblBookingDetailIds);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@iCountryOffset", timZone);
                con.Open();
                var ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspCalculateCancellationCharges", MyParams);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var result = ds.Tables[0].Rows[0][0];
                    response.Refund = Convert.ToDecimal(Convert.ToString(Convert.ToString(ds.Tables[0].Rows[0][2])) == null ? 0 : ds.Tables[0].Rows[0][2]);
                    response.CancellationPolicy = Convert.ToString(ds.Tables[0].Rows[0][1]);
                    response.CancellationCharges = string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0][0])) ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
                }
                return response;
            }
        }

        public static string GetSocialShareMessage(long BookingId)
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlParameter[] MyParams = new SqlParameter[3];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iBookingId", BookingId);
                con.Open();
                var ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspGetMessageToShareAfterBooking", MyParams);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var result = ds.Tables[0].Rows[0][0].ToString();
                    return result;
                }
            }

            return string.Empty;
        }

        public static List<etblBookingDetailsTx> GetBookingDetailList(long bookingId)
        {
            try
            {
                List<etblBookingDetailsTx> bookingDetailList = new List<etblBookingDetailsTx>();

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var db_bookingDetailList = db.tblBookingDetailsTxes.Where(x => x.iBookingId == bookingId).ToList();

                    if (db_bookingDetailList != null && db_bookingDetailList.Count > 0)
                    {
                        var bookingResult = db_bookingDetailList.FirstOrDefault().tblBookingTx;

                        foreach (var item in db_bookingDetailList)
                        {
                            var singleRow = (etblBookingDetailsTx)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblBookingDetailsTx());

                            singleRow.dtCheckIn = bookingResult.dtCheckIn;
                            singleRow.dtCheckOut = bookingResult.dtChekOut;
                            bookingDetailList.Add(singleRow);
                        }
                    }

                    return bookingDetailList;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<etblBookingDetailsTx> GetBookingWithBookingDetails(long bookingId, out etblBookingTx booking)
        {
            try
            {
                List<etblBookingDetailsTx> bookingDetailList = new List<etblBookingDetailsTx>();

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var bookingDb = db.tblBookingTxes.Find(bookingId);

                    booking = (etblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(bookingDb, new etblBookingTx());

                    if (booking != null)
                    {
                        var db_bookingDetailList = bookingDb.tblBookingDetailsTxes.Where(x => x.iBookingId == bookingId).ToList();

                        foreach (var item in db_bookingDetailList)
                        {
                            var singleRow = (etblBookingDetailsTx)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblBookingDetailsTx());
                            bookingDetailList.Add(singleRow);
                        }
                    }
                }

                return bookingDetailList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static etblBookingTx GetBooking(long bookingId)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var booking = db.tblBookingTxes.Find(bookingId);
                    if (booking != null)
                    {
                        return (etblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(booking, new etblBookingTx());
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int UpdateBooking(etblBookingTx booking)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblBookingTx obj = (OneFineRate.tblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(booking, new OneFineRate.tblBookingTx());
                    db.tblBookingTxes.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    return db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static int UpdateBookingStatus(etblBookingTx booking, etblBookingTrakerTx objTrck)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblBookingTx obj = (OneFineRate.tblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(booking, new OneFineRate.tblBookingTx());
                    db.tblBookingTxes.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;

                    if (objTrck != null)
                    {
                        OneFineRate.tblBookingTrakerTx tbtrck = (OneFineRate.tblBookingTrakerTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objTrck, new OneFineRate.tblBookingTrakerTx());
                        db.tblBookingTrakerTxes.Add(tbtrck);
                    }
                    db.SaveChanges();

                    return 1;
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    throw;
                }
                catch (Exception ex)
                {
                    return 0;
                    throw;
                }
            }
        }

        public static BalancePaymentModel GetNegotionTransactionDataForBooking(long bookingId)
        {
            try
            {

                BalancePaymentModel model = new BalancePaymentModel();
                using (OneFineRateEntities db = new OneFineRateEntities())
                {


                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@bookingId", bookingId);

                    model = db.Database.SqlQuery<BalancePaymentModel>("uspGetUnfinishedTransactionNegotiationByBookingId @bookingId", MyParam).FirstOrDefault();

                    int propId = model.iPropId;
                    SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                    MyParamHotelFacilities[0] = new SqlParameter("@PropId", propId);
                    model.objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();

                    SqlConnection objConn = default(SqlConnection);
                    DataSet ds = new DataSet();
                    objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());

                    objConn.Open();
                    SqlParameter[] MyParamNew1 = new SqlParameter[1];
                    MyParamNew1[0] = new SqlParameter("@iBookingId", bookingId);
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBookingDetailsById", MyParamNew1);


                    if (ds.Tables.Count > 0)
                    {
                        List<etblBookingDetailsTx> lstDetails = new List<etblBookingDetailsTx>();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                lstDetails.Add(new etblBookingDetailsTx()
                                {
                                    iBookingDetailsID = Convert.ToInt64(ds.Tables[0].Rows[i]["iBookingDetailsID"]),
                                    iBookingId = Convert.ToInt64(ds.Tables[0].Rows[i]["iBookingId"]),
                                    iRoomId = Convert.ToString(ds.Tables[0].Rows[i]["iRoomId"]),
                                    iRPId = Convert.ToString(ds.Tables[0].Rows[i]["iRPId"]),
                                    sRoomName = ds.Tables[0].Rows[i]["sRoomName"].ToString(),
                                    sRPName = ds.Tables[0].Rows[i]["sRPName"].ToString(),
                                    iOccupancy = Convert.ToInt16(ds.Tables[0].Rows[i]["iOccupancy"]),
                                    dRoomRate = ds.Tables[0].Rows[i]["dRoomRate"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["dRoomRate"]) : 0,
                                    dExtraBedRate = ds.Tables[0].Rows[i]["dExtraBedRate"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["dExtraBedRate"]) : 0,
                                    iExtraBeds = Convert.ToInt16(ds.Tables[0].Rows[i]["iExtraBeds"]),
                                    iRooms = Convert.ToInt16(ds.Tables[0].Rows[i]["iRooms"]),
                                    dTaxes = ds.Tables[0].Rows[i]["dTaxes"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["dTaxes"]) : 0,
                                    iAdults = Convert.ToInt16(ds.Tables[0].Rows[i]["iAdults"]),
                                    iChildren = Convert.ToInt16(ds.Tables[0].Rows[i]["iChildren"])
                                });
                            }
                        }

                        List<eBookingPolicies> lstPolicies = new List<eBookingPolicies>();
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                lstPolicies.Add(new eBookingPolicies()
                                {
                                    iBookingDetailsID = Convert.ToInt64(ds.Tables[1].Rows[i]["iBookingDetailsID"]),
                                    sAmenityRatePlan = ds.Tables[1].Rows[i]["sAmenityRatePlan"].ToString(),
                                    sPolicyName = ds.Tables[1].Rows[i]["sPolicyName"].ToString()

                                });
                            }
                        }

                        List<TaxesDateRoomRateWise> lstDayTaxes = new List<TaxesDateRoomRateWise>();
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            for (int tax = 0; tax < ds.Tables[2].Rows.Count; tax++)
                            {
                                var table = ds.Tables[2].Rows[tax];
                                lstDayTaxes.Add(new TaxesDateRoomRateWise()
                                {
                                    dtDate = Convert.ToDateTime(table["dtStayDay"]),
                                    RoomID = Convert.ToInt32(table["iRoomId"]),
                                    iOccupancy = Convert.ToInt32(table["iOccupancy"]),
                                    dBasePrice = Convert.ToDecimal(table["dRoomRate"])
                                });
                            }
                        }
                        model.lstTaxesDateWise_OfferReview = lstDayTaxes;
                        model.BookingDetailList = lstDetails;
                        model.BookingPoliciesList = lstPolicies;

                    }

                    objConn.Close();
                    return model;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static BalancePaymentModel GetCompetitorHotels(long bookingId)
        {
            BalancePaymentModel model = new BalancePaymentModel();
            List<eComplementaryRoomFacilities> lstroomfac = new List<eComplementaryRoomFacilities>();
            List<eComplementaryViews> lstViews = new List<eComplementaryViews>();
            List<eComplementaryFacility> lstfacilities = new List<eComplementaryFacility>();
            List<eComplementaryNegoRoomData> lstRoomDataCurrent = new List<eComplementaryNegoRoomData>();
            List<eComplementaryNegoRoomData> lstRoomData = new List<eComplementaryNegoRoomData>();
            try
            {

                using (OneFineRateEntities db = new OneFineRateEntities())
                {

                    SqlConnection objConn = default(SqlConnection);
                    DataSet ds = new DataSet();
                    objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                    objConn.Open();

                    SqlParameter[] MyParamNew = new SqlParameter[1];
                    MyParamNew[0] = new SqlParameter("@iBookingId", bookingId);
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetRejectionCompetitionRates", MyParamNew);
                    objConn.Close();


                    if (ds.Tables.Count > 0)
                    {

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            lstroomfac.Add(new eComplementaryRoomFacilities()
                            {
                                iHotelId = Convert.ToInt32(ds.Tables[0].Rows[i]["iHotelId"]),
                                Amen = ds.Tables[0].Rows[i]["Amen"].ToString(),
                                Appl = ds.Tables[0].Rows[i]["Appl"].ToString()

                            });
                        }

                        model.lstroomfac = lstroomfac;

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            lstViews.Add(new eComplementaryViews()
                            {
                                iPropId = Convert.ToInt32(ds.Tables[1].Rows[i]["iPropId"]),
                                sView = ds.Tables[1].Rows[i]["sView"].ToString()
                            });
                        }
                        model.lstViews = lstViews;

                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            lstfacilities.Add(new eComplementaryFacility()
                            {
                                iPropId = Convert.ToInt32(ds.Tables[2].Rows[i]["iPropId"]),
                                sFacility = ds.Tables[2].Rows[i]["sFacility"].ToString()
                            });
                        }
                        model.lstfacilities = lstfacilities;

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                            {
                                lstRoomDataCurrent.Add(new eComplementaryNegoRoomData()
                                {
                                    Id = Convert.ToInt32(ds.Tables[3].Rows[i]["Id"]),
                                    iHotelID = Convert.ToInt32(ds.Tables[3].Rows[i]["iHotelID"]),
                                    iRoomId = Convert.ToInt32(ds.Tables[3].Rows[i]["iRoomId"]),
                                    iRPID = Convert.ToInt32(ds.Tables[3].Rows[i]["iRPID"]),
                                    dPrice = ds.Tables[3].Rows[i]["dPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[3].Rows[i]["dPrice"]) : 0,
                                    dTax = ds.Tables[3].Rows[i]["dTax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[3].Rows[i]["dTax"]) : 0,
                                    dDiscPrice = ds.Tables[3].Rows[i]["dDiscPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[3].Rows[i]["dDiscPrice"]) : 0,
                                    sHotelName = ds.Tables[3].Rows[i]["sHotelName"].ToString(),
                                    sArea = ds.Tables[3].Rows[i]["sArea"].ToString(),
                                    iStarCategoryId = Convert.ToInt32(ds.Tables[3].Rows[i]["iStarCategoryId"]),
                                    sRatingImageURL = ds.Tables[3].Rows[i]["sRatingImageURL"].ToString(),
                                    sRankingString = ds.Tables[3].Rows[i]["sRankingString"].ToString(),
                                    sRoomName = ds.Tables[3].Rows[i]["sRoomName"].ToString(),
                                    Size = ds.Tables[3].Rows[i]["Size"].ToString(),
                                    sImgUrl = ds.Tables[3].Rows[i]["sImgUrl"].ToString(),
                                    Symbol = ds.Tables[3].Rows[i]["Symbol"].ToString(),
                                    CommissionRate = ds.Tables[3].Rows[i]["CommissionRate"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[3].Rows[i]["CommissionRate"]) : 0,

                                    dServiceCharge = ds.Tables[3].Rows[i]["ServiceCharge"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[3].Rows[i]["ServiceCharge"]) : 0,
                                    dTaxOnServiceCharge = ds.Tables[3].Rows[i]["TaxOnServiceCharge"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[3].Rows[i]["TaxOnServiceCharge"]) : 0,

                                });
                            }
                        }
                        model.lstRoomDataCurrent = lstRoomDataCurrent;


                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                            {
                                lstRoomData.Add(new eComplementaryNegoRoomData()
                                {
                                    Id = Convert.ToInt32(ds.Tables[4].Rows[i]["Id"]),
                                    iHotelID = Convert.ToInt32(ds.Tables[4].Rows[i]["iHotelID"]),
                                    iRoomId = Convert.ToInt32(ds.Tables[4].Rows[i]["iRoomId"]),
                                    iRPID = Convert.ToInt32(ds.Tables[4].Rows[i]["iRPID"]),
                                    dPrice = ds.Tables[4].Rows[i]["dPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[4].Rows[i]["dPrice"]) : 0,
                                    dTax = ds.Tables[4].Rows[i]["dTax"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[4].Rows[i]["dTax"]) : 0,
                                    dDiscPrice = ds.Tables[4].Rows[i]["dDiscPrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[4].Rows[i]["dDiscPrice"]) : 0,
                                    sHotelName = ds.Tables[4].Rows[i]["sHotelName"].ToString(),
                                    sArea = ds.Tables[4].Rows[i]["sArea"].ToString(),
                                    iStarCategoryId = Convert.ToInt32(ds.Tables[4].Rows[i]["iStarCategoryId"]),
                                    sRatingImageURL = ds.Tables[4].Rows[i]["sRatingImageURL"].ToString(),
                                    sRankingString = ds.Tables[4].Rows[i]["sRankingString"].ToString(),
                                    sRoomName = ds.Tables[4].Rows[i]["sRoomName"].ToString(),
                                    Size = ds.Tables[4].Rows[i]["Size"].ToString(),
                                    sImgUrl = ds.Tables[4].Rows[i]["sImgUrl"].ToString(),
                                    Symbol = ds.Tables[4].Rows[i]["Symbol"].ToString(),
                                    CommissionRate = ds.Tables[4].Rows[i]["CommissionRate"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[4].Rows[i]["CommissionRate"]) : 0,

                                    dServiceCharge = ds.Tables[4].Rows[i]["ServiceCharge"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[4].Rows[i]["ServiceCharge"]) : 0,
                                    dTaxOnServiceCharge = ds.Tables[4].Rows[i]["TaxOnServiceCharge"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[4].Rows[i]["TaxOnServiceCharge"]) : 0,

                                });
                            }
                        }
                        model.lstRoomData = lstRoomData;


                        model.ccType = ds.Tables[5].Rows[0]["ccType"].ToString();
                        model.cBookingStatus = ds.Tables[5].Rows[0]["BookingStatus"].ToString();
                        model.iLinkedBookId = Convert.ToInt32(ds.Tables[5].Rows[0]["BookingId"]);
                        model.iPropId = Convert.ToInt32(ds.Tables[5].Rows[0]["iPropId"]);
                        model.ExchangeRate = Convert.ToDecimal(ds.Tables[5].Rows[0]["ExchangeRate"]);
                    }
                    else
                    {
                        model.lstroomfac = lstroomfac;
                        model.lstViews = lstViews;
                        model.lstfacilities = lstfacilities;
                        model.lstRoomDataCurrent = lstRoomDataCurrent;
                        model.lstRoomData = lstRoomData;

                    }

                }
            }
            catch (Exception ex)
            {
                model.lstroomfac = lstroomfac;
                model.lstViews = lstViews;
                model.lstfacilities = lstfacilities;
                model.lstRoomDataCurrent = lstRoomDataCurrent;
                model.lstRoomData = lstRoomData;
            }
            return model;
        }

        public static BalancePaymentModel GetUnfinishedTransactionToSendRevenueManager(long bookingId)
        {
            try
            {

                BalancePaymentModel model = new BalancePaymentModel();
                using (OneFineRateEntities db = new OneFineRateEntities())
                {


                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@bookingId", bookingId);
                    //MyParam[1] = new SqlParameter("@CurrencyCode", CurrencyCode);
                    model = db.Database.SqlQuery<BalancePaymentModel>("uspGetUnfinishedTransactionNegotiationByBookingId @bookingId", MyParam).FirstOrDefault();

                    int propId = model.iPropId;
                    SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                    MyParamHotelFacilities[0] = new SqlParameter("@PropId", propId);
                    model.objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                    return model;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static BalancePaymentModel GetUnfinishedTransactionNegotiationForHotelPending(long bookingId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {


                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@bookingId", bookingId);

                BalancePaymentModel model = db.Database.SqlQuery<BalancePaymentModel>("uspGetUnfinishedTransactionNegotiationByBookingId @bookingId", MyParam).FirstOrDefault();

                if (model != null)
                {
                    if (model.iCustomerId != default(int) && model.iCustomerId != null)
                    {
                        SqlParameter[] MyParamHotelFacilities = new SqlParameter[2];
                        MyParamHotelFacilities[0] = new SqlParameter("@customerId", model.iCustomerId);
                        MyParamHotelFacilities[1] = new SqlParameter("@bookingid", bookingId);
                        model.GuestBookingInformationList = db.Database.SqlQuery<GuestBookingInformation>("uspGetGuestBookingsByBookingId @customerId,@bookingid", MyParamHotelFacilities).ToList();
                    }
                    model.slargeMapURL = "https://www.google.com/maps/place/" + model.sHotelName + "+-+" + model.sCity.Trim() + "/@" + model.dLatitude + "," + model.dLongitude + ",17z?hl=en-US";
                }

                return model;
            }
        }

        public static NegotiationApplicable CheckIfNegotiationApplicableByBid(long bookingId)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    NegotiationApplicable model = new NegotiationApplicable();

                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@iBookingId", bookingId);
                    model = db.Database.SqlQuery<NegotiationApplicable>("uspCheckIfNegotiationApplicableByBid @iBookingId", MyParam).FirstOrDefault();
                    return model;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<etblHotelFacilities> GetHotelFacilities(int iPropId)
        {
            try
            {
                List<etblHotelFacilities> objresultHotelFacilities = new List<etblHotelFacilities>();
                using (OneFineRateEntities db = new OneFineRateEntities())
                {

                    SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                    MyParamHotelFacilities[0] = new SqlParameter("@PropId", iPropId);
                    objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                    return objresultHotelFacilities;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static GetNewBookingDetails MakeNewBooking(int bookingId, int propid)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    GetNewBookingDetails model = new GetNewBookingDetails();

                    SqlParameter[] MyParam = new SqlParameter[2];
                    MyParam[0] = new SqlParameter("@iBookingId", bookingId);
                    MyParam[1] = new SqlParameter("@iSelectedPropId", propid);
                    model = db.Database.SqlQuery<GetNewBookingDetails>("uspSaveNewBookingFromOldBooking @iBookingId,@iSelectedPropId", MyParam).FirstOrDefault();
                    return model;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet UpdatePropertyRoomInventory(int bookingid, string Type)
        {
            DataSet ds = new DataSet();
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    //SqlParameter[] MyParam = new SqlParameter[3];
                    //MyParam[0] = new SqlParameter("@iBookingId", bookingid);
                    //MyParam[1] = new SqlParameter("@Type", Type);
                    //MyParam[2] = new SqlParameter("@User", 0);
                    //db.Database.ExecuteSqlCommand("uspPropertyRoomInventory @iBookingId,@Type,@User ", MyParam);

                    SqlConnection objConn = default(SqlConnection);

                    objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                    objConn.Open();
                    SqlParameter[] MyParamNew1 = new SqlParameter[3];
                    MyParamNew1[0] = new SqlParameter("@iBookingId", bookingid);
                    MyParamNew1[1] = new SqlParameter("@Type", Type);
                    MyParamNew1[2] = new SqlParameter("@User", 0);
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspPropertyRoomInventory", MyParamNew1);

                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public static BalancePaymentModel GetNewTaxesForCouterOffer(long bookingId)
        {
            try
            {
                BalancePaymentModel model = new BalancePaymentModel();
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@bookingId", bookingId);

                    model = db.Database.SqlQuery<BalancePaymentModel>("uspGetUnfinishedTransactionNegotiationByBookingId @bookingId", MyParam).FirstOrDefault();
                    return model;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Update Day wise booking rates
        public static async Task UpdateDaywiseBookingRate(int bookingid)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@iBookingId", bookingid);
                    await db.Database.ExecuteSqlCommandAsync("uspUpdateDaywiseBookingRate @iBookingId", MyParam);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Update loyality points by booking
        public static async Task UpdateLoyalityPointsForBooking(int bookingid)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@iBookingId", bookingid);
                    await db.Database.ExecuteSqlCommandAsync("uspUpdateLoyalityPointsByBooking @iBookingId", MyParam);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static eBookingShareInfo GetBookingDetailsForSharing(int bookingId)
        {
            eBookingShareInfo model = new eBookingShareInfo();
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {


                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@bookingid", bookingId);
                    model = db.Database.SqlQuery<eBookingShareInfo>("uspGetSharingInformationByBookingId @bookingid", MyParam).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public static string ProvideFinalOffer(int BookingId, int FinalOffer, int User, int Convert)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {

                    SqlConnection objConn = default(SqlConnection);
                    DataSet ds = new DataSet();
                    objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                    objConn.Open();
                    SqlParameter[] MyParamNew1 = new SqlParameter[4];
                    MyParamNew1[0] = new SqlParameter("@iBookingId", BookingId);
                    MyParamNew1[1] = new SqlParameter("@FinalOffer", FinalOffer);
                    MyParamNew1[2] = new SqlParameter("@User", User);
                    MyParamNew1[3] = new SqlParameter("@Convert", Convert);
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspProvideFinalOffer", MyParamNew1);

                    return ds.Tables[0].Rows[0]["Outcome"].ToString();
                }
            }
            catch (Exception)
            {
                return "Please try again later.";
            }
        }

        public static string ReleaseInventory(int BookingId, int User)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {

                    SqlConnection objConn = default(SqlConnection);
                    DataSet ds = new DataSet();
                    objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                    objConn.Open();
                    SqlParameter[] MyParamNew1 = new SqlParameter[3];
                    MyParamNew1[0] = new SqlParameter("@iBookingId", BookingId);
                    MyParamNew1[1] = new SqlParameter("@Type", "IR");
                    MyParamNew1[2] = new SqlParameter("@User", User);
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspPropertyRoomInventory", MyParamNew1);

                    return ds.Tables[0].Rows[0]["Outcome"].ToString();
                }
            }
            catch (Exception)
            {
                return "Please try again later.";
            }
        }

        public static void UpdateDayWiseBookingRate(long bookingId)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@iBookingId", bookingId);
                    db.Database.ExecuteSqlCommandAsync("uspUpdateDaywiseBookingRate @iBookingId", MyParam);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static etblBookingTx GetBookingDetailForFinalBooking(long bookingId)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    etblBookingTx dtoBooking = null;
                    var booking = db.tblBookingTxes.Where(x => x.iBookingId == bookingId).FirstOrDefault();
                    if (booking != null)
                    {
                        dtoBooking = new etblBookingTx();

                        var bookingDetail = booking.tblBookingDetailsTxes.FirstOrDefault();
                        if (bookingDetail != null)
                        {
                            dtoBooking.iBookingId = booking.iBookingId;
                            dtoBooking.dtCheckIn = booking.dtCheckIn;
                            dtoBooking.dtChekOut = booking.dtChekOut;
                            dtoBooking.sCurrencyCode = booking.sCurrencyCode;
                            dtoBooking.sRatePlanCode = bookingDetail.iRPId;
                            dtoBooking.sRoomId = bookingDetail.iRoomId;
                            dtoBooking.iPropId = booking.iPropId;
                            dtoBooking.sProvisionalBookingIdTG = booking.sProvisionalBookingIdTG;
                            dtoBooking.sProvisionalTrxnTypeTG = booking.sProvisionalTrxnTypeTG;
                        }

                        var hotelCode = db.tblPropertyMs.Where(x => x.iPropId == booking.iPropId).Select(x => x.iVendorId).FirstOrDefault();

                        dtoBooking.sVendorId = hotelCode;
                    }

                    return dtoBooking;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static eCancelBookingResponse CancelBooking(long BookingId, decimal timZone, DataTable dtTblBookingDetailIds)
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            eCancelBookingResponse cancel_booking_response = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlParameter[] MyParams = new SqlParameter[3];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iBookingId", BookingId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@BigIds", dtTblBookingDetailIds);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@iCountryOffset", timZone);
                con.Open();
                var ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspCancelBookingOFR", MyParams);
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    cancel_booking_response = new eCancelBookingResponse();
                    cancel_booking_response.Refund = ds.Tables[1].Rows[0]["Refund"] is DBNull ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[0]["Refund"]);
                    cancel_booking_response.CustName = Convert.ToString(ds.Tables[1].Rows[0]["CustName"]);
                    cancel_booking_response.CustEmail = Convert.ToString(ds.Tables[1].Rows[0]["CustEmail"]);
                    cancel_booking_response.CustPhoneNumber = Convert.ToString(ds.Tables[1].Rows[0]["CustPhoneNumber"]);
                    cancel_booking_response.sRefundEmail = Convert.ToString(ds.Tables[1].Rows[0]["sRefundEmail"]);
                    cancel_booking_response.RoomsCancelled = ds.Tables[1].Rows[0]["RoomsCancelled"] is DBNull ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0]["RoomsCancelled"]);
                    cancel_booking_response.CancellationCharges = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["CancelCharges"].ToString()) ? Convert.ToDecimal(ds.Tables[1].Rows[0]["CancelCharges"]) : 0;
                    cancel_booking_response.CancelChargesHotel = !string.IsNullOrEmpty(ds.Tables[1].Rows[0]["CancelChargesHotel"].ToString()) ? Convert.ToDecimal(ds.Tables[1].Rows[0]["CancelChargesHotel"]) : 0;
                    cancel_booking_response.sCountryPhoneCode = Convert.ToString(ds.Tables[1].Rows[0]["sCountryPhoneCode"]);
                    cancel_booking_response.HotelName = Convert.ToString(ds.Tables[1].Rows[0]["HotelName"]);
                    cancel_booking_response.City = Convert.ToString(ds.Tables[1].Rows[0]["City"]);
                    cancel_booking_response.CheckInDate = Convert.ToString(ds.Tables[1].Rows[0]["CheckIn"]);
                    cancel_booking_response.CheckoutDate = Convert.ToString(ds.Tables[1].Rows[0]["CheckOut"]);
                    cancel_booking_response.Status = Convert.ToString(ds.Tables[1].Rows[0]["cStatus"]);
                }
            }

            return cancel_booking_response;
        }

        public class TaxesComparator : IEqualityComparer<etblBookedDayWiseTaxAmountDetails>
        {
            public bool Equals(etblBookedDayWiseTaxAmountDetails x, etblBookedDayWiseTaxAmountDetails y)
            {
                if (x.RoomID == y.RoomID && x.iOccupancy == y.iOccupancy && x.RPID == y.RPID && x.dtStayDay == y.dtStayDay)
                    return true;
                else
                    return false;
            }


            public int GetHashCode(etblBookedDayWiseTaxAmountDetails obj)
            {
                return obj.RoomID.GetHashCode();
            }
        }
        public class SeprateTaxesComparator : IEqualityComparer<etblBookedDayWiseTaxAmountDetailsAll>
        {
            public bool Equals(etblBookedDayWiseTaxAmountDetailsAll x, etblBookedDayWiseTaxAmountDetailsAll y)
            {
                if (x.RoomID == y.RoomID && x.iOccupancy == y.iOccupancy && x.RPID == y.RPID && x.dtStayDay == y.dtStayDay && x.iTaxId == y.iTaxId)
                    return true;
                else
                    return false;
            }


            public int GetHashCode(etblBookedDayWiseTaxAmountDetailsAll obj)
            {
                return obj.RoomID.GetHashCode();
            }
        }

        public class CancellationComparator : IEqualityComparer<etblBookingCancellationPolicyMap>
        {
            public bool Equals(etblBookingCancellationPolicyMap x, etblBookingCancellationPolicyMap y)
            {
                if (x.dtDate == y.dtDate && x.sPolicyName == y.sPolicyName && x.iRPId == y.iRPId)
                    return true;
                else
                    return false;
            }


            public int GetHashCode(etblBookingCancellationPolicyMap obj)
            {
                return obj.dtDate.GetHashCode();
            }
        }

        public static long GetBookingIdByLinkedId(long id)
        {
            long BookingId = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblBookingTxes.Select(u => new { u.iBookingId, u.iLinkedBookingId }).SingleOrDefault(u => u.iLinkedBookingId == id);
                if (dbobj != null)
                {
                    BookingId = dbobj.iBookingId;
                }
            }
            return BookingId;

        }

        public class BookingDetailsComparator : IEqualityComparer<etblBookingDetailsTx>
        {
            public bool Equals(etblBookingDetailsTx x, etblBookingDetailsTx y)
            {
                if (x.iBookingDetailsID == y.iBookingDetailsID)
                    return true;
                else
                    return false;
            }


            public int GetHashCode(etblBookingDetailsTx obj)
            {
                return obj.iBookingDetailsID.GetHashCode();
            }
        }

        public static BookingAmedCancelation GetAmendCacellationCharges(long BookingId, DataTable dt, decimal CountryOffset, DateTime checkin, DateTime checkout, Boolean IsModifyDate)
        {

            BookingAmedCancelation eobj = new BookingAmedCancelation();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                #region Cancellation Charges Calculate
                SqlConnection objConn = default(SqlConnection);
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[6];

                MyParams[0] = new System.Data.SqlClient.SqlParameter("@iBookingId", BookingId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@BookingDetailsAmend", dt);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@iCountryOffset", CountryOffset);
                MyParams[3] = new System.Data.SqlClient.SqlParameter("@dtCheckInNew", checkin);
                MyParams[4] = new System.Data.SqlClient.SqlParameter("@dtCheckOutNew", checkout);
                MyParams[5] = new System.Data.SqlClient.SqlParameter("@bIsModifyDate", IsModifyDate);
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspCalculateCancellationChargesAmend", MyParams);
                objConn.Close();

                if (ds.Tables.Count > 0)
                {
                    List<eBookingAmendCancelBookingRoomIdWiseCharges> lstBookingRoomCharges = new List<eBookingAmendCancelBookingRoomIdWiseCharges>();
                    List<eBookingAmendCancelRoomCharges> lstRoomCharges = new List<eBookingAmendCancelRoomCharges>();


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        lstBookingRoomCharges.Add(new eBookingAmendCancelBookingRoomIdWiseCharges()
                        {

                            iBookingDetailsID = Convert.ToInt64(ds.Tables[0].Rows[i]["iBookingDetailsID"].ToString()),
                            CanCharges = Convert.ToDecimal(ds.Tables[0].Rows[i]["CanCharges"].ToString())
                        });
                    }

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        lstRoomCharges.Add(new eBookingAmendCancelRoomCharges()
                        {

                            sRoomName = Convert.ToString(ds.Tables[1].Rows[i]["sRoomName"].ToString()),
                            CanCharges = Convert.ToDecimal(ds.Tables[1].Rows[i]["CanCharges"].ToString())
                        });
                    }

                    eobj.lstRoomCharges = lstRoomCharges;
                    eobj.lstBookingRoomCharges = lstBookingRoomCharges;

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        eobj.TotalCanellationCharges = ds.Tables[2].Rows[0]["TotalCanellationCharges"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[2].Rows[0]["TotalCanellationCharges"]) : 0;
                    }

                    #endregion
                }

                return eobj;
            }
        }

        public static int Amendments_UpdateBooking(etblBookingTx obj, etblBookingAmedTx objOldBooking, List<etblBookingDetailsTx> lstBookDetails, List<etblBookingCancellationPolicyMap> lstCancelPolicy, List<etblBookedDayWiseTaxAmountDetails> lstDayTaxes, List<int> lstOldBookings, List<eBookingAmendCancelBookingRoomIdWiseCharges> lstRoomCancelCharges, List<etblBookedDayWiseTaxAmountDetailsAll> lstDayTaxesAll = null)
        {

            int retval = 0;
            List<tblBookingGuestMap> guestMaps = new List<tblBookingGuestMap>();
            List<tblBookingDetailsTx> bookingdetails = new List<tblBookingDetailsTx>();
            int? stateid = null;

            using (var transationscope = new TransactionScope())
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    try
                    {
                        var propertyDetails = db.tblPropertyMs.Where(x => x.iPropId == obj.iPropId).SingleOrDefault();
                        var propertyStateId = 0;
                        if (propertyDetails != null)
                        {
                            propertyStateId = propertyDetails.iStateId;
                        }

                        OneFineRate.tblBookingTx objNewBook = (OneFineRate.tblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(obj, new OneFineRate.tblBookingTx());
                        db.tblBookingTxes.Attach(objNewBook);
                        db.Entry(objNewBook).State = System.Data.Entity.EntityState.Modified;
                        if (objNewBook.iCustomerId != null && objNewBook.iCustomerId != 0)
                        {
                            stateid = db.tblWebsiteUserMaters.Where(x => x.Id == objNewBook.iCustomerId).Select(x => x.StateId).SingleOrDefault();
                        }
                        else
                        {
                            stateid = db.tblWebsiteGuestUserMasters.Where(x => x.Id == objNewBook.iCustomerId).Select(x => x.iStateId).SingleOrDefault();
                        }

                        // TO DO
                        objOldBooking.BookingStatus = objNewBook.BookingStatus;

                        long AmendId = db.tblBookingAmedTxes.OrderByDescending(u => u.iAmendId).FirstOrDefault().iAmendId + 1;

                        objOldBooking.iAmendId = AmendId;

                        OneFineRate.tblBookingAmedTx dbBooking = (OneFineRate.tblBookingAmedTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objOldBooking, new OneFineRate.tblBookingAmedTx());
                        db.tblBookingAmedTxes.Add(dbBooking);

                        var RecentSavings = db.tblRecentSavingsTxes.SingleOrDefault(u => u.iBookingId == obj.iBookingId);

                        if (RecentSavings != null)
                        {
                            db.tblRecentSavingsTxes.Attach(RecentSavings);
                            db.Entry(RecentSavings).State = System.Data.Entity.EntityState.Deleted;
                        }

                        var PastSavings = db.tblPastSavings.SingleOrDefault(u => u.iBookingId == obj.iBookingId);

                        if (PastSavings != null)
                        {
                            db.tblPastSavings.Attach(PastSavings);
                            db.Entry(PastSavings).State = System.Data.Entity.EntityState.Deleted;
                        }

                        foreach (var OldBookDetailsId in lstOldBookings)
                        {

                            var bookingDetailsTx = db.tblBookingDetailsTxes.SingleOrDefault(u => u.iBookingDetailsID == OldBookDetailsId);
                            if (bookingDetailsTx != null)
                            {
                                bookingdetails.Add(bookingDetailsTx);

                                var lstGuestDetails = db.tblBookingGuestMaps.Where(u => u.iBookingDetailsID == OldBookDetailsId).AsQueryable().ToList();

                                if (lstGuestDetails != null)
                                {
                                    foreach (var GuestDetails in lstGuestDetails)
                                    {
                                        etblBookingGuestAmendMap objGuest = new etblBookingGuestAmendMap();
                                        etblBookingGuestMap objGuestMap = new etblBookingGuestMap();
                                        objGuest = (etblBookingGuestAmendMap)OneFineRateAppUtil.clsUtils.ConvertToObject(GuestDetails, objGuest);
                                        objGuestMap = (etblBookingGuestMap)OneFineRateAppUtil.clsUtils.ConvertToObject(GuestDetails, objGuestMap);
                                        objGuest.iAmendId = AmendId;
                                        objGuestMap.iBookingGuestId = 0;

                                        OneFineRate.tblBookingGuestAmendMap dbGuestDetails = (OneFineRate.tblBookingGuestAmendMap)OneFineRateAppUtil.clsUtils.ConvertToObject(objGuest, new OneFineRate.tblBookingGuestAmendMap());
                                        OneFineRate.tblBookingGuestMap dbGuestMapDetails = (OneFineRate.tblBookingGuestMap)OneFineRateAppUtil.clsUtils.ConvertToObject(objGuestMap, new OneFineRate.tblBookingGuestMap());
                                        db.tblBookingGuestAmendMaps.Add(dbGuestDetails);
                                        // db.SaveChanges();
                                        guestMaps.Add(dbGuestMapDetails);
                                        db.tblBookingGuestMaps.Attach(GuestDetails);
                                        db.Entry(GuestDetails).State = System.Data.Entity.EntityState.Deleted;
                                    }

                                }

                                var DayWiseRate = db.tblDaywiseBookingRateTxes.Where(x => x.iBookingDetailsID == OldBookDetailsId).ToList();
                                if (DayWiseRate != null)
                                {
                                    db.tblDaywiseBookingRateTxes.RemoveRange(DayWiseRate);
                                }



                                etblBookingDetailsAmendTx objAmendDetails = new etblBookingDetailsAmendTx();
                                objAmendDetails = (etblBookingDetailsAmendTx)OneFineRateAppUtil.clsUtils.ConvertToObject(bookingDetailsTx, objAmendDetails);
                                objAmendDetails.iAmendId = AmendId;

                                if (lstRoomCancelCharges != null)
                                {
                                    decimal Charges = 0;
                                    var objRoomCharges = lstRoomCancelCharges.Where(u => u.iBookingDetailsID == OldBookDetailsId).SingleOrDefault();
                                    if (objRoomCharges != null)
                                    {
                                        Charges = Convert.ToDecimal(objRoomCharges.CanCharges);
                                    }
                                    objAmendDetails.dCancellationCharges = Charges;
                                }

                                OneFineRate.tblBookingDetailsAmendTx dbDetails = (OneFineRate.tblBookingDetailsAmendTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objAmendDetails, new OneFineRate.tblBookingDetailsAmendTx());
                                db.tblBookingDetailsAmendTxes.Add(dbDetails);
                                // db.SaveChanges();

                                db.tblBookingDetailsTxes.Attach(bookingDetailsTx);
                                db.Entry(bookingDetailsTx).State = System.Data.Entity.EntityState.Deleted;
                                //db.SaveChanges();
                            }
                            //db.tblBookingCancellationPolicyMaps.RemoveRange(item);



                            var CancellationTx = db.tblBookingCancellationPolicyMaps.Where(x => x.iBookingDetailsID == OldBookDetailsId).ToList();
                            if (CancellationTx != null)
                            {

                                foreach (var item in CancellationTx)
                                {
                                    etblBookingCancellationPolicyAmendMap objCancelAmendDetails = new etblBookingCancellationPolicyAmendMap();
                                    objCancelAmendDetails = (etblBookingCancellationPolicyAmendMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, objCancelAmendDetails);
                                    objCancelAmendDetails.iAmendId = AmendId;

                                    OneFineRate.tblBookingCancellationPolicyAmendMap dbCancelDetails = (OneFineRate.tblBookingCancellationPolicyAmendMap)OneFineRateAppUtil.clsUtils.ConvertToObject(objCancelAmendDetails, new OneFineRate.tblBookingCancellationPolicyAmendMap());
                                    db.tblBookingCancellationPolicyAmendMaps.Add(dbCancelDetails);
                                    // db.SaveChanges();

                                    db.tblBookingCancellationPolicyMaps.Attach(item);
                                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;

                                }

                            }

                            //remove day wise taxes 
                            var lt = db.tblBookedDayWiseTaxAmountDetails.Where(x => x.iBookingDetailsID == OldBookDetailsId).ToList();
                            db.tblBookedDayWiseTaxAmountDetails.RemoveRange(lt);
                            var ltAll = db.tblBookedDayWiseSeparateTaxAmountDetails.Where(x => x.iBookingDetailsID == OldBookDetailsId).ToList();
                            db.tblBookedDayWiseSeparateTaxAmountDetails.RemoveRange(ltAll);



                        }
                        //OneFineRate.tblBookingDetailsTx dbBookDetails = (OneFineRate.tblBookingDetailsTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objBookDetails, new OneFineRate.tblBookingDetailsTx());


                        //OneFineRate.tblBookingTx bdbooking = (OneFineRate.tblBookingTx)OneFineRateAppUtil.clsUtils.ConvertToObject(obj.objBooking, new OneFineRate.tblBookingTx());
                        //db.tblBookingTxes.Add(bdbooking);
                        //db.SaveChanges();


                        var booking = 0;
                        foreach (var objBookDetails in lstBookDetails)
                        {
                            objBookDetails.iBookingId = obj.iBookingId;
                            OneFineRate.tblBookingDetailsTx dbBookDetails = (OneFineRate.tblBookingDetailsTx)OneFineRateAppUtil.clsUtils.ConvertToObject(objBookDetails, new OneFineRate.tblBookingDetailsTx());
                            db.tblBookingDetailsTxes.Add(dbBookDetails);
                            db.Database.Log = Console.Write;
                            db.SaveChanges();
                            var newBookingId = dbBookDetails.iBookingDetailsID;
                            booking++;
                            for (int ib = 0; ib < booking; ib++)
                            {
                                foreach (var item in guestMaps.Where(bookingid => bookingid.iBookingDetailsID == bookingdetails[ib].iBookingDetailsID).ToList())
                                {
                                    item.iBookingDetailsID = newBookingId;
                                }
                            }
                            objBookDetails.iBookingDetailsID = dbBookDetails.iBookingDetailsID;


                            var dlist = lstDayTaxes.Distinct(new TaxesComparator()).ToList();

                            foreach (var objTaxes in dlist.Where(u => u.RoomID.ToString() == objBookDetails.iRoomId && u.RPID.ToString() == objBookDetails.iRPId && u.iOccupancy == objBookDetails.iOccupancy).AsQueryable())
                            {
                                objTaxes.iBookingDetailsID = Convert.ToInt32(dbBookDetails.iBookingDetailsID);
                                objTaxes.iBookingID = Convert.ToInt32(obj.iBookingId);
                                OneFineRate.tblBookedDayWiseTaxAmountDetail dbDayTaxes = (OneFineRate.tblBookedDayWiseTaxAmountDetail)OneFineRateAppUtil.clsUtils.ConvertToObject(objTaxes, new OneFineRate.tblBookedDayWiseTaxAmountDetail());
                                db.tblBookedDayWiseTaxAmountDetails.Add(dbDayTaxes);
                            }

                            var dlistAll = lstDayTaxesAll.Distinct(new SeprateTaxesComparator()).ToList();

                            foreach (var objTaxes in dlistAll.Where(u => u.RoomID.ToString() == objBookDetails.iRoomId && u.RPID.ToString() == objBookDetails.iRPId && u.iOccupancy == objBookDetails.iOccupancy).AsQueryable())
                            {
                                if (propertyStateId == stateid && objTaxes.sTaxName != "IGST")
                                {
                                    objTaxes.iBookingDetailsID = Convert.ToInt32(dbBookDetails.iBookingDetailsID);
                                    objTaxes.iBookingID = Convert.ToInt32(obj.iBookingId);
                                    OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail dbDayTaxes = (OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail)OneFineRateAppUtil.clsUtils.ConvertToObject(objTaxes, new OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail());
                                    db.tblBookedDayWiseSeparateTaxAmountDetails.Add(dbDayTaxes);
                                }
                                else if (propertyStateId != stateid && objTaxes.sTaxName != "CGST" || propertyStateId != stateid && objTaxes.sTaxName != "SGST")
                                {
                                    objTaxes.iBookingDetailsID = Convert.ToInt32(dbBookDetails.iBookingDetailsID);
                                    objTaxes.iBookingID = Convert.ToInt32(obj.iBookingId);
                                    OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail dbDayTaxes = (OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail)OneFineRateAppUtil.clsUtils.ConvertToObject(objTaxes, new OneFineRate.tblBookedDayWiseSeparateTaxAmountDetail());
                                    db.tblBookedDayWiseSeparateTaxAmountDetails.Add(dbDayTaxes);
                                }
                            }
                            var dlist_Cancel = lstCancelPolicy.Distinct(new CancellationComparator()).ToList();

                            foreach (var objCancel in dlist_Cancel.Where(u => u.iRPId.ToString() == objBookDetails.iRPId).AsQueryable())
                            {
                                objCancel.iBookingDetailsID = objBookDetails.iBookingDetailsID;
                                OneFineRate.tblBookingCancellationPolicyMap dbCancelPolicy = (OneFineRate.tblBookingCancellationPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(objCancel, new OneFineRate.tblBookingCancellationPolicyMap());
                                db.tblBookingCancellationPolicyMaps.Add(dbCancelPolicy);
                            }

                        }
                        db.tblBookingGuestMaps.AddRange(guestMaps);
                        var i = db.SaveChanges();
                        retval = 1;
                        transationscope.Complete();

                    }
                    catch (Exception ex)
                    {
                        retval = 0;
                        throw;
                    }
                    finally
                    {
                        transationscope.Dispose();
                    }

                }
            }
            return retval;
        }

        //Add new record
        public static int AddRecord_OriginalBookingPrice(etblOriginalBookingPrice eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    OneFineRate.tblOriginalBookingPrice dbuser = (OneFineRate.tblOriginalBookingPrice)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblOriginalBookingPrice());
                    db.tblOriginalBookingPrices.Add(dbuser);
                    db.SaveChanges();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }


        public static List<eTaxDifferenceBookings> GetTaxAffectedBookings(jQueryDataTableParamModel param, int bookingId, string propIds, string cityIds, out int TotalCount)
        {
            var result = new List<eTaxDifferenceBookings>();

            //int displayStart = param.iDisplayStart == 0 ? 1 : param.iDisplayStart + 1;

            SqlParameter[] MyParam = new SqlParameter[10];
            MyParam[1] = new SqlParameter("@HotelIds", propIds);
            MyParam[2] = new SqlParameter("@CityIds", cityIds);
            MyParam[3] = new SqlParameter("@iBookingID", bookingId);
            MyParam[4] = new SqlParameter("@DisplayStart", param.iDisplayStart);
            MyParam[5] = new SqlParameter("@DisplayLength", param.iDisplayLength);
            MyParam[6] = new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D");
            MyParam[7] = new SqlParameter("@SortBy", param.iSortingCols);
            MyParam[8] = new SqlParameter("@TotalCount", 0);
            MyParam[8].Direction = System.Data.ParameterDirection.Output;
            TotalCount = 0;
            DataTable dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetBookingsForTaxChangeForEmail", MyParam).Tables[0];

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new eTaxDifferenceBookings();
                    model.iBookingId = Convert.ToInt32(dt.Rows[i]["iBookingId"]);
                    model.sHotelName = Convert.ToString(dt.Rows[i]["sHotelName"]);
                    model.sCity = Convert.ToString(dt.Rows[i]["sCity"]);
                    model.dtCheckIn = Convert.ToDateTime(dt.Rows[i]["dtCheckIn"]);
                    model.dtCheckOut = Convert.ToDateTime(dt.Rows[i]["dtChekOut"]);
                    model.Diff = Convert.ToDecimal(dt.Rows[i]["Diff"]);
                    model.sEmailOFR = Convert.ToString(dt.Rows[i]["sEmailOFR"]);
                    result.Add(model);
                }

                TotalCount = Convert.ToInt16(MyParam[8].Value);
            }

            return result;
        }

        public static bool UpdateSyncStatusChannelMgr(long bookingId,bool? status)
        {
            try
            {
                using (var db = new OneFineRateEntities())
                {
                    var data = (from t in db.tblBookingTxes where t.iBookingId == bookingId select t).SingleOrDefault();
                    data.bSyncToChannelMgr = status;
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        public static List<eTaxDifferenceBookings> GetTaxAffectedBookings(long[] bookingIds)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var result = (from b in db.tblBookingTxes
                              join bd in db.tblBookedDayWiseTaxAmountDetails_Mod on b.iBookingId equals bd.iBookingID
                              join p in db.tblPropertyMs on b.iPropId equals p.iPropId
                              join c in db.tblCityMs on p.iCityId equals c.iCityId
                              join u in db.tblWebsiteUserMaters on b.iCustomerId equals u.Id into userResult
                              from udefault in userResult.DefaultIfEmpty()
                              join g in db.tblWebsiteGuestUserMasters on b.iGuestId equals g.Id into guestUserResult
                              from defultguestUser in guestUserResult.DefaultIfEmpty()
                              where bookingIds.Contains(b.iBookingId)
                              select new eTaxDifferenceBookings
                              {
                                  iBookingId = b.iBookingId,
                                  dtCheckIn = b.dtCheckIn.Value,
                                  dtCheckOut = b.dtChekOut.Value,
                                  sHotelName = p.sHotelName,
                                  sEmailOFR = b.sEmailOFR,
                                  sCity = c.sCity,
                                  Diff = bd.dTax_New.Value - bd.dTax_Old.Value,
                                  CustomerName = !string.IsNullOrEmpty(udefault.FirstName) ?
                                  udefault.FirstName + " " + udefault.LastName :
                                  defultguestUser.FirstName + " " + defultguestUser.LastName

                              }).Distinct().ToList();

                return result;
            }
        }

        public static int UpdateTaxAffectedBookings(string[] bookingIds, int iActionBy)
        {
            DataTable dtBookingIds = new DataTable("Ids");
            dtBookingIds.Columns.AddRange(new DataColumn[1]
            {
                new DataColumn("ID", typeof(int))
            });

            for (int i = 0; i < bookingIds.Count(); i++)
            {
                DataRow dr = dtBookingIds.NewRow();
                dr["ID"] = bookingIds[i];
                dtBookingIds.Rows.Add(dr);
            }

            SqlParameter[] MyParam = new SqlParameter[2];
            MyParam[0] = new SqlParameter("@Ids", dtBookingIds);
            MyParam[1] = new SqlParameter("@iActionBy", iActionBy);

            var status = SqlHelper.ExecuteNonQuery(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdateEmailStatusForTaxChange", MyParam);

            return status;
        }
    }
}