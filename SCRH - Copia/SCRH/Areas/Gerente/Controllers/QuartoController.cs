using PagedList;
using SCRH.Helpers;
using SCRH.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCRH.Areas.Gerente.Controllers
{
    [Permissoes(Roles = "Gerente")]
    public class QuartoController : Controller
    {
        private readonly QuartoRepository quartoRepository;
        private readonly CategoriaRepository categoriaRepository;

        public QuartoController()
        {
            var db = new Context.ConexaoBanco();
            quartoRepository = new QuartoRepository(db);
            categoriaRepository = new CategoriaRepository(db);
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
        public ActionResult Adicionar(Models.Quarto quarto)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", new { area = "Gerente" }).Error("Erro ao cadastrar quarto.");           

            quartoRepository.Adicionar(quarto);
            return RedirectToAction("Index", new { area = "Gerente" }).Success("Quarto cadastrado com sucesso.");
        }

        [HttpPost]
        public ActionResult Atualizar(Models.Quarto quarto)
        {
            quarto.AlterarQuarto(quarto);
            quartoRepository.Atualizar(quarto);
            return RedirectToAction("Index", new { area = "Gerente" }).Success("Quarto atualizado com sucesso.");
        }

        [HttpPost]
        public ActionResult Excluir(Models.Quarto quarto)
        {
            var usuarioAtual = quartoRepository.BuscarPorId(quarto.Id);
            usuarioAtual.ExcluirQuarto();
            quartoRepository.Excluir(usuarioAtual);
            return RedirectToAction("Index", new { area = "Gerente" }).Success("Quarto excluído com sucesso.");
        }

        public PartialViewResult AbrirModalAdicionar()
        {
            ViewBag.Categorias = categoriaRepository.BuscarTodos();
            return PartialView("_Adicionar");
        }

        public PartialViewResult AbrirModalAtualizar(Guid id)
        {
            ViewBag.Categorias = categoriaRepository.BuscarTodos();
            var quarto = quartoRepository.BuscarPorId(id);
            return PartialView("_Atualizar", quarto);
        }

        public PartialViewResult AbrirDetalhes(Guid id)
        {
            ViewBag.Categorias = categoriaRepository.BuscarTodos();
            var quarto = quartoRepository.BuscarPorId(id);
            return PartialView("_Detalhes", quarto);
        }

        public PartialViewResult AbrirModalExcluir(Guid id)
        {
            var quarto = quartoRepository.BuscarPorId(id);
            return PartialView("_Excluir", quarto);
        }
        
        public PartialViewResult BuscarQuarto(string textoDigitado, int pagina = 1)
        {
            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var quartos = quartoRepository.BuscarTodosPorFiltro(textoDigitado).Where(x => !x.Excluido).ToList();
            var lista = new List<Models.Quarto>();
            lista.AddRange(quartos);
            return PartialView("_PartialTabela", lista.ToPagedList(pagina, itensPorPagina));
        }
    }
}