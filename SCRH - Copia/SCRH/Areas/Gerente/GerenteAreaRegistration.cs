using System.Web.Mvc;

namespace SCRH.Areas.Gerente
{
    public class GerenteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Gerente";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Gerente_default",
                "Gerente/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}