using SCRH.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SCRH.EntityConfig
{
    public class UsuarioConfig : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfig()
        {
            HasKey(x => x.UsuarioId);
            Property(x => x.UsuarioId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).IsRequired().HasMaxLength(250);
            Property(x => x.Login).IsRequired().HasMaxLength(50);
            Property(x => x.Senha).IsRequired();
            Property(x => x.Email).IsRequired();
            Property(x => x.EmailConfirmado);
            Property(x => x.TipoUsuario).IsRequired();
            Property(x => x.Ativo);
            Property(x => x.Excluido).IsRequired();
            Property(x => x.DataAlteracao).IsOptional();
            Property(x => x.DataRegistro).IsRequired();

            ToTable(nameof(Usuario));
        }
    }
}