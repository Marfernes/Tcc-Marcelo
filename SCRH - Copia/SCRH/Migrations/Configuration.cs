using SCRH.Context;
using SCRH.Models;
using System;
using System.Data.Entity.Migrations;

namespace SCRH.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ConexaoBanco>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ConexaoBanco context)
        {
            context.Usuario.AddOrUpdate(u => u.Login,
                     new Usuario()
                     {
                         UsuarioId = Guid.NewGuid(),
                         Documento = "875.937.010-69",
                         Nome = "Gerente",
                         Login = "gerente",
                         Senha = "8D969EEF6ECAD3C29A3A629280E686CFC3F5D5A86AFF3CA122C923ADC6C92",//senha = 123456
                         Email = "gerente@gmail.com",
                         EmailConfirmado = true,
                         Ativo = true,
                         TipoUsuario = TipoUsuario.Gerente,
                         Excluido = false,
                         DataAlteracao = null,
                         DataRegistro = DateTime.Now.Date
                     },
                     new Usuario()
                     {
                         UsuarioId = Guid.NewGuid(),
                         Documento= "754.699.440-34",
                         Nome = "Recepcionista",
                         Login = "recepcao",
                         Senha = "8D969EEF6ECAD3C29A3A629280E686CFC3F5D5A86AFF3CA122C923ADC6C92",//senha =123456
                         Email = "recepcionista@gmail.com",
                         EmailConfirmado = true,
                         Ativo = true,
                         TipoUsuario = TipoUsuario.Recepcionista,
                         Excluido = false,
                         DataAlteracao = null,
                         DataRegistro = DateTime.Now.Date
                     },
                     new Usuario()
                     {
                         UsuarioId = Guid.NewGuid(),
                         Documento= "87.911.841/0001-39",
                         Nome = "Usuário",
                         Login = "usuario",
                         Senha = "8D969EEF6ECAD3C29A3A629280E686CFC3F5D5A86AFF3CA122C923ADC6C92",//senha =123456
                         Email = "usuario@gmail.com",
                         EmailConfirmado = true,
                         Ativo = true,
                         TipoUsuario = TipoUsuario.Usuario,
                         Excluido = false,
                         DataAlteracao = null,
                         DataRegistro = DateTime.Now.Date
                     });
        }
    }
}
