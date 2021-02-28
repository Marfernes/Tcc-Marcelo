using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



using SCRH.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SCRH.Repository;
using System.Linq;

namespace SCRH.Models {
	public class Reserva {
        private Guid id;
        private DateTime data;
        private DateTime dataFinal;
		private DateTime? dataDeEntrada;
		private DateTime? dataDeSaida;
		private decimal valor;
        private ICollection<Consumo> consumo;
        private Usuario usuario;
        [Display(Name ="Cancelada")]
        public bool Excluido { get; set; }

        public Guid Id { get => id; set => id = value; }
        [Display(Name ="Data da reserva"),Required(ErrorMessage ="{0} é obrigatório")]
        public DateTime Data { get => data; set => data = value; }
        [Display(Name ="Data final da reserva"),Required(ErrorMessage ="{0} é obrigatório")]
        public DateTime DataFinal { get => dataFinal; set => dataFinal = value; }
        [Display(Name ="Entrada")]
        public DateTime? DataDeEntrada { get => dataDeEntrada; set => dataDeEntrada = value; }
        [Display(Name ="Saida")]
        public DateTime? DataDeSaida { get => dataDeSaida; set => dataDeSaida = value; }
        [Display(Name ="Valor da diária")]
        public decimal Valor { get => valor; set => valor = value; }
        [Display(Name = "Hóspede")]
        public virtual Usuario Usuario { get => usuario; set => usuario = value; }
        public virtual ICollection<Consumo> Consumo { get => consumo; set => consumo = value; }

        [ForeignKey("Usuario"), Display(Name = "Hóspede"), Required(ErrorMessage = "{0} é obrigatório")]
        public Guid UsuarioId { get; set; }

        public Reserva(){
            this.Data = DateTime.Now;
            this.Id = Guid.NewGuid();
		}

        public bool IsCheckin()
        {
            return this.DataDeEntrada != null;
        }

        public bool IsLivre()
        {
            return IsLivre(DateTime.Now);
        }

        public decimal ValorFinal()
        {
            decimal consumo = this.Consumo?.Sum(x => x.ValorFinal()) ?? 0;
            decimal reserva = QuantidadeDiarias() * Valor;
            return consumo + reserva;
        }

        public bool IsLivre(DateTime dateTime)
        {
            bool result = false;
            if (!Excluido)
            {                
                if (dateTime.Date >= this.Data.Date && dateTime.Date <= this.DataFinal.Date)
                {
                    result = this.DataDeSaida != null && IsCheckin();
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        public bool IsLivre(DateTime dateTime, DateTime dataFim)
        {
            bool result = false;
            if (!Excluido)
            {
                for (int i = 0; i < ((dataFim.Date-dateTime.Date).TotalDays+1); i++)
                {
                    var data = dateTime.AddDays(i);
                    if (!IsLivre(data))
                    {
                        result = false;
                        break;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public int QuantidadeDiarias()
        {
            var result = (this.DataFinal.Date - this.Data.Date).TotalDays;
            if (IsCheckin()&&this.DataDeSaida!=null)
            {
                var diasCheckin = (this.DataDeSaida?.Date - this.DataDeEntrada?.Date)?.TotalDays ?? 0;
                result = diasCheckin > result ? diasCheckin : result;
            }
            return (int)(result <= 0 ? 1 : result);
        }


        public void Adicionar(Reserva reserva)
        {
            if (this.Data<this.DataFinal)
            {
                throw new Exception("Data de saída deve ser maior que a data de entrdada");
            }
        }

        public void Atualizar(Reserva reserva)
        {
            if (this.Data < this.DataFinal)
            {
                throw new Exception("Data de saída deve ser maior que a data de entrdada");
            }
        }
        public void FazerCheckin()
        {
            DataDeEntrada = DateTime.Now;
        }
        public void FazerCheckout()
        {
            DataDeSaida = DateTime.Now;
        }
        public void ExcluirReserva()
        {
            Excluido = true;
        }


        public bool IsContains(string textoDigitado)
        {
            if (string.IsNullOrEmpty(textoDigitado))
            {
                return true;
            }
            return (this.Usuario?.Nome.Contains(textoDigitado) ?? false) || (this.Usuario?.Documento.Contains(textoDigitado) ?? false);
        }

    }

}