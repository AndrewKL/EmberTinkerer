using System.Web;
using System.Web.Http;
using System.Web.Security;
using EmberTinkerer.Core.Auth;

namespace EmberTinkerer.Controllers
{
    public class UserController : ApiController
    {
        private IUserProvider _membershipProvider;

        public UserController(IUserProvider membershipProvider)
        {
            _membershipProvider = membershipProvider;
        }

        [HttpPost]
        public void Login(UserModel user)
        {
            if (_membershipProvider.ValidateUser(user.Username, user.Password))
            {
                FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                return;
            }
            else
            {
                throw new HttpException(401, "Bad username or password");
            }
        }

        [HttpPost]
        public RegistrationModel Register(UserModel user)
        {
            MembershipCreateStatus createStatus;
            _membershipProvider.CreateUser(user.Username, user.Password, user.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, status: out createStatus);

            switch (createStatus)
            {
                case MembershipCreateStatus.Success:
                    FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                    return new RegistrationModel { RegistrationFailed = false };
                case MembershipCreateStatus.DuplicateUserName:
                    return new RegistrationModel { RegistrationFailed = true, ErrorMessage = "This Username is already in use." };
                case MembershipCreateStatus.InvalidUserName:
                    return new RegistrationModel { RegistrationFailed = true, ErrorMessage = "This is an invalid Username." };
                case MembershipCreateStatus.InvalidEmail:
                    return new RegistrationModel { RegistrationFailed = true, ErrorMessage = "This is an invalid Email." };
                case MembershipCreateStatus.DuplicateEmail:
                    return new RegistrationModel { RegistrationFailed = true, ErrorMessage = "This email address is already in use." };
                case MembershipCreateStatus.InvalidPassword:
                    return new RegistrationModel { RegistrationFailed = true, ErrorMessage = "Invalid Password" };
                default:
                    return new RegistrationModel { RegistrationFailed = true, ErrorMessage = "Error" };
            }
        }
        public class RegistrationModel
        {
            public bool RegistrationFailed { get; set; }
            public string ErrorMessage { get; set; }
        }

        [HttpPost]
        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public bool LoginFailed { get; set; }
        public bool RegistrationFailed { get; set; }
        public string ErrorMessage { get; set; }
    }
}