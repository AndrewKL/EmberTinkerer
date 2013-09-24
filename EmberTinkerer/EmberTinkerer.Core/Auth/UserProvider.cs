using System;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;
using Raven.Client.Document;

namespace EmberTinkerer.Core.Auth
{
    public interface IRavenDbMembershipProvider
    {
        User CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                        bool isApproved, out MembershipCreateStatus status);

        bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer);
        bool ChangePassword(string username, string oldPassword, string newPassword);
        bool ValidateUser(string username, string password);
    }

    public class UserProvider : IRavenDbMembershipProvider
    {
        private IUserRepo _userRepo;
        private readonly string _machineKey;
        public string ApplicationName { get; set; }
        public string PasswordStrengthRegularExpression { get { return @"^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$"; } }
        
        public UserProvider(IUserRepo userRepo = null, string machineKey = null)
        {
            _userRepo = userRepo ?? CreateRepo();
            _machineKey = machineKey ?? GetMachineKey();
        }

        private IUserRepo CreateRepo()
        {
            var store = new DocumentStore()
            {
                ConnectionStringName = "RavenDB"
            }.Initialize();

            return new UserRepo(store);
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

        public User CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                  bool isApproved, out MembershipCreateStatus status)
        {
            //todo: test this
            
            var newUser = new User()
                {
                    Username = username,
                    Email = email,
                    PasswordQuestion = passwordQuestion,
                    PasswordAnswer = passwordAnswer,
                    DateCreated = DateTime.UtcNow,
                    IsLockedOut = false,
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

        public bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
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

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _userRepo.GetByUsername(username);

            if (user == null) return false;
            if (!user.CheckPassword(oldPassword, _machineKey)) return false;

            user.GeneratePasswordHash(newPassword, _machineKey);
            _userRepo.Update(user);
            return true;
        }

        public bool ValidateUser(string username, string password)
        {
            //todo: test this
            var user = _userRepo.GetByUsername(username);
            if (user == null) return false;
            return user.CheckPassword(password, _machineKey);
        }

        //public bool UnlockUser(string userName)
        //{
        //    //todo: test this
        //    var user = _userRepo.GetByUsername(userName);
        //    if (user == null) return false;
        //    user.IsLockedOut = false;
        //    _userRepo.Update(user);
        //    return true;
        //}
        
        //public string GetUserNameByEmail(string email)
        //{
        //    return _userRepo.GetByEmail(email).UserName;
        //}
    }
}
