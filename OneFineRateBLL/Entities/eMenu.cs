using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eMenu
    {
        public int iMenuId { get; set; }
        public int iParentId { get; set; }
        public string sMenuName { get; set; }
        public string sPath { get; set; }
        public short iDispSeq { get; set; }
    }
    public class ControllerAction
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }        
    }
    public class clsValid
    {
        public int Valid { get; set; }        
    }
}