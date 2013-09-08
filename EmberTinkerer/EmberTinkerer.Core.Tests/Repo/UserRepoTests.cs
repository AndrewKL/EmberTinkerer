using System;
using System.ComponentModel.Composition.Hosting;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using Raven.Abstractions.Exceptions;
using Raven.Bundles.UniqueConstraints;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.UniqueConstraints;
using Raven.Tests.Helpers;

namespace EmberTinkerer.Core.Tests.Repo
{
    [TestFixture]
    class UserRepoTests : RavenTestBase
    {
        private UserRepo _userRepo;
        private DocumentStore _store;
        private readonly CompareObjects comparer = new CompareObjects();

        [SetUp]
        public void Setup()
        {
            var store = new EmbeddableDocumentStore { RunInMemory = true, };
            store.Configuration.Catalog.Catalogs.Add(new AssemblyCatalog(typeof(UniqueConstraintsPutTrigger).Assembly));
            store.RegisterListener(new UniqueConstraintsStoreListener());
            store.Initialize();

            _store = store;

            _userRepo = new UserRepo(_store);
        }

        [TearDown]
        public void TearDown()
        {
            _store.Dispose();
        }

        [Test]
        public void AddAndGetUserById()
        {
            var user = new User
            {
                Username = "userA",
                ApplicationName = "ember",
                Comment = "comment",
                DateCreated = new DateTimeOffset(new DateTime(2001, 1, 1)),
                DateLastLogin = new DateTimeOffset(new DateTime(2002, 1, 1)),
                Email = "user@gmail.com",
                FailedPasswordAnswerAttempts = 12,
                FailedPasswordAttempts = 10,
                LastFailedPasswordAttempt = new DateTime(2001, 1, 1),
                FullName = "asmithee",
                IsLockedOut = false,
                IsApproved = true,
                PasswordSalt = "asdf1234",
                PasswordAnswer = "some answer",
                PasswordHash = "asdfaqwer12341341234",
                PasswordQuestion = "some question",
                IsOnline = true,
                Id = "nonsense id",
            };

            _userRepo.AddUser(user);
            WaitForIndexing(_store);
            var reloadedUser = _userRepo.GetById(user.Id);

            Assert.True(comparer.Compare(user, reloadedUser));
        }

        [Test]
        public void AddAndGetUserByEmail()
        {
            var user = new User
            {
                Username = "userA",
                ApplicationName = "ember",
                Comment = "comment",
                DateCreated = new DateTimeOffset(new DateTime(2001, 1, 1)),
                DateLastLogin = new DateTimeOffset(new DateTime(2002, 1, 1)),
                Email = "user@gmail.com",
                FailedPasswordAnswerAttempts = 12,
                FailedPasswordAttempts = 10,
                LastFailedPasswordAttempt = new DateTime(2001, 1, 1),
                FullName = "asmithee",
                IsLockedOut = false,
                IsApproved = true,
                PasswordSalt = "asdf1234",
                PasswordAnswer = "some answer",
                PasswordHash = "asdfaqwer12341341234",
                PasswordQuestion = "some question",
                IsOnline = true,
                Id = "nonsense id",
            };

            _userRepo.AddUser(user);
            WaitForIndexing(_store);
            var reloadedUser = _userRepo.GetByEmail(user.Email);

            Assert.True(comparer.Compare(user, reloadedUser));
        }

        [Test]
        public void AddAndGetUserByUsername()
        {
            var user = new User
            {
                Username = "usera",
                ApplicationName = "ember",
                Comment = "comment",
                DateCreated = new DateTimeOffset(new DateTime(2001, 1, 1)),
                DateLastLogin = new DateTimeOffset(new DateTime(2002, 1, 1)),
                Email = "user@gmail.com",
                FailedPasswordAnswerAttempts = 12,
                FailedPasswordAttempts = 10,
                LastFailedPasswordAttempt = new DateTime(2001, 1, 1),
                FullName = "asmithee",
                IsLockedOut = false,
                IsApproved = true,
                PasswordSalt = "asdf1234",
                PasswordAnswer = "some answer",
                PasswordHash = "asdfaqwer12341341234",
                PasswordQuestion = "some question",
                IsOnline = true,
                Id = "nonsense id",
            };

            _userRepo.AddUser(user);
            WaitForIndexing(_store);
            var reloadedUser = _userRepo.GetByUsername(user.Username);

            Assert.True(comparer.Compare(user, reloadedUser));
        }

        [Test]
        public void AddUserTwiceThrowsException()
        {
            var usera = new User
            {
                Username = "userA",
                ApplicationName = "ember",
                Comment = "comment",
                DateCreated = new DateTimeOffset(new DateTime(2001, 1, 1)),
                DateLastLogin = new DateTimeOffset(new DateTime(2002, 1, 1)),
                Email = "user@gmail.com",
                FailedPasswordAnswerAttempts = 12,
                FailedPasswordAttempts = 10,
                LastFailedPasswordAttempt = new DateTime(2001, 1, 1),
                FullName = "asmithee",
                IsLockedOut = false,
                IsApproved = true,
                PasswordSalt = "asdf1234",
                PasswordAnswer = "some answer",
                PasswordHash = "asdfaqwer12341341234",
                PasswordQuestion = "some question",
                IsOnline = true,
                Id = "nonsense id",
            };

            var userb = new User
            {
                Username = "userA",
                ApplicationName = "ember",
                Comment = "comment b",
                DateCreated = new DateTimeOffset(new DateTime(2001, 1, 1)),
                DateLastLogin = new DateTimeOffset(new DateTime(2002, 1, 1)),
                Email = "userb@gmail.com",
                FailedPasswordAnswerAttempts = 12,
                FailedPasswordAttempts = 10,
                LastFailedPasswordAttempt = new DateTime(2001, 1, 1),
                FullName = "bsmithee",
                IsLockedOut = false,
                IsApproved = true,
                PasswordSalt = "asdfb1234",
                PasswordAnswer = "some answebr",
                PasswordHash = "asdfaqwer12341341234b",
                PasswordQuestion = "some question",
                IsOnline = true,
                Id = "nonsense id",
            };

            _userRepo.AddUser(usera);
            Assert.False( _userRepo.AddUser(userb));
        }

        [Test]
        public void AddTwoUsersWithSameEmailAddressThrowsException()
        {
            var usera = new User
            {
                Email = "user@gmail.com",
            };

            var userb = new User
            {
                Email = "user@gmail.com",
            };

            _userRepo.AddUser(usera);
            Assert.False( _userRepo.AddUser(userb));
        }

        [Test]
        public void AddTwoUsersWithSameUsernameThrowsException()
        {
            var usera = new User
            {
                Username = "user@gmail.com",
            };

            var userb = new User
            {
                Username = "user@gmail.com",
            };

            _userRepo.AddUser(usera);
            Assert.False( _userRepo.AddUser(userb));
        }
    }
}
