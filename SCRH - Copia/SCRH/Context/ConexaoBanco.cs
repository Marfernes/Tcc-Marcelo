using SCRH.EntityConfig;
using SCRH.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;

namespace SCRH.Context
{
    public class ConexaoBanco : DbContext
    {
        public ConexaoBanco() : base("name=ConexaoBanco") { }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>().Configure(x => x.HasColumnType("varchar"));
            modelBuilder.Configurations.Add(new UsuarioConfig());
            modelBuilder.Configurations.Add(new QuartoConfig());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.Entries)
                {
                    sb.Append($"ERROS:\n{failure.State}\n{failure.Entity.GetType().Name}");
                }
                var erro = sb.ToString();
                throw;
            }
            catch (DbUnexpectedValidationException un)
            {
                var erro = un.Message;
                throw;
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                throw;
            }
        }
    }
}