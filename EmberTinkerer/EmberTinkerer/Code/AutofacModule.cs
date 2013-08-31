using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac.Integration.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using EmberTinkerer.Controllers;
using EmberTinkerer.Core.Repo;
using Raven.Client.Document;

namespace EmberTinkerer.Code
{
    public class AutofacModule
    {
        public static Autofac.IContainer GetAutofacContainer()
        {
            var builder = new Autofac.ContainerBuilder();

            //ravenDB stuff
            var store = new DocumentStore()
            {
                ConnectionStringName = "RavenDB"
            }.Initialize();
            builder.RegisterInstance(store).As<DocumentStore>().SingleInstance();
            
            //controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<ProjectController>().SingleInstance();

            //repos
            builder.RegisterType<ProjectRepo>().As<IProjectRepo>().SingleInstance();
            
            return builder.Build();
        }

    }
}
