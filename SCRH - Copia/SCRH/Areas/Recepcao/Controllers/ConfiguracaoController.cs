using SCRH.Helpers;
using SCRH.Models;
using SCRH.Repository;
using SCRH.Utils;
using System.Web.Mvc;

namespace SCRH.Areas.Recepcao.Controllers
{
    [Permissoes(Roles = "Recepcionista")]
    public class ConfiguracaoController : Controller
    {
        private readonly UsuarioRepository usuarioRepository;

        public ConfiguracaoController()
        {
            usuarioRepository = new UsuarioRepository();
        }

        public ActionResult AlterarDados()
        {
            return View(usuarioRepository.BuscarPorId(Account.UsuarioId));
        }

        [HttpPost]
        public ActionResult AlterarDados(Usuario usuario)
        {
            var usuarioAtual = usuarioRepository.BuscarPorId(usuario.UsuarioId);
            usuarioAtual.AlterarUsuario(usuario);
            usuarioRepository.Atualizar(usuarioAtual);
            return RedirectToAction("Index", "Home", new { area = "Recepcao" }).Success();
        }
    }
}