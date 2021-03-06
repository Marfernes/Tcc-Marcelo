﻿using PagedList;
using SCRH.Helpers;
using SCRH.Models;
using SCRH.Repository;
using SCRH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SCRH.Areas.Gerente.Controllers
{
    [Permissoes(Roles = "Gerente")]
    public class UsuarioController : Controller
    {

        private readonly UsuarioRepository usuarioRepository;

        public UsuarioController()
        {
            usuarioRepository = new UsuarioRepository();
        }

        public ActionResult Index(int pagina = 1)
        {
            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var usuarios = usuarioRepository.BuscarTodos().Where(x => !x.Excluido).ToList();
            var lista = new List<Usuario>();
            lista.AddRange(usuarios);
            return View(lista.ToPagedList(pagina, itensPorPagina));
        }

        [HttpPost]
        public ActionResult Adicionar(Usuario usuario)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", new { area = "Gerente" }).Error("Erro ao cadastrar usuário.");
            usuario.Senha = Hash.GerarHash(usuario.Senha);
            usuarioRepository.Adicionar(usuario);
            return RedirectToAction("Index", new { area = "Gerente" }).Success("Usuário cadastrado com sucesso.");
        }

        [HttpPost]
        public ActionResult Atualizar(Usuario usuario)
        {
            var usuarioAtual = usuarioRepository.BuscarPorId(usuario.UsuarioId);
            usuarioAtual.AlterarUsuario(usuario);
            usuarioRepository.Atualizar(usuarioAtual);
            return RedirectToAction("Index", new { area = "Gerente" }).Success("Usuário atualizado com sucesso.");
        }

        [HttpPost]
        public ActionResult Excluir(Usuario usuario)
        {
            var usuarioAtual = usuarioRepository.BuscarPorId(usuario.UsuarioId);
            usuarioAtual.ExcluirUsuario();
            usuarioRepository.Excluir(usuarioAtual);
            return RedirectToAction("Index", new { area = "Gerente" }).Success("Usuário excluído com sucesso.");
        }

        public PartialViewResult AbrirModalAdicionar()
        {
            return PartialView("_Adicionar");
        }

        public PartialViewResult AbrirModalAtualizar(Guid usuarioId)
        {
            var usuario = usuarioRepository.BuscarPorId(usuarioId);
            return PartialView("_Atualizar", usuario);
        }

        public PartialViewResult AbrirModalExcluir(Guid usuarioId)
        {
            var usuario = usuarioRepository.BuscarPorId(usuarioId);
            return PartialView("_Excluir", usuario);
        }

        public ActionResult Ativo(Guid usuarioId, int pagina)
        {
            var usuarioAtual = usuarioRepository.BuscarPorId(usuarioId);
            usuarioAtual.AtivarOuDesativar();
            usuarioRepository.Atualizar(usuarioAtual);
            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var usuarios = usuarioRepository.BuscarTodos().Where(x => !x.Excluido).ToList(); ;
            var lista = new List<Usuario>();
            lista.AddRange(usuarios);
            return PartialView("_PartialTabela", lista.ToPagedList(pagina, itensPorPagina));
        }

        public PartialViewResult BuscarUsuario(string textoDigitado, int pagina = 1)
        {
            TempData["Pagina"] = pagina;
            int itensPorPagina = 5;
            var usuarios = usuarioRepository.BuscarTodosPorFiltro(textoDigitado).Where(x => !x.Excluido).ToList();
            var lista = new List<Usuario>();
            lista.AddRange(usuarios);
            return PartialView("_PartialTabela", lista.ToPagedList(pagina, itensPorPagina));
        }
    }
}


