using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCRH.Models
{
    public enum Status
    {
        Ativo = 1,
        Inativo = 2,

        [Display(Name = "Excluído")]
        Excluido = 3
    }
}