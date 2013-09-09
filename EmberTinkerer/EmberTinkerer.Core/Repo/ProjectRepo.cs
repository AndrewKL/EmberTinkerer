using System.Collections.Generic;
using System.Linq;
using EmberTinkerer.Core.Documents;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace EmberTinkerer.Core.Repo
{
    public interface IProjectRepo
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
            project.Id = null;
            using (var session = _store.OpenSession())
            {
                session.Store(project);
                session.SaveChanges(); 
            }
            return project;
        }
        public void Update(Project project)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(project); 
                session.SaveChanges(); 
            }
        }
        public IEnumerable<Project> SearchByName(string text)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Project>().Where(x=>x.name.StartsWith(text));
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
                                         entry.author,
                                         entry.tags,
                                         entry.name,
                                         entry.description
                                     },
                                     Rating = entry.rating
                                 };
                Sort(x => x.Rating, SortOptions.Int);

            }
        }
    }
}
