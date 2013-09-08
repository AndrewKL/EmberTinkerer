using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Security;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;
using Raven.Abstractions.Exceptions;
using Raven.Client;
using Raven.Client.Document;

namespace EmberTinkerer.Core.Auth
{
    class RavenDbMembershipProvider : MembershipProvider
    {
        private IUserRepo _userRepo;
        private readonly string _machineKey;
        public override string ApplicationName { get; set; }
        public override int MinRequiredPasswordLength { get { throw new NotImplementedException(); } }
        public override int MinRequiredNonAlphanumericCharacters { get { throw new NotImplementedException(); } }
        public override string PasswordStrengthRegularExpression { get { throw new NotImplementedException(); } }

        public RavenDbMembershipProvider(IUserRepo userRepo = null, string machineKey = null)
        {
            _userRepo = userRepo;
            _machineKey = machineKey ?? GetMachineKey();
        }

        private string GetMachineKey()
        {
            var cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
            var machineKeySection = cfg.GetSection("system.web/machineKey") as MachineKeySection;
            return machineKeySection.ValidationKey;
        }

        public void SetUserRepo(IUserRepo repo)
        {
            _userRepo = repo;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                  bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            
            var newUser = new User()
                {
                    Username = username,
                    Email = email,
                    PasswordQuestion = passwordQuestion,
                    PasswordAnswer = passwordAnswer,
                    IsApproved = isApproved,
                    DateCreated = DateTime.UtcNow,
                    IsLockedOut = false,
                    IsOnline = false,
                    PasswordSalt = GeneratePasswordSalt(),
                };
            if (!newUser.ValidUsername())
            {
                status = MembershipCreateStatus.InvalidUserName;
                return newUser;
            }
            if (!newUser.ValidEmail())
            {
                status = MembershipCreateStatus.InvalidEmail;
                return newUser;
            }
            if (!User.ValidPassword(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return newUser;
            }

            newUser.PasswordHash = GeneratePasswordHash(password,newUser.PasswordSalt);

            status = _userRepo.AddUser(newUser) ? MembershipCreateStatus.Success : MembershipCreateStatus.InvalidUserName;

            return newUser;
        }

        private string GeneratePasswordSalt()
        {
            return Crypto.GenerateSalt();
        }

        private string GeneratePasswordHash(string password, string salt)
        {
            return Crypto.HashPassword(password+":"+salt+":"+_machineKey);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
                                                             string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
    }
}
