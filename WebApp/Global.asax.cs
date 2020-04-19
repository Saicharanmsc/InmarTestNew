using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Helpers;
using System.Net;
using System.Web.Security;
using System.Web.Script.Serialization;
using WebApp.Common;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }


        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return;
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var serializer = new JavaScriptSerializer();

            if (authTicket != null)
            {
                var serializeUserData = serializer.Deserialize<CustomUserData>(authTicket.UserData);

                var objUserData = new CustomPrincipal(authTicket.Name)
                {
                    UserID = serializeUserData.UserID,
                    DisplayName = serializeUserData.DisplayName,
                    UserName = serializeUserData.UserName,
                    
                };

                HttpContext.Current.User = objUserData;
            }
        }

    }
}
