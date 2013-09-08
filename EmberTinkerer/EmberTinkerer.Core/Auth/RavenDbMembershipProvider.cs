using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class RavenDbMembershipProvider : MembershipProvider
    {
        private IUserRepo _userRepo;
        private readonly string _machineKey;
        public override string ApplicationName { get; set; }
        public override int MinRequiredPasswordLength { get { return User.MinPasswordLength; } }
        public override int MinRequiredNonAlphanumericCharacters { get { return 0; } }
        public override string PasswordStrengthRegularExpression { get { return @"^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$"; } }

        public RavenDbMembershipProvider(IUserRepo userRepo = null, string machineKey = null)
        {
            _userRepo = userRepo;
            _machineKey = machineKey ?? GetMachineKey();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            //TODO:test this
        }

        private string GetMachineKey()
        {
            var cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
            var machineKeySection = cfg.GetSection("system.web/machineKey") as MachineKeySection;
            var validationKey =  machineKeySection.ValidationKey;
            if(String.IsNullOrEmpty(validationKey)) throw new ArgumentException("no validation key");
            return validationKey;
        }

        public void SetUserRepo(IUserRepo repo)
        {
            _userRepo = repo;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                  bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            //todo: test this
            
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

            newUser.GeneratePasswordHash(password,_machineKey);

            status = _userRepo.AddUser(newUser) ? MembershipCreateStatus.Success : MembershipCreateStatus.DuplicateUserName;

            return newUser;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            //todo: test this
            var user = _userRepo.GetByUsername(username);

            if (user == null) return false;
            if (!user.CheckPassword(password, _machineKey)) return false;

            user.PasswordQuestion = newPasswordQuestion;
            user.PasswordAnswer = newPasswordAnswer;
            _userRepo.Update(user);
            return true;
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //todo: test this
            var user = _userRepo.GetByUsername(username);

            if (user == null) return false;
            if (!user.CheckPassword(oldPassword, _machineKey)) return false;

            user.GeneratePasswordHash(newPassword, _machineKey);
            _userRepo.Update(user);
            return true;
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
            //todo: test this
            var user = _userRepo.GetByUsername(username);
            if (user == null) return false;
            return user.CheckPassword(password, _machineKey);
        }

        public override bool UnlockUser(string userName)
        {
            //todo: test this
            var user = _userRepo.GetByUsername(userName);
            if (user == null) return false;
            user.IsLockedOut = false;
            _userRepo.Update(user);
            return true;
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
            return _userRepo.GetByEmail(email).UserName;
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
            get { return true; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }
    }
}
