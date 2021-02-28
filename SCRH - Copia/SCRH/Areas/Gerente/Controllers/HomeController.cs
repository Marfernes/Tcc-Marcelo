using SCRH.Helpers;
using SCRH.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SCRH.Areas.Gerente.Controllers
{
    [Permissoes(Roles = "Gerente")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Resumo()
        {
            int TotalQuartos = 0;
            int TotalQuartosOcupados = 0;
            decimal CaixaUltimos30Dias = 0;
            int Agendamentos = 0;
            int TotalDiariasUltimos30Dias = 0;

            var quartos = new QuartoRepository().BuscarTodos().Where(x => !x.Excluido);
            TotalQuartos = quartos.Count();
            TotalQuartosOcupados = quartos.Count(x => !x.IsDisponivel());

            DateTime data = DateTime.Now.AddDays(-30);

            CaixaUltimos30Dias = quartos.Sum(x => x.Reservas.Where(v => v.DataDeSaida != null && !v.Excluido && v.DataDeEntrada >= data).Sum(v => v.ValorFinal()));

            Agendamentos = quartos.Sum(x => x.Reservas.Where(d => d.Data >= data && !d.Excluido).Count(d => !d.IsCheckin()));

            TotalDiariasUltimos30Dias = quartos.Sum(x => x.Reservas.Where(d => d.DataDeEntrada >= data && !d.Excluido).Count(d => d.DataDeSaida != null));

            return Json(new { TotalQuartos, TotalQuartosOcupados, CaixaUltimos30Dias, Agendamentos, TotalDiariasUltimos30Dias });
        }
    }
}