using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StartupOne.Models;

namespace StartupOne.Mapping
{
    public class InteressesMap : IEntityTypeConfiguration<Interesses>
    {
        public void Configure(EntityTypeBuilder<Interesses> builder)
        {
            builder.ToTable("T_STO_INTERESSE");

            builder.HasKey(u => u.IdInteresse);

            builder.Property(u => u.IdInteresse)
                .HasColumnName("CD_INTERESSE");

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasColumnName("NM_EVENTO");

            builder.Property(u => u.Categoria)
                   .HasColumnName("DS_CATEGORIA");

            builder.Property(u => u.Status)
                   .HasColumnName("DS_STATUS");

            builder.Property(u => u.PeriodoInicio)
                    .IsRequired()
                    .HasColumnName("DT_PERIODO_INICIO");

            builder.Property(u => u.PeriodoFim)
                    .IsRequired()
                    .HasColumnName("DT_PERIODO_FIM");

            builder.Property(u => u.Prioridade)
                    .HasColumnName("ST_PRIORIDADE");

            builder.Property(u => u.TempoEstimado)
                   .HasColumnName("DT_TEMPO_ESTIMADO");

            builder.HasOne(x => x.Usuario)
                   .WithMany(u => u.EventosPendentes)
                   .HasForeignKey(x => x.IdUsuario)
                   .IsRequired();
        }
    }
}
