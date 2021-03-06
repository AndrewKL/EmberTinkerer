﻿using System;
using EmberTinkerer.Core.Documents;
using Raven.Abstractions.Exceptions;
using Raven.Client;
using Raven.Client.UniqueConstraints;

namespace EmberTinkerer.Core.Repo
{
    public interface IUserRepo
    {
        bool AddUser(User user);
        User GetById(string id);
        User GetByUsername(string username);
        User GetByEmail(string email);

        void Update(User user);
    }

    public class UserRepo : IUserRepo
    {
        private readonly IDocumentStore _store;

        public UserRepo(IDocumentStore store)
        {
            _store = store;
        }

        public bool AddUser(User user)
        {
            user.Id=null;
            using (var session = _store.OpenSession())
            {
                session.Store(user); 
                try
                {
                    session.SaveChanges();
                }
                catch (Exception e)
                {
                    /* Oren Ini has bad opinions.  the ravendb client should throw a OperationVetoedException
                     * and not an InvalidOperationException when an insert is vetoed by the unique
                     * constraints bundle.(and for some strange reason embeded ravenDB does the right thing) */ 
                    if (e.GetType() == typeof(OperationVetoedException)
                        || e.GetType() == typeof(InvalidOperationException))
                    return false;
                }
                return true;
            }
        }

        public User GetById(string id)
        {
            using (var session = _store.OpenSession())
            {
                return session.Load<User>(id);
            }
        }

        public User GetByUsername(string username)
        {
            using (var session = _store.OpenSession())
            {
                return session.LoadByUniqueConstraint<User>(x => x.Username, username);
            }
        }

        public User GetByEmail(string email)
        {
            using (var session = _store.OpenSession())
            {
                return session.LoadByUniqueConstraint<User>(x => x.Email, email);
            }
        }

        public void Update(User user)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(user);
                session.SaveChanges();
            }
        }
    }
}
