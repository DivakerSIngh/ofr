using OneFineRate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;

namespace OneFineRateBLL.Entities
{
    public class etblMasterTaxMap
    {
        [Required(ErrorMessage = "Please select Rate Range.")]
        public int iRangeId { get; set; }

        public int iId { get; set; }
        public int iMasterTaxId { get; set; }        

        public DateTime? dtStayFrom { get; set; }
        public DateTime? dtStayTo { get; set; }

        [Required(ErrorMessage = "Required")]
        public string sStayFrom { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string sStayTo { get; set; }

        public string cStatus { get; set; }
        public DateTime? dtActionDate { get; set; }
        public int? iActionBy { get; set; }


        [Required(ErrorMessage = "Please select at least one state.")]
        public int[] iStateIds { get; set; }

        // Used to load record from database using LINQ
        public List<int> iStateIdList { get; set; }
        public int iStateId { get; set; }
        public List<eTax> ListTaxes { get; set; }

        public List<eRoomRateRange> ListRoomRateRange { get; set; }
        public bool IsAllStates { get; set; }
    }

    public class eRoomRateRange
    {
        public int iRangeId { get; set; }
        public string sRangeValue { get; set; }
    }


    public class etblMasterTaxMapForDatatable
    {
        public int iMasterTaxId { get; set; }

        public string sAmountFrom { get; set; }

        public string sAmountTo { get; set; }

        public string sTax { get; set; }

        public string sStayFrom { get; set; }

        public string sStayTo { get; set; }

        public string cStatus { get; set; }

        public string sActionDate { get; set; }

        public string sActionBy { get; set; }

        public string sState { get; set; }

        public int iRangeId { get; set; }

        public bool IsAllState { get; set; }
        public string sTaxName { get; set; }
    }
}