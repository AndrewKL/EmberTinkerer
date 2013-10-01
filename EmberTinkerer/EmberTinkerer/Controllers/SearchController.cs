using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;

namespace EmberTinkerer.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IProjectRepo _repo;

        public SearchController(IProjectRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<Project> GetAll()
        {
            return _repo.GetAll();
        }

        [HttpGet]
        public IEnumerable<Project> Projects(string text)
        {
            return _repo.SearchByName(text);
        }
    }
}
