using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCRH.Models
{
    public class Consumo
    {
		private Guid id;
		private string produto;
        private int quantidade;
		private decimal valor;

        public Consumo()
        {
            this.id = Guid.NewGuid();
        }

        public Guid Id { get => id; set => id = value; }
        [Display(Name ="Nome do produto"),Required]
        public string Produto { get => produto; set => produto = value; }
        [Display(Name ="Valor unitário"), Range(0.1,double.PositiveInfinity,ErrorMessage ="{0} deve ser maior ou igual a {1}")]
        public decimal Valor { get => valor; set => valor = value; }
        [Range(1,double.PositiveInfinity,ErrorMessage ="{0} deve ser maior ou igual a {1}")]
        public int Quantidade { get => quantidade; set => quantidade = value; }

        public decimal ValorFinal()
        {
            return this.Quantidade * this.Valor;
        }
    }
}