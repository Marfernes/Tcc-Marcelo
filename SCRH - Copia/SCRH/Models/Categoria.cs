
using SCRH.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCRH.Models
{
    public class Categoria {

		private Guid id;
		private string nome;

		public Categoria(){
            this.Id = Guid.NewGuid();
		}

        public Guid Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }

    }

}