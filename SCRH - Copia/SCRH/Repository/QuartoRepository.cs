using SCRH.Context;
using SCRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCRH.Repository
{
    public class QuartoRepository : BaseRepositorio<Quarto>
    {
        public QuartoRepository()
        {
        }

        public QuartoRepository(ConexaoBanco db) : base(db)
        {
        }

        public Quarto BuscarPorId(Guid id)
        {
            return DbSet.Find(id);
        }
        
        public IEnumerable<Quarto> BuscarTodos()
        {
            return DbSet.ToList();
        }

        public IEnumerable<Quarto> BuscarTodosPorFiltro(string textoDigitado)
        {
            if (string.IsNullOrEmpty(textoDigitado))
            {
                return BuscarTodos();
            }
            int.TryParse(textoDigitado, out int id);
            return DbSet.Where(x => x.Categoria.Nome.Contains(textoDigitado) || x.NumeroDoQuarto == id || x.Reservas.Any(c => c.Usuario.Nome.Contains(textoDigitado)|| c.Usuario.Documento.Contains(textoDigitado))).ToList();
        }

        public override Quarto Adicionar(Quarto entity)
        {
            var categoria = new CategoriaRepository().BuscarPorNome(entity.Categoria?.Nome);
            if (categoria != null)
            {
                entity.Categoria = null;
                entity.CategoriaId = categoria.Id;
            }
            return base.Adicionar(entity);
        }

        public override Quarto Atualizar(Quarto entity)
        {
            new CategoriaRepository().AdicionarSeNaoExiste(entity.Categoria);
            entity.CategoriaId = entity.Categoria.Id;
            return base.Atualizar(entity);
        }
    }
}