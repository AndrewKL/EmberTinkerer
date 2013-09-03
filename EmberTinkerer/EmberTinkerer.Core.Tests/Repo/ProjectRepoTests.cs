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
                    description = "description",
                    html = "<html></html>",
                    javascript = "alert('test');",
                    name = "name"
                };

            _repo.Add(project);
            WaitForIndexing(_store);

            var reloadedProject = _repo.Get(project.GetIntId());

            Assert.AreEqual(project.description,reloadedProject.description);
            Assert.AreEqual(project.name, reloadedProject.name);
            Assert.AreEqual(project.javascript, reloadedProject.javascript);
            Assert.AreEqual(project.html, reloadedProject.html);
        }

        [Test]
        public void AddWithExistingBadId()
        {
            var project = new Project()
            {
                Id = "not what its supposed to be",
                description = "description",
                html = "<html></html>",
                javascript = "alert('test');",
                name = "name"
            };

            _repo.Add(project);
            WaitForIndexing(_store);

            var reloadedProject = _repo.Get(project.GetIntId());

            Assert.True(reloadedProject.Id.Contains("project"),"id: "+reloadedProject.Id);
            Assert.AreEqual(project.description, reloadedProject.description);
            Assert.AreEqual(project.name, reloadedProject.name);
            Assert.AreEqual(project.javascript, reloadedProject.javascript);
            Assert.AreEqual(project.html, reloadedProject.html);
        }

        [Test]
        public void GetAllTest()
        {
            var project1 = new Project()
            {
                name = "name 1",
                tags = new Collection<string>() { "a", "b", "c" },
                description = "test description 1",
                author = "author",
                rating = 1,
                upvotes = 1,
                downvotes = 0,
                html = "<html></html>",
                javascript = "alert('test');",
            };
            var project2 = new Project()
            {
                name = "name 2",
                tags = new Collection<string>() { "a", "b", "c" },
                description = "test description 2",
                author = "author",
                rating = 1,
                upvotes = 1,
                downvotes = 0,
                html = "<html></html>",
                javascript = "alert('test');",
            };
            var project3 = new Project()
            {
                name = "name 3",
                tags = new Collection<string>() { "a", "b", "c" },
                description = "test description 3",
                author = "author",
                rating = 1,
                upvotes = 1,
                downvotes = 0,
                html = "<html></html>",
                javascript = "alert('test');",
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
                description = "description",
                html = "<html></html>",
                javascript = "alert('test');",
                name = "name"
            };

            _repo.Add(project);
            WaitForIndexing(_store);

            project.description = "updated description";
            project.html = "<html> update</html>";
            project.javascript = "alert('update');";
            project.name = "updated name";

            _repo.Update(project);
            
            WaitForIndexing(_store);

            var reloadedProject = _repo.Get(project.GetIntId());

            Assert.AreEqual(project.description, reloadedProject.description);
            Assert.AreEqual(project.name, reloadedProject.name);
            Assert.AreEqual(project.javascript, reloadedProject.javascript);
            Assert.AreEqual(project.html, reloadedProject.html);
        }

        [Test]
        public void searchByNameTest()
        {
            var project1 = new Project()
            {
                description = "description 1",
                html = "<html></html>",
                javascript = "alert('test');",
                name = "name"
            };
            var project2 = new Project()
            {
                description = "description 2",
                html = "<html></html>",
                javascript = "alert('test');",
                name = "name"
            };
            var project3 = new Project()
            {
                description = "test description 3",
                html = "<html></html>",
                javascript = "alert('test');",
                name = "test name"
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
