using SCRH.Context;
using SCRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCRH.Repository
{
    public class CategoriaRepository: BaseRepositorio<Categoria>
    {
        public CategoriaRepository()
        {
        }

        public CategoriaRepository(ConexaoBanco db) : base(db)
        {
        }

        public Categoria BuscarPorId(Guid id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<Categoria> BuscarTodos()
        {
            return DbSet.ToList();
        }

        public Categoria BuscarPorNome(string nome)
        {
            return DbSet.AsNoTracking().FirstOrDefault(x => x.Nome == nome);
        }

        public IEnumerable<Categoria> BuscarTodosPorFiltro(string textoDigitado)
        {
            if (string.IsNullOrEmpty(textoDigitado))
            {
                return BuscarTodos();
            }
            return DbSet.Where(x => x.Nome.Contains(textoDigitado)).ToList();
        }

        public void AdicionarSeNaoExiste(Categoria categoria)
        {
            var categoriaAtual = BuscarPorNome(categoria.Nome);
            if (categoriaAtual == null)
            {
                Adicionar(categoria);
            }
            else
            {
                categoria.Id = categoriaAtual.Id;
            }
        }
    }
}