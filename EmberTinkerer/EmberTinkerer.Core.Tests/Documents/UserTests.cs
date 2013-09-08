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
