using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using EmberTinkerer.Code;
using EmberTinkerer.Core.Documents;

namespace EmberTinkerer.Controllers
{
    public class DebugApiController : ApiController
    {
        [HttpGet]
        public User InjectUser([ModelBinder(typeof(UserInjectorModelBinder))]User user)
        {
            return user;
        }
    }
}
