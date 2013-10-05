using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using EmberTinkerer.Code;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;

namespace EmberTinkerer.Controllers
{
    public class ProjectController : ApiController
    {
        private readonly IProjectRepo _repo;
        
        public ProjectController(IProjectRepo projectRepo)
        {
            _repo = projectRepo;
        }

        public Project Get(int id)
        {
            return _repo.Get(id);
        }

        public void Update(Project project, [ModelBinder(typeof(UserInjectorModelBinder))]User user)
        {
            if (project.author != user.Username) throw new HttpException("You are not the owner of this project");
            _repo.Update(project);
        }

        [HttpPost]
        public Project Add(Project project, [ModelBinder(typeof(UserInjectorModelBinder))]User user)
        {
            project.author = user.Username;
            return _repo.Add(project);
        }

        public IEnumerable<Project> GetAll()
        {
            return _repo.GetAll();
        }

        [HttpGet]
        public IEnumerable<Project> Search(string text)
        {
            return _repo.SearchByName(text);
        }
    }
}
