using PagedList;
using SCRH.Helpers;
using SCRH.Models;
using SCRH.Repository;
using SCRH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SCRH.Areas.User.Controllers
{
    [Permissoes(Roles = "Usuario")]
    public class HomeController : Controller
    {
        private readonly QuartoRepository quartoRepository;
        private readonly UsuarioRepository usuarioRepository;
        private readonly Context.ConexaoBanco db = new Context.ConexaoBanco();

        public HomeController()
        {
            quartoRepository = new QuartoRepository(db);
            usuarioRepository = new UsuarioRepository(db);
        }

        public ActionResult Index(int pagina = 1)
        {
            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var usuarioId = Account.UsuarioId;
            ViewBag.UsuarioId = usuarioId;
            var quartos = quartoRepository.BuscarTodos().Where(x => !x.Excluido && x.Reservas.Any(r=>r.UsuarioId==usuarioId)).ToList();
            var lista = new List<Models.Quarto>();
            lista.AddRange(quartos);
            return View(lista.ToPagedList(pagina, itensPorPagina));
        }

        [HttpPost]
        public ActionResult Adicionar(Guid quartoId, Models.Reserva reserva)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", new { area = "User" }).Error("Erro ao cadastrar quarto.");

            new ReservaRepository(quartoRepository.BuscarPorId(quartoId), db)
                .Adicionar(reserva);
            return RedirectToAction("Index", new { area = "User" }).Success("Reserva cadastrado com sucesso.");
        }

        [HttpPost]
        public ActionResult Atualizar(Guid quartoId, Models.Reserva reserva)
        {
            new ReservaRepository(quartoRepository.BuscarPorId(quartoId), db)
                .Atualizar(reserva);
            return RedirectToAction("Index", new { area = "User" }).Success("Reserva atualizado com sucesso.");
        }

        [HttpPost]
        public ActionResult Excluir(Guid quartoId, Models.Reserva reserva)
        {
            var quarto = quartoRepository.BuscarPorId(quartoId);
            reserva = quarto.Reservas.FirstOrDefault(x => x.Id == reserva.Id);
            reserva.ExcluirReserva();
            new ReservaRepository(quarto, db)
                .Excluir(reserva);
            return RedirectToAction("Index", new { area = "User" }).Success("Reserva excluído com sucesso.");
        }

        public PartialViewResult AbrirModalAdicionar()
        {
            ViewBag.UsuarioId = Account.UsuarioId;
            return PartialView("_Adicionar");
        }

        public PartialViewResult AbrirModalAtualizar(Guid id, Guid quartoId)
        {
            var quarto = quartoRepository.BuscarPorId(quartoId);
            var reserva = new ReservaRepository(quarto, db).Buscar(id);
            ViewBag.UsuarioId = Account.UsuarioId;

            var quartos = quartoRepository.BuscarTodos().Where(x => x.IsDisponivel(reserva.Data, reserva.DataFinal)).Append(quarto);
            ViewBag.QuartoId = new SelectList(quartos, "Id", null, quartoId);
            return PartialView("_Atualizar", reserva);
        }

        public PartialViewResult AbrirModalExcluir(Guid id, Guid quartoId)
        {
            var quarto = quartoRepository.BuscarPorId(quartoId);
            return PartialView("_Excluir", new ReservaRepository(quarto, db).Buscar(id));
        }

        public PartialViewResult BuscarReservas(string textoDigitado, int pagina = 1)
        {
            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var usuarioId = Account.UsuarioId;
            ViewBag.UsuarioId = usuarioId;
            var quartos = quartoRepository.BuscarTodosPorFiltro(textoDigitado).Where(x => !x.Excluido && x.Reservas.Any(r => r.UsuarioId == usuarioId)).ToList();
            return PartialView("_PartialTabela", quartos.ToPagedList(pagina, itensPorPagina));
        }

        public ActionResult CheckInOut(Guid id, Guid quartoId, int pagina = 1)
        {
            var quarto = quartoRepository.BuscarPorId(quartoId);
            var reservaRepository = new ReservaRepository(quarto, db);
            var reserva = reservaRepository.Buscar(id);
            if (reserva.IsCheckin())
            {
                reserva.FazerCheckout();
            }
            else
            {
                reserva.FazerCheckin();
            }
            reservaRepository.Atualizar(reserva);

            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var quartos = quartoRepository.BuscarTodos().Where(x => !x.Excluido).ToList();
            var lista = new List<Models.Quarto>();
            lista.AddRange(quartos);
            return PartialView("_PartialTabela", lista.ToPagedList(pagina, itensPorPagina));
        }

        [HttpPost]
        public JsonResult BuscarQuartos(DateTime data, DateTime dataFinal)
        {
            var quartos = quartoRepository.BuscarTodos();
            return Json(new SelectList(quartos.Where(x => x.IsDisponivel(data, dataFinal)), "Id", null));
        }

        [HttpPost]
        public JsonResult CalcularValor(Guid quartoId, DateTime data1, DateTime data2)
        {
            var diarias = data2 - data1;
            var quarto = quartoRepository.BuscarPorId(quartoId);
            return Json(quarto?.ValorDiaria * (int)diarias.TotalDays);
        }
    }
}