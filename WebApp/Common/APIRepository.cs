using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApp.Common
{
    public class APIRepository
    {
        private static volatile APIRepository _instance = new APIRepository();

        public static APIRepository Instance
        {
            get
            {
                return _instance;
            }
        }

        public HttpClient Client { get; set; }
        public APIRepository()
        {
            string token = Convert.ToString(ConfigurationManager.AppSettings["APITokenWebAppAccess"]);
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConfigurationManager.AppSettings["APIBaseUrl"].ToString());
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", token);
        }

        public HttpResponseMessage GetResponse(string url)
        {
            return Client.GetAsync(url).Result;
        }
        public HttpResponseMessage PutResponse(string url, object model)
        {
            return Client.PutAsJsonAsync(url, model).Result;
        }
        public HttpResponseMessage PostResponse(string url, object model)
        {
            return Client.PostAsJsonAsync(url, model).Result;
        }
        public HttpResponseMessage DeleteResponse(string url)
        {
            return Client.DeleteAsync(url).Result;
        }
    }
}