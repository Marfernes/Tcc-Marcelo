using SCRH.Helpers;
using SCRH.Models;
using SCRH.Repository;
using SCRH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SCRH.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsuarioRepository usuarioRepository;

        public HomeController()
        {
            usuarioRepository = new UsuarioRepository();
        }

        public ActionResult Index(DateTime? dataInicio, DateTime? dataFim)
        {
            dataInicio = dataInicio ?? DateTime.Now;
            dataFim = dataFim ?? DateTime.Now;
            if (dataInicio?.Date<DateTime.Now.Date)
            {
                dataInicio = DateTime.Now;
            }
            if (dataFim?.Date < DateTime.Now.Date)
            {
                dataFim = DateTime.Now;
            }
            if (dataFim<dataInicio)
            {
                dataFim = dataInicio;
            }
            var quartos = new QuartoRepository().BuscarTodos().Where(x=>!x.Excluido);
            quartos = quartos.Where(x => x.IsDisponivel(dataInicio.Value, dataFim.Value));
            ViewBag.De = dataInicio;
            ViewBag.Ate = dataFim;
            return View(quartos);
        }

        public PartialViewResult BuscarQuartos(DateTime? dataInicio, DateTime? dataFim)
        {
            dataInicio = dataInicio ?? DateTime.Now;
            dataFim = dataFim ?? DateTime.Now;
            if (dataInicio?.Date < DateTime.Now.Date)
            {
                dataInicio = DateTime.Now;
            }
            if (dataFim?.Date < DateTime.Now.Date)
            {
                dataFim = DateTime.Now;
            }

            if (dataFim < dataInicio)
            {
                dataFim = dataInicio;
            }
            var quartos = new QuartoRepository().BuscarTodos().Where(x => !x.Excluido);
            quartos = quartos.Where(x => x.IsDisponivel(dataInicio.Value, dataFim.Value));
            ViewBag.De = dataInicio;
            ViewBag.Ate = dataFim;
            return PartialView("_PartialTabela", quartos);
        }

        public PartialViewResult AbrirModalLogin()
        {
            return PartialView("_ModalLogin");
        }

        [HttpPost]
        public ActionResult Cadastrar(Usuario usuario)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index").Error("Erro ao realizar cadastro");
            if (usuarioRepository.BuscarPorLogin(usuario.Login) != null) return RedirectToAction("Index").Error("Este login já está em uso");
            usuario.CriarUsuario(usuario.Senha);
            usuarioRepository.Adicionar(usuario);
            var usuarioCadastrado = usuarioRepository.BuscarPorLogin(usuario.Login);
            var identity = new ClaimsIdentity(new[]
            {
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider","ASP.NET Identity"),
                new Claim(ClaimTypes.NameIdentifier, usuarioCadastrado.UsuarioId.ToString()),
                new Claim(ClaimTypes.Authentication, usuarioCadastrado.Login),
                new Claim(ClaimTypes.Name, usuarioCadastrado.Nome),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(TipoUsuario), usuarioCadastrado.TipoUsuario)),
            }, "ApplicationCookie");
            Request.GetOwinContext().Authentication.SignIn(identity);
            return RedirectToAction("Index", "Home", new { area = "User" }).Success($"Usuário Cadastrado, Bem vindo {usuarioCadastrado.Nome}");
        }

        [HttpPost]
        public ActionResult Entrar(Usuario usuarioTela)
        {
            try
            {
                if (usuarioTela == null) return RedirectToAction("Index").Error("Preencha todos os campos!");
                var usuario = usuarioRepository.BuscarPorLogin(usuarioTela.Login);
                if (usuario == null) return RedirectToAction("Index").Error("Este usuário não existe!");
                if (usuario.Ativo != true) return RedirectToAction("Index").Error("Este usuário está desativado!");
                if (usuario.Senha != Hash.GerarHash(usuarioTela.Senha)) return RedirectToAction("Index").Error("Senha incorreta!");

                var identity = new ClaimsIdentity(new[]
                {
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider","ASP.NET Identity"),
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Authentication, usuario.Login),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(TipoUsuario), usuario.TipoUsuario)),
            }, "ApplicationCookie");
                Request.GetOwinContext().Authentication.SignIn(identity);

                if (usuario.TipoUsuario == TipoUsuario.Gerente)
                    return RedirectToAction("Index", "Home", new { area = "Gerente" }).Information($"Bem vindo {usuario.Nome}");
                else if (usuario.TipoUsuario == TipoUsuario.Recepcionista)
                    return RedirectToAction("Index", "Home", new { area = "Recepcao" }).Information($"Bem vindo {usuario.Nome}");
                else
                    return RedirectToAction("Index", "Home", new { area = "User" }).Information($"Bem vindo {usuario.Nome}");
            }
            catch (Exception)
            {
                return RedirectToAction("Index").Error("Erro ao fazer login!");
            }
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}