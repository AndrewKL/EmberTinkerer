﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using EmberTinkerer.Core.Auth;

namespace EmberTinkerer.Controllers
{
    public class AccountController : Controller
    {
        private IRavenDbMembershipProvider _membershipProvider;

        public AccountController(IRavenDbMembershipProvider membershipProvider)
        {
            _membershipProvider = membershipProvider;
        }

        public ActionResult Index()
        {
            return View();
        }


        public void Login(UserModel user)
        {
            if (_membershipProvider.ValidateUser(user.Username, user.Password))
            {
                FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                return;
            }
            else
            {
                throw new HttpException(401,"Bad username or password");
            }
        }

        public void Register(UserModel user)
        {
            MembershipCreateStatus createStatus;
            _membershipProvider.CreateUser(user.Username, user.Password, user.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, status: out createStatus);

        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
