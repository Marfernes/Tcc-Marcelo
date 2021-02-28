using System.ComponentModel.DataAnnotations;

namespace SCRH.Models
{
    public enum TipoUsuario
    {
        Gerente = 1,
        [Display(Name = "Usuário")]
        Usuario = 2,
        Recepcionista = 3
    }
}