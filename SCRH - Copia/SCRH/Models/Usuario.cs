using SCRH.Repository;
using SCRH.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCRH.Models
{
    public class Usuario
    {
        #region Atributos
        public Guid UsuarioId { get; set; }
        [Required]
        public string Nome { get; set; }
        [Display(Name ="CPF/Cnpj")]
        public string Documento { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="{0} inválido")]
        public string Email { get; set; }
        public bool EmailConfirmado { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime DataRegistro { get; set; }

        public Usuario()
        {
            this.DataRegistro = DateTime.Now;
            this.TipoUsuario = TipoUsuario.Usuario;
            this.Ativo = true;
        }
        #endregion

        #region Bidirecional
        //public ICollection<Teste> Testes { get; set; }
        #endregion

        public override string ToString()
        {
            return string.Format("{0}, ({1})", this.Nome, this.Documento);
        }

        #region Métodos
        public void AtivarOuDesativar()
        {
            Ativo = !Ativo;
            DataAlteracao = DateTime.Now;
        }

        public void CriarUsuario(string senha)
        {
            TipoUsuario = TipoUsuario.Usuario;
            Senha = Hash.GerarHash(senha);
        }

        public void AlterarUsuario(Usuario usuario)
        {
            Nome = usuario.Nome;
            Login = usuario.Login;
            Documento = usuario.Documento;
            TipoUsuario = usuario.TipoUsuario;
            DataAlteracao = DateTime.Now;
            if (usuario.Senha != null)
                Senha = Hash.GerarHash(usuario.Senha);
        }

        public void ExcluirUsuario()
        {
            DataAlteracao = DateTime.Now;
            Excluido = true;
        }
        #endregion
    }
}