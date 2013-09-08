using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using EmberTinkerer.Core.Auth;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;
using Moq;
using NUnit.Framework;

namespace EmberTinkerer.Core.Tests.Auth
{
    [TestFixture]
    class RavenDbMembershipProviderTests
    {
        public RavenDbMembershipProvider MembershipProvider;
        public string MachineKey = "D44CF7F89CCA6D97D0648415EE68D3ED88C82805119CAA2C2F197906F40ED8FD";
        public Mock<IUserRepo> UserRepo;

        [SetUp]
        public void setup()
        {
            UserRepo = new Mock<IUserRepo>();

            MembershipProvider = new RavenDbMembershipProvider(UserRepo.Object,MachineKey);
        }

        [Test]
        public void CreateUser()
        {
            MembershipCreateStatus status;
            User user = null;
            UserRepo.Setup(x => x.AddUser(It.IsAny<User>())).Returns(true).Callback((User value) => user = value);

            MembershipProvider.CreateUser("username", "password", "email@abc.com", "question", "answer", true,
                                          null,out status);

            Assert.AreEqual("username",user.Username);
            Assert.True(user.CheckPassword("password",MachineKey));
            Assert.AreEqual("email@abc.com",user.Email);
            Assert.AreEqual(true,user.IsApproved);
            Assert.AreEqual(false,user.IsLockedOut);
            Assert.AreEqual("question",user.PasswordQuestion);
            Assert.AreEqual("answer",user.PasswordAnswer);

            Assert.AreEqual(MembershipCreateStatus.Success, status);
        }

        [Test]
        public void CreateUserWithBadUsername()
        {
            MembershipCreateStatus status;

            MembershipProvider.CreateUser("u", "password", "email@abc.com", "question", "answer", true,
                                          null, out status);

            UserRepo.Verify(x => x.AddUser(It.IsAny<User>()), Times.Never);
            Assert.AreEqual(MembershipCreateStatus.InvalidUserName, status);
        }

        [Test]
        public void CreateUserWithBadPassword()
        {
            MembershipCreateStatus status;

            MembershipProvider.CreateUser("username", "", "email@abc.com", "question", "answer", true,
                                          null, out status);

            UserRepo.Verify(x => x.AddUser(It.IsAny<User>()), Times.Never);
            Assert.AreEqual(MembershipCreateStatus.InvalidPassword, status);
        }

        [Test]
        public void CreateUserWithBadEmail()
        {
            MembershipCreateStatus status;

            MembershipProvider.CreateUser("username", "password", "", "question", "answer", true,
                                          null, out status);

            UserRepo.Verify(x => x.AddUser(It.IsAny<User>()), Times.Never);
            Assert.AreEqual(MembershipCreateStatus.InvalidEmail, status);
        }

        [Test]
        public void CreateUserWithDuplicateUsernameOrPassword()
        {
            MembershipCreateStatus status;
            User user = null;
            UserRepo.Setup(x => x.AddUser(It.IsAny<User>())).Returns(false).Callback((User value) => user = value);

            MembershipProvider.CreateUser("username", "password", "email@abc.com", "question", "answer", true,
                                          null, out status);

            Assert.AreEqual(MembershipCreateStatus.DuplicateUserName, status);
        }

        [Test]
        public void AutoGetMachineKey()
        {
            MembershipProvider = new RavenDbMembershipProvider(UserRepo.Object);
        }

        [Test]
        public void ChangePassword()
        {
            var oldPassword = "oldpassword";
            var newPassword = "new password";
            var user = new User();
            user.GeneratePasswordHash(oldPassword,MachineKey);
            User updatedUser = null;

            UserRepo.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(user);
            UserRepo.Setup(x => x.Update(It.IsAny<User>())).Callback((User value) => updatedUser = value);

            Assert.True(MembershipProvider.ChangePassword("username", oldPassword, newPassword));
            UserRepo.Verify(x=>x.Update(It.IsAny<User>()),Times.Once());
            Assert.True(user.CheckPassword(newPassword,MachineKey));
        }

        [Test]
        public void ChangePasswordWithBadPassword()
        {
            var oldPassword = "oldpassword";
            var newPassword = "new password";
            var user = new User();
            user.GeneratePasswordHash(oldPassword, MachineKey);

            UserRepo.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(user);

            Assert.False(MembershipProvider.ChangePassword("username", "bad old password", newPassword));
            UserRepo.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public void ValidateUser()
        {
            var password = "password";
            var user = new User();
            user.GeneratePasswordHash(password, MachineKey);

            UserRepo.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(user);

            Assert.True(MembershipProvider.ValidateUser("username","password"));
            Assert.False(MembershipProvider.ValidateUser("username", "not password"));
        }
    }
}
