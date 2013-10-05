using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using Autofac.Integration.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using EmberTinkerer.Controllers;
using EmberTinkerer.Core.Auth;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.UniqueConstraints;

namespace EmberTinkerer.Code
{
    public class AutofacModule
    {
        public static Autofac.IContainer GetAutofacContainer()
        {
            var builder = new Autofac.ContainerBuilder();

            //ravenDB stuff
            

            //MembershipProvider.UserRepo = new UserRepo(store);

            builder.Register(x =>
                {
                    var store = new DocumentStore()
                        {
                            ConnectionStringName = "RavenDB"
                        };
                
                    store.RegisterListener(new UniqueConstraintsStoreListener());
                    store.Initialize();
                return store;
            })
           .As<IDocumentStore>()
           .SingleInstance();

            //builder.RegisterInstance(store).As<IDocumentStore>().SingleInstance();
            
            //controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());//.InjectActionInvoker();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());//.InjectActionInvoker();
            //builder.Register(c => new ProjectController(c.Resolve<IProjectRepo>(), HttpContext.Current.User.Identity.Name));

            //repos
            builder.RegisterType<ProjectRepo>().As<IProjectRepo>().SingleInstance();
            builder.RegisterType<UserRepo>().As<IUserRepo>().SingleInstance();
            builder.RegisterType<UserProvider>().As<IUserProvider>().SingleInstance();

            //auth
            builder.Register(c =>
                {
                    if (String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name)) return new User();
                    else
                    {
                        var user = c.Resolve<IUserRepo>().GetByUsername(HttpContext.Current.User.Identity.Name);
                        return user;
                    }
                }).InstancePerHttpRequest();

            builder.RegisterType<UserInjectorModelBinder>();

            return builder.Build();
        }

    }
}
