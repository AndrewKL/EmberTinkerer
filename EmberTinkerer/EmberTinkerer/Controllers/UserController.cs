using System.Web;
using System.Web.Http;
using System.Web.Security;
using EmberTinkerer.Core.Auth;

namespace EmberTinkerer.Controllers
{
    public class UserController : ApiController
    {
        private IRavenDbMembershipProvider _membershipProvider;

        public UserController(IRavenDbMembershipProvider membershipProvider)
        {
            _membershipProvider = membershipProvider;
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