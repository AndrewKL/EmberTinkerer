using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Tests.Helpers;

namespace EmberTinkerer.Core.Tests.Repo
{
    [TestFixture]
    class ProjectRepoTests : RavenTestBase
    {
        public DocumentStore _store;
        private ProjectRepo _repo;
        [SetUp]
        public void SetUp()
        {
            _store = new EmbeddableDocumentStore { RunInMemory = true };
            _store.Initialize();
            _repo = new ProjectRepo(_store);
        }
        [Test]
        public void AddAndGetProject()
        {
            var project = new Project()
                {
                    Description = "description",
                    Html = "<html></html>",
                    Javascript = "alert('test');",
                    Name = "name"
                };

            _repo.Add(project);
            WaitForIndexing(_store);

            var reloadedProject = _repo.Get(project.GetIntId());

            Assert.AreEqual(project.Description,reloadedProject.Description);
            Assert.AreEqual(project.Name, reloadedProject.Name);
            Assert.AreEqual(project.Javascript, reloadedProject.Javascript);
            Assert.AreEqual(project.Html, reloadedProject.Html);
        }

        [Test]
        public void GetAllTest()
        {
            var project1 = new Project()
            {
                Name = "name 1",
                Tags = new Collection<string>() { "a", "b", "c" },
                Description = "test description 1",
                Author = "author",
                Rating = 1,
                Upvotes = 1,
                Downvotes = 0,
                Html = "<html></html>",
                Javascript = "alert('test');",
            };
            var project2 = new Project()
            {
                Name = "name 2",
                Tags = new Collection<string>() { "a", "b", "c" },
                Description = "test description 2",
                Author = "author",
                Rating = 1,
                Upvotes = 1,
                Downvotes = 0,
                Html = "<html></html>",
                Javascript = "alert('test');",
            };
            var project3 = new Project()
            {
                Name = "name 3",
                Tags = new Collection<string>() { "a", "b", "c" },
                Description = "test description 3",
                Author = "author",
                Rating = 1,
                Upvotes = 1,
                Downvotes = 0,
                Html = "<html></html>",
                Javascript = "alert('test');",
            };

            _repo.Add(project1);
            _repo.Add(project2);
            _repo.Add(project3);
            WaitForIndexing(_store);

            var projects = _repo.GetAll();
            Assert.AreEqual(3,projects.Count());
        }

        [Test]
        public void UpdateProject()
        {
            var project = new Project()
            {
                Description = "description",
                Html = "<html></html>",
                Javascript = "alert('test');",
                Name = "name"
            };

            _repo.Add(project);
            WaitForIndexing(_store);

            project.Description = "updated description";
            project.Html = "<html> update</html>";
            project.Javascript = "alert('update');";
            project.Name = "updated name";

            _repo.Update(project);
            
            WaitForIndexing(_store);

            var reloadedProject = _repo.Get(project.GetIntId());

            Assert.AreEqual(project.Description, reloadedProject.Description);
            Assert.AreEqual(project.Name, reloadedProject.Name);
            Assert.AreEqual(project.Javascript, reloadedProject.Javascript);
            Assert.AreEqual(project.Html, reloadedProject.Html);
        }

        [Test]
        public void searchByNameTest()
        {
            var project1 = new Project()
            {
                Description = "description 1",
                Html = "<html></html>",
                Javascript = "alert('test');",
                Name = "name"
            };
            var project2 = new Project()
            {
                Description = "description 2",
                Html = "<html></html>",
                Javascript = "alert('test');",
                Name = "name"
            };
            var project3 = new Project()
            {
                Description = "test description 3",
                Html = "<html></html>",
                Javascript = "alert('test');",
                Name = "test name"
            };

            _repo.Add(project1);
            _repo.Add(project2);
            _repo.Add(project3);
            WaitForIndexing(_store);

            var projects = _repo.SearchByName("test");
            Assert.AreEqual(1, projects.Count());
        }

        [TearDown]
        public void TearDown()
        {
            _store.Dispose();
        }
    }
}
