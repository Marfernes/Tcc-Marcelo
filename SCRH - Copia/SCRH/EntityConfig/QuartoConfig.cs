using SCRH.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SCRH.EntityConfig
{
    public class QuartoConfig : EntityTypeConfiguration<Quarto>
    {
        public QuartoConfig()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(nameof(Quarto));
        }
    }
}