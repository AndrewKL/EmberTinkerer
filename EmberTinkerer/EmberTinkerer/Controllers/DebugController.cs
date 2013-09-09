using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmberTinkerer.Core.Repo;

namespace EmberTinkerer.Controllers
{
    public class DebugController : Controller
    {
        //
        // GET: /Debug/
        private IProjectRepo _repo;

        public DebugController(IProjectRepo repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
