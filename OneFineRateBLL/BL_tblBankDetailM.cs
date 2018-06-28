using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Data.Entity.Validation;

namespace OneFineRateBLL
{
    public class BL_tblBankDetailM
    {
        //Get Single Record
        public static etblBankDetail GetSingleRecordById(int id)
        {
            etblBankDetail eobj = new etblBankDetail();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblBankDetailMs.SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = (etblBankDetail)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
                else
                    eobj.iPropId = id;
            }
            return eobj;

        }


        //Add new record
        public static int AddOrUpdateRecord(etblBankDetail eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var existingRecord = db.tblBankDetailMs.Find(eobj.iPropId);

                    if (existingRecord != null)
                    {
                        existingRecord.dCommission = eobj.dCommission;
                        existingRecord.dtActionDate = eobj.dtActionDate;
                        existingRecord.dtModifyDate = eobj.dtModifyDate;
                        existingRecord.iActionBy = eobj.iActionBy; ;
                        existingRecord.iModifyBy = eobj.iModifyBy;
                        existingRecord.sBaneficiariesName = eobj.sBaneficiariesName;
                        existingRecord.sBankAccountNo = eobj.sBankAccountNo;
                        existingRecord.sBankName = eobj.sBankName;
                        existingRecord.sBranchAddress = eobj.sBranchAddress;
                        existingRecord.sBranchName = eobj.sBranchName;
                        existingRecord.sFEmailId = eobj.sFEmailId;
                        existingRecord.sFFaxNo = eobj.sFFaxNo;
                        existingRecord.sFName = eobj.sFName;
                        existingRecord.sFPhoneNo = eobj.sFPhoneNo;
                        existingRecord.sIfscCode = eobj.sIfscCode;
                        existingRecord.sMicrCode = eobj.sMicrCode;

                        existingRecord.sRegisteredEntityName = eobj.sRegisteredEntityName;
                        existingRecord.sGstinNumber = eobj.sGstinNumber;
                        existingRecord.iStateId = eobj.iStateId;
                        existingRecord.sPanNumber = eobj.sPanNumber;
                        existingRecord.sEntityType = eobj.sEntityType;

                        //if (string.IsNullOrEmpty(existingRecord.sCancelledCheque) && !string.IsNullOrEmpty(eobj.sCancelledCheque))
                        //{
                        //    existingRecord.sCancelledCheque = eobj.sCancelledCheque;
                        //}
                        //if (string.IsNullOrEmpty(existingRecord.sLetterhead_BankAccount) && !string.IsNullOrEmpty(eobj.sLetterhead_BankAccount))
                        //{
                        //    existingRecord.sLetterhead_BankAccount = eobj.sLetterhead_BankAccount;
                        //}
                        //if (string.IsNullOrEmpty(existingRecord.sPanCard) && !string.IsNullOrEmpty(eobj.sPanCard))
                        //{
                        //    existingRecord.sPanCard = eobj.sPanCard;
                        //}


                        if (!string.IsNullOrEmpty(eobj.sCancelledCheque))
                        {
                            existingRecord.sCancelledCheque = eobj.sCancelledCheque;
                        }
                        if (!string.IsNullOrEmpty(eobj.sLetterhead_BankAccount))
                        {
                            existingRecord.sLetterhead_BankAccount = eobj.sLetterhead_BankAccount;
                        }
                        if (!string.IsNullOrEmpty(eobj.sPanCard))
                        {
                            existingRecord.sPanCard = eobj.sPanCard;
                        }

                        db.Entry(existingRecord).State = System.Data.Entity.EntityState.Detached;
                        db.Entry(existingRecord).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        retval = 2;
                    }
                    else
                    {
                        retval = AddRecord(eobj);
                    }

                }
                catch (DbEntityValidationException e)
                {
                    string validationMessage = string.Empty;

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            validationMessage += ve.ErrorMessage;
                        }
                    }
                    throw new DbEntityValidationException(validationMessage);
                }
            }
            return retval;
        }


        //Add new record
        public static int AddRecord(etblBankDetail eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblBankDetailM dbuser = (OneFineRate.tblBankDetailM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblBankDetailM());
                    db.tblBankDetailMs.Add(dbuser);
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
        //Update a record
        public static int UpdateRecord(etblPropertyM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblBankDetailM obj = (OneFineRate.tblBankDetailM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblBankDetailM());
                    db.tblBankDetailMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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


        // Get fixed information for OFR Finace Details As Temperory Method 
        // Need to put this action method in there own Business Logic Class
        public static etblSettingM GetFixedOFRFinanceDetails()
        {
            etblSettingM eobj = new etblSettingM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblSettingMs.SingleOrDefault();
                if (dbobj != null)
                    eobj = (etblSettingM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;
        }

    }
}