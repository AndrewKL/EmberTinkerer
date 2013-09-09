using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using System.Web.Security;
using Raven.Client.UniqueConstraints;

namespace EmberTinkerer.Core.Documents
{
    public class User : MembershipUser
    {
        public string Id { get; set; }
        
        [UniqueConstraint(CaseInsensitive = true)]
        public string Username { get; set; }
        [UniqueConstraint(CaseInsensitive = true)]
        public string Email { get; set; }
        public string ApplicationName { get; set; }
        public IList<string> Roles { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public string FullName { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsOnline { get; set; }

        public DateTimeOffset DateLastLogin { get; set; }
        public int FailedPasswordAttempts { get; set; }
        public int FailedPasswordAnswerAttempts { get; set; }
        public DateTimeOffset LastFailedPasswordAttempt { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }

        public bool ValidUsername()
        {
            return Regex.Match(Username, "^[a-zA-Z0-9_]+$").Success
                   && Username.Length >= 5;
        }

        public bool ValidEmail()
        {
            try {
                new System.Net.Mail.MailAddress(Email);
                return true;
            }
            catch {
                return false;
            }
        }

        public static bool ValidPassword(string password)
        {
            return password.Length >= User.MinPasswordLength;
        }

        public const int MinPasswordLength = 8;

        public void GeneratePasswordSalt()
        {
            PasswordSalt =  Crypto.GenerateSalt();
        }

        public void GeneratePasswordHash(string password, string machineKey)
        {
            if (this.PasswordSalt == null) GeneratePasswordSalt();
            this.PasswordHash = Crypto.HashPassword(password + ":" + PasswordSalt + ":" + machineKey);
        }

        public bool CheckPassword(string unverifiedPassword, string machineKey)
        {
            var saltedpw = unverifiedPassword + ":" + PasswordSalt + ":" + machineKey;
            return Crypto.VerifyHashedPassword(this.PasswordHash, saltedpw);
        }
    }
}
