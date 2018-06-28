using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace OneFineRateWeb.Models
{
    [HubName("userActivityHub")]
    public class UserActivityHub : Hub
    {

        private static List<string> Users = new List<string>();

        public void Send(int count)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<UserActivityHub>();
            context.Clients.All.updateUsersOnlineCount(count);
        }

        public override Task OnConnected()
        {
            string token = GetBrowserUniqueRequestVerificationToken();
            Users.Add(token);
            Send(Users.Distinct().Count());
            return base.OnConnected();
        }


        public override Task OnReconnected()
        {
            string token = GetBrowserUniqueRequestVerificationToken();
            Users.Add(token);
            Send(Users.Distinct().Count());
            return base.OnReconnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            string token = GetBrowserUniqueRequestVerificationToken();
            if (Users.IndexOf(token) > -1)
            {
                Users.Remove(token);
            }
            Send(Users.Distinct().Count());
            return base.OnDisconnected(stopCalled);
        }


        /// <summary>
        /// Get's the currently connected browser request verification code.
        /// This is unique for each browser and is used to identify a browser connection.
        /// </summary>
        /// <returns>The request verification code.</returns>
        private string GetBrowserUniqueRequestVerificationToken()
        {
            string requestVerificationToken = "";
            if (Context.Request.Cookies["__RequestVerificationToken"] != null)
            {
                requestVerificationToken = Context.Request.Cookies["__RequestVerificationToken"].Value;
            }
            return requestVerificationToken;
        }



        /// <summary>
        /// Get's the currently connected Id of the client.
        /// This is unique for each client and is used to identify a connection.
        /// </summary>
        /// <returns>The client Id.</returns>
        private string GetClientId()
        {
            string clientId = "";
            if (Context.QueryString["clientId"] != null)
            {
                clientId = this.Context.QueryString["clientId"];
            }

            if (string.IsNullOrEmpty(clientId.Trim()))
            {
                clientId = Context.ConnectionId;
            }
            return clientId;
        }
    }
}