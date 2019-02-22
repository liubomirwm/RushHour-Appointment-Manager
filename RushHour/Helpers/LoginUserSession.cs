using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RushHour.Helpers
{
    public class LoginUserSession
    {
        public int UserId { get; private set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsAuthenticated { get; private set; }
        public bool IsAdmin { get; private set; }

        public static LoginUserSession Current
        {
            get
            {
                LoginUserSession loginUserSession = (LoginUserSession)HttpContext.Current.Session["LoginUser"];
                if (loginUserSession == null)
                {
                    loginUserSession = new LoginUserSession();
                    HttpContext.Current.Session["LoginUser"] = loginUserSession;
                }
                return loginUserSession;
            }
        }

        public void SetCurrentUser(int userId, string email, string name, bool isAdmin)
        {
            this.UserId = userId;
            this.Email = email;
            this.Name = name;
            this.IsAdmin = isAdmin;
            this.IsAuthenticated = true;
        }

        public void Logout()
        {
            this.IsAuthenticated = false;
            this.UserId = 0;
            this.Email = null;
            this.Name = null;
            this.IsAdmin = false;
        }
    }
}