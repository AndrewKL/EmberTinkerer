using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmberTinkerer.Core.Documents;
using NUnit.Framework;

namespace EmberTinkerer.Core.Tests.Documents
{
    [TestFixture]
    class UserTests
    {
        [Test]
        public void GenerateHashAndSaltAndCanVerify()
        {
            var password = "somearbitrarypw";
            var machinekey = "D44CF7F89CCA6D97D0648415EE68D3ED88C82805119CAA2C2F197906F40ED8FD";
            var user = new User();

            user.GeneratePasswordHash(password, machinekey);
            Assert.True(user.PasswordSalt!=null);
            Assert.True(user.PasswordHash != null);

            Assert.True(user.CheckPassword(password,machinekey));
            Assert.False(user.CheckPassword("not the password", machinekey));
        }


        [Test]
        [TestCase("abc@google.com",Result = true)]
        [TestCase("aBc@Google.com", Result = true)]
        [TestCase("a.b.c@google.com", Result = true)]
        [TestCase("abc@google", Result = true)]
        [TestCase("@google.com", Result = false)]
        [TestCase("@", Result = false)]
        [TestCase("lalala@", Result = false)]
        public bool ValidEmail(string email)
        {
            var user = new User
                {
                    Email = email
                };

            return user.ValidEmail();
        }

        [Test]
        [TestCase("1234", Result = false)]
        [TestCase("12345", Result = true)]
        [TestCase("12345678!", Result = false)]
        [TestCase("12345678@", Result = false)]
        [TestCase("12345678#", Result = false)]
        [TestCase("12345678$", Result = false)]
        [TestCase("12345678%", Result = false)]
        [TestCase("12345678!", Result = false)]
        [TestCase("12345678 ", Result = false)]
        [TestCase("12345678/", Result = false)]
        [TestCase("12345678\\", Result = false)]
        [TestCase("12345678?", Result = false)]
        public bool ValidUsername(string username)
        {
            var user = new User
            {
                Username = username
            };

            return user.ValidUsername();
        }

        [Test]
        [TestCase("1234567", Result = false)]
        [TestCase("12345678", Result = true)]
        public bool ValidPassword(string password)
        {
            return User.ValidPassword(password);
        }
    }
}
