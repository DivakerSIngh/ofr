using OneFineRateAPI.Models;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.WebUserBL;
using OneFineRateBLL.WebUserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OneFineRateAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/WebsiteUser")]
    public class WebsiteUserController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
