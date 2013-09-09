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
using EmberTinkerer.Core.Repo;
using Raven.Client;
using Raven.Client.Document;

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
                }.Initialize();
                return store;
            })
           .As<IDocumentStore>()
           .SingleInstance();

            //builder.RegisterInstance(store).As<IDocumentStore>().SingleInstance();
            
            //controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());//.InjectActionInvoker();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());//.InjectActionInvoker();

            //repos
            builder.RegisterType<ProjectRepo>().As<IProjectRepo>().SingleInstance();
            
            return builder.Build();
        }

    }
}
