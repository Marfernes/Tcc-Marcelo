using PagedList;
using SCRH.Helpers;
using SCRH.Models;
using SCRH.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCRH.Areas.Recepcao.Controllers
{
    [Permissoes(Roles = "Recepcionista,Gerente")]
    public class ReservasController : Controller
    {
        private readonly QuartoRepository quartoRepository;
        private readonly UsuarioRepository usuarioRepository;
        private readonly Context.ConexaoBanco db = new Context.ConexaoBanco();

        public ReservasController()
        {
            quartoRepository = new QuartoRepository(db);
            usuarioRepository = new UsuarioRepository(db);
        }

        public ActionResult Index(int pagina = 1)
        {
            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var quartos = quartoRepository.BuscarTodos().Where(x => !x.Excluido).ToList();
            var lista = new List<Models.Quarto>();
            lista.AddRange(quartos);
            return View(lista.ToPagedList(pagina, itensPorPagina));
        }

        [HttpPost]
        public ActionResult Adicionar(Guid quartoId, Models.Reserva reserva)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", new { area = "Recepcao" }).Error("Erro ao cadastrar quarto.");

            new ReservaRepository(quartoRepository.BuscarPorId(quartoId), db)
                .Adicionar(reserva);
            return RedirectToAction("Index", new { area = "Recepcao" }).Success("Reserva cadastrado com sucesso.");
        }

        [HttpPost]
        public ActionResult Atualizar(Guid quartoId, Models.Reserva reserva)
        {
            new ReservaRepository(quartoRepository.BuscarPorId(quartoId), db)
                .Atualizar(reserva);
            return RedirectToAction("Index", new { area = "Recepcao" }).Success("Reserva atualizado com sucesso.");
        }

        [HttpPost]
        public ActionResult Excluir(Guid quartoId, Models.Reserva reserva)
        {
            var quarto = quartoRepository.BuscarPorId(quartoId);
            reserva = quarto.Reservas.FirstOrDefault(x => x.Id == reserva.Id);
            reserva.ExcluirReserva();
            new ReservaRepository(quarto, db)
                .Excluir(reserva);
            return RedirectToAction("Index", new { area = "Recepcao" }).Success("Reserva excluído com sucesso.");
        }

        public PartialViewResult AbrirModalAdicionar()
        {
            ViewBag.UsuarioId = new SelectList(usuarioRepository.Buscar(TipoUsuario.Usuario), "UsuarioId", null);
            return PartialView("_Adicionar");
        }

        public PartialViewResult AbrirModalAtualizar(Guid id, Guid quartoId)
        {
            var quarto = quartoRepository.BuscarPorId(quartoId);
            var reserva = new ReservaRepository(quarto, db).Buscar(id);
            ViewBag.UsuarioId = new SelectList(usuarioRepository.Buscar(TipoUsuario.Usuario), "UsuarioId", null, reserva.UsuarioId);

            var quartos = quartoRepository.BuscarTodos().Where(x => x.IsDisponivel(reserva.Data, reserva.DataFinal)).Append(quarto);
            ViewBag.QuartoId = new SelectList(quartos, "Id", null, quartoId);
            return PartialView("_Atualizar", reserva);
        }

        #region CONSUMO

        public PartialViewResult AbrirModalConsumo(Guid reservaId, Guid quartoId)
        {
            ViewBag.ReservaId = reservaId;
            ViewBag.QuartoId = quartoId;
            var quarto = quartoRepository.BuscarPorId(quartoId);
            var reserva = new ReservaRepository(quarto, db).Buscar(reservaId);
            return PartialView("_Consumo", reserva);
        }

        [HttpPost]
        public ActionResult CadastrarConsumo(Guid reservaId, Guid quartoId, Consumo consumo)
        {
            if (ModelState.IsValid)
            {
                var quarto = quartoRepository.BuscarPorId(quartoId);
                var reservaRepository = new ReservaRepository(quarto, db);
                var reserva = reservaRepository.Buscar(reservaId);
                reserva.Consumo.Add(consumo);
                reservaRepository.Atualizar(reserva);

                return RedirectToAction("Index", new { area = "Recepcao" }).Success("Consumo cadastrado com sucesso.");
            }
            return RedirectToAction("Index", new { area = "Recepcao" }).Error("Consumo não foi cadastrado.");
        }

        [HttpPost]
        public ActionResult ApagarConsumo(Guid reservaId, Guid quartoId, Consumo consumo)
        {
            var quarto = quartoRepository.BuscarPorId(quartoId);
            var reservaRepository = new ReservaRepository(quarto, db);
            var reserva = reservaRepository.Buscar(reservaId);

            reserva.Consumo.Remove(reserva.Consumo.FirstOrDefault(x => x.Id == consumo.Id));
            reservaRepository.Atualizar(reserva);

            TempData["Pagina"] = 1;
            var quartos = quartoRepository.BuscarTodos().Where(x => !x.Excluido).ToList();
            return PartialView("_PartialTabela", quartos.ToPagedList(1, 5));
        }

        #endregion

        public PartialViewResult AbrirModalExcluir(Guid id, Guid quartoId)
        {
            var quarto = quartoRepository.BuscarPorId(quartoId);
            return PartialView("_Excluir", new ReservaRepository(quarto, db).Buscar(id));
        }

        public PartialViewResult BuscarReservas(string textoDigitado, int pagina = 1)
        {
            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var quartos = quartoRepository.BuscarTodosPorFiltro(textoDigitado).Where(x => !x.Excluido).ToList();
            
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