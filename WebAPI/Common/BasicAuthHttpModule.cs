using InmarTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace WebAPI.Common
{
    public class BasicAuthHttpModule : IHttpModule
    {
        private const string Realm = "WebAPI";
        static Logger olog = new Logger(Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        private void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
            }
        }
        private void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            Logger olog = new Logger(Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));
            var authHeader = request.Headers["Authorization"];

            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);
                olog.Error("authHeader" + authHeaderVal);

                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        private static bool AuthenticateUser(string Credentials)
        {

            var CredentialsArray = Credentials.Split(':');
            var UserValue = CredentialsArray[0];
            var EncodedValue = CredentialsArray[1];

            var DecodeUserValue = RSAClass.Decrypt(EncodedValue);

            if (DecodeUserValue != UserValue)
            {
                olog.Error("Decode Value: " + DecodeUserValue);
                olog.Error("UserValue Value: " + UserValue);

                return false;
            }

            var identity = new GenericIdentity(UserValue);
            SetPrincipal(new GenericPrincipal(identity, null));

            return true;
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        public void Dispose()
        {
        }

    }
}