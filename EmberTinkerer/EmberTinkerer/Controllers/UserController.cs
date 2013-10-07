using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Security;
using EmberTinkerer.Code;
using EmberTinkerer.Core.Auth;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;

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
        public LoginModel Login(UserModel user)
        {
            if (_membershipProvider.ValidateUser(user.Username, user.Password))
            {
                FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                return new LoginModel(){ LoginSucceeded = true};
            }
            else
            {
                return new LoginModel() { LoginSucceeded = false, ErrorMessage = "Log in failed. Bad username or password."};
            }
        }
        public class LoginModel
        {
            public bool LoginSucceeded { get; set; }
            public string ErrorMessage { get; set; }
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
        public class UserModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
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
        
        [HttpGet]
        public UserInformationModel GetCurrentUserInformation([ModelBinder(typeof(UserInjectorModelBinder))] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username))throw new HttpException("user not logged in or doesn't exist");
            return new UserInformationModel()
                {
                    Username = user.Username,
                    Email = user.Email
                };
        }
        public class UserInformationModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }
    }
}