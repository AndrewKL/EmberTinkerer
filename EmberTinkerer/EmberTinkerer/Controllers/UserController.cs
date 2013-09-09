using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace EmberTinkerer.Controllers
{
    public class UserController : ApiController
    {
        public void Login(UserModel user)
        {
            if (Membership.ValidateUser(user.Username, user.Password))
            {
                FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                return;
            }
            else
            {
                throw new HttpException(401,"Bad username or password");
            }
        }

        public void Regsiter(UserModel user)
        {
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.Username, user.Password, user.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

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
    }
}