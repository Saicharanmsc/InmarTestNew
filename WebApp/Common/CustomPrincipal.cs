using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebApp.Common
{
    interface ICustomPrincipal : IPrincipal
    {
        int UserID { get; set; }
        string DisplayName { get; set; }
        string UserName { get; set; }
    }

    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }

        public CustomPrincipal(string UserName)
        {
            this.Identity = new GenericIdentity(UserName);
        }

        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomUserData
    {
        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
    }
}