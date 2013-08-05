using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using EmberTinkerer.Code;
using EmberTinkerer.Models;

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
        public void Add(Project project)
        {
            _repo.Add(project);
        }
    }
}
