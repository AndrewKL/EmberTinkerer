using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;

namespace EmberTinkerer.Core.autofac
{
    [TestFixture]
    class AutofacModuleTest
    {
        public IContainer Container { get; private set; }

        [SetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            ///HttpContext.Current = new HttpContext(new HttpRequest("filename", "http://www.google.com", "asdfasdf"), new HttpResponse(new StreamWriter(new MemoryStream())));
            builder.RegisterModule(new WorkerRoleLogInjectionModule());


            Container = builder.Build();
        }

        [TearDown]
        public void TearDown()
        {
            Container.Dispose();
            Container = null;
        }
    }
}
