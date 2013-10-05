using System;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using EmberTinkerer.Core.Documents;
using EmberTinkerer.Core.Repo;

namespace EmberTinkerer.Code
{
    public class UserInjectorModelBinder : IModelBinder
    {
        private IUserRepo _userRepo;

        public UserInjectorModelBinder(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (actionContext == null) throw new ArgumentNullException("actionContext");
            if (bindingContext == null) throw new ArgumentNullException("bindingContext");

            if (bindingContext.ModelType != typeof(User))return false;

            if (String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
            {
                bindingContext.Model =  new User();
                return true;
            }
            else
            {
                bindingContext.Model = _userRepo.GetByUsername(HttpContext.Current.User.Identity.Name);
                return true;
            }
            
        }

        //// In the Global.asax GlobalConfiguration.Configuration.Services.Add(typeof(ModelBinderProvider), new DateTimeModelBinderProvider());
    }
}