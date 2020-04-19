using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using BusinessObjects.User;
using InmarTest.DataAccess;
using WebApp.Common;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());
        // GET: Home
        [AllowAnonymous]
        public ActionResult Login()
        {
            User objUser = new User();

            return View(objUser);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(User objUser)
        {
            try
            {
                User checkUser = UserFactory.Instance.GetUserData(objUser.UserName);

                if (checkUser != null)
                {
                    if (checkUser.Password == objUser.Password)
                    {
                        var objCustomUserData = new CustomUserData();
                        objCustomUserData.UserID = checkUser.Id;
                        objCustomUserData.UserName = checkUser.UserName;
                        objCustomUserData.DisplayName = checkUser.FirstName + " " + checkUser.LastName;

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        string userData = serializer.Serialize(objCustomUserData);

                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                 1,
                                 objCustomUserData.UserName,
                                 DateTime.Now,
                                 DateTime.Now.AddMinutes(30),
                                 true,
                                 userData);

                        string encTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);

                        return RedirectToAction("GetAllProducts", "Product");
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Invalid credentials. Please relogin";
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "Login", "HomeController");
            }

            return View(objUser);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}