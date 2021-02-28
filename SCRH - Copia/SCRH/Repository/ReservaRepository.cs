using SCRH.Context;
using SCRH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCRH.Repository
{
    public class ReservaRepository: BaseRepositorio<Reserva>
    {
        private readonly Quarto quarto;
        
        public ReservaRepository(Quarto quarto):base()
        {
            this.quarto = quarto;
        }

        public ReservaRepository(Quarto quarto, ConexaoBanco db) : base(db)
        {
            this.quarto = quarto;
        }

        public override Reserva Adicionar(Reserva entity)
        {
            entity.Valor = quarto.ValorDiaria;
            quarto.Reservas.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public override Reserva Atualizar(Reserva entity)
        {
            entity.Valor = quarto.ValorDiaria;
            return base.Atualizar(entity);
        }

        internal Reserva Buscar(Guid id)
        {
            return quarto.Reservas.FirstOrDefault(x => x.Id == id);
        }
        internal IEnumerable<Reserva> Buscar()
        {
            return quarto.Reservas;
        }
    }
}