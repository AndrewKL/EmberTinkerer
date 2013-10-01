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
        private readonly IProjectRepo _repo;
        private readonly User _user;
        
        public ProjectController(IProjectRepo projectRepo, User user)
        {
            _repo = projectRepo;
            _user = user;
        }

        

        public Project Get(int id)
        {
            return _repo.Get(id);
        }

        public void Update(Project project)
        {
            if (project.author != _user.Username) throw new HttpException("You are not the owner of this project");
            _repo.Update(project);
        }

        [HttpPost]
        public Project Add(Project project)
        {
            project.author = _user.Username;
            return _repo.Add(project);
        }
    }
}
