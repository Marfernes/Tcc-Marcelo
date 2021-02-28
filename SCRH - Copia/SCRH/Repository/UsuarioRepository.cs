using SCRH.Context;
using SCRH.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SCRH.Repository
{
    public class UsuarioRepository : BaseRepositorio<Usuario>
    {
        public UsuarioRepository()
        {
        }

        public UsuarioRepository(ConexaoBanco db) : base(db)
        {
        }

        public Usuario BuscarPorId(Guid usuarioId)
        {
            return DbSet.FirstOrDefault(c => c.UsuarioId == usuarioId);
        }

        public Usuario BuscarPorLogin(string login)
        {
            return DbSet.FirstOrDefault(c => (c.Login == login || c.Email == login) && !c.Excluido);
        }

        public IEnumerable<Usuario> BuscarTodos()
        {
            return DbSet.OrderBy(x => x.Nome).ToList();
        }

        public IEnumerable<Usuario> BuscarTodosPorFiltro(string textoDigitado)
        {
            if (string.IsNullOrEmpty(textoDigitado))
            {
                return BuscarTodos();
            }
            return DbSet.Where(x => x.Nome.Contains(textoDigitado)||x.Documento.Contains(textoDigitado)).OrderBy(x => x.Nome).ToList();
        }

        internal IEnumerable<Usuario> Buscar(TipoUsuario tipo)
        {
            return DbSet.Where(x => x.TipoUsuario == tipo).OrderBy(x => x.Nome).ToList();
        }
    }
}