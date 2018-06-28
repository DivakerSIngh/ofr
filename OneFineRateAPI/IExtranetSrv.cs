using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Services;
using System.Web.Script.Services;

namespace OneFineRateAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtranetSrv" in both code and config file together.
    [ServiceContract]
    public interface IExtranetSrv
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetAllRecords1", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //[WebInvoke(Method = "GET")]
        string GetAllRecords1(int sEcho, int iDisplayLength, int iDisplayStart, string sSortDir_0, string sSearch, int iSortCol_0);
    }
}
