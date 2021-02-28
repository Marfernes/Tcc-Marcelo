using SCRH.Utils;
using System.Web.Mvc;
using System.Web.Routing;

namespace SCRH.Helpers
{
    public class Permissoes : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult && Account.NomeCompleto != null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
               new { action = "Error", controller = "Home", area = "" }));
            }
            else if (Account.NomeCompleto == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
               new { action = "Index", controller = "Home", area = "" })).Information("Usuário não está logado");
            }
        }
    }
}