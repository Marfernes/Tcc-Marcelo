using SCRH.Helpers;
using SCRH.Repository;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SCRH.Areas.Recepcao.Controllers
{
    [Permissoes(Roles = "Recepcionista,Gerente")]
    public class HomeController : Controller
    {
        private readonly QuartoRepository quartoRepository;
        public HomeController()
        {
            quartoRepository = new QuartoRepository();
        }
        public ActionResult Index()
        {
            return View(quartoRepository.BuscarTodos().Where(x => !x.Excluido));
        }

        public PartialViewResult BuscarCalendario(DateTime data)
        {
            return PartialView("_PartialTabela", quartoRepository.BuscarTodos().Where(x => !x.Excluido));
        }
    }
}