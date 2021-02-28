

using SCRH.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCRH.Models
{
    public class Quarto {

		private Guid id;
		private int numeroDoQuarto;
		private decimal valorDiaria;
        private Categoria categoria;
        private ICollection<Reserva> reservas;

        public Guid Id { get => id; set => id = value; }
        [Display(Name = "Nº do quarto"), Range(1, double.PositiveInfinity, ErrorMessage = "{0} deve ser maior ou igual a {1}")]
        public int NumeroDoQuarto { get => numeroDoQuarto; set => numeroDoQuarto = value; }


        [Display(Name = "Valor da diária"), Range(0.1,double.PositiveInfinity,ErrorMessage ="{0} deve ser maior ou igual a {1}")]
        public decimal ValorDiaria { get => valorDiaria; set => valorDiaria = value; }
        public virtual Categoria Categoria { get => categoria; set => categoria = value; }
        public virtual ICollection<Reserva> Reservas { get => reservas; set => reservas = value; }

        public bool Excluido { get; set; }


        public override string ToString()
        {
            return string.Format("{0} nº{1}, {2}/dia", (Categoria?.Nome), this.NumeroDoQuarto, this.ValorDiaria.ToString("C2"));
        }

        public Quarto(){
            this.Id = Guid.NewGuid();
		}

        [ForeignKey("Categoria"), Display(Name = "Categoria"), Required(ErrorMessage ="{0} é obrigatório")]
        public Guid CategoriaId { get; set; }


        internal void AlterarQuarto(Quarto quarto)
        {
            NumeroDoQuarto = quarto.NumeroDoQuarto;
            ValorDiaria = quarto.ValorDiaria;
        }

        public bool IsDisponivel()
        {
            return IsDisponivel(DateTime.Now);
        }

        public bool IsDisponivel(DateTime dateTime)
        {
            bool result = false;
            if (!Excluido)
            {
                result = !this.Reservas.Where(x=>!x.Excluido).Any(x => !x.IsLivre(dateTime));
            }
            return result;
        }

        public bool IsCheckin()
        {
            return IsCheckin(DateTime.Now);
        }
        public bool IsCheckin(DateTime dateTime)
        {
            return this.Reservas.Where(x => !x.IsLivre(dateTime) && !x.Excluido).Any(x => x.IsCheckin());
        }

        public bool IsDisponivel(DateTime dateTime, DateTime dataFinal)
        {
            bool result = false;
            if (!Excluido)
            {
                result = !this.Reservas.Where(x=>!x.Excluido).Any(x => !x.IsLivre(dateTime, dataFinal));
            }
            return result;
        }


        internal void ExcluirQuarto()
        {
            Excluido = true;
        }
    }

}