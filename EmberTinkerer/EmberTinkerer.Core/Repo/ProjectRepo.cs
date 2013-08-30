using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmberTinkerer.Core.Documents;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace EmberTinkerer.Core.Repo
{
    interface IProjectRepo
    {
        Project Get(int id);
        IEnumerable<Project> GetAll();
        Project Add(Project project);
        void Update(Project project);
        IEnumerable<Project> SearchByName(string text);
    }

    public class ProjectRepo : IProjectRepo
    {
        private readonly DocumentStore _store;

        public ProjectRepo(DocumentStore store)
        {
            _store = store;
        }

        public Project Get(int id)
        {
            using (var session = _store.OpenSession())
            {
                return session.Load<Project>(id);
            }
        }

        public IEnumerable<Project> GetAll()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Project>();
            }
        }

        public Project Add(Project project)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(project); // in-memory operations are committed asynchronously when calling SaveChangesAsync
                session.SaveChanges(); // returns a task that completes asynchronously
            }
            return project;
        }
        public void Update(Project project)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(project); // in-memory operations are committed asynchronously when calling SaveChangesAsync
                session.SaveChanges(); // returns a task that completes asynchronously
            }
        }
        public IEnumerable<Project> SearchByName(string text)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Project>().Where(x=>x.Name.StartsWith(text));
            }
        }

        public IEnumerable<Project> Search(string searchText, int take, int skip)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<ProjectFullTextIndex.ReduceResult, ProjectFullTextIndex>()
                    .Where(x => x.Query.StartsWith(searchText))
                    .OrderByDescending(x => x.Rating)
                    .Skip(skip)
                    .Take(take)
                    .As<Project>().ToList();
            }
        }

        public class ProjectFullTextIndex : AbstractIndexCreationTask<Project, ProjectFullTextIndex.ReduceResult>
        {
            public class ReduceResult
            {
                public string Query { get; set; }
                public int Rating { get; set; }
            }

            public ProjectFullTextIndex()
            {
                Map = entries => from entry in entries
                                 select new
                                 {
                                     Query = new object[]
                                     {
                                         entry.Author,
                                         entry.Tags,
                                         entry.Name,
                                         entry.Description
                                     },
                                     Rating = entry.Rating
                                 };
                Sort(x => x.Rating, SortOptions.Int);

            }
        }
    }
}
