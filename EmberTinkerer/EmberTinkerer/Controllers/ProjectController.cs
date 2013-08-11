using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;

namespace EmberTinkerer.Controllers
{
    public class ProjectController : ApiController
    {
        //
        // GET: /Project/
        public static StubProjectRepo _repo = new StubProjectRepo();

        public IEnumerable<Project> GetAll()
        {
            return _repo.GetAll();
        }

        public Project Get(int id)
        {
            return _repo.Get(id);
        }

        public void Update(Project project)
        {
            _repo.Update(project);
        }

        [HttpGet]
        public IEnumerable<Project> SearchByName(string text)
        {
            return _repo.SearchByName(text);
        }

        [HttpPost]
        public Project Add(Project project)
        {
            return _repo.Add(project);
        }

        [HttpGet]
        public Project Create()
        {
            return _repo.Create();
        }
    }
}
