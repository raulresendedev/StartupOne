using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StartupOne.Models;

namespace StartupOne.Mapping
{
    public class EventosPendentesMap : IEntityTypeConfiguration<EventosPendentes>
    {
        public void Configure(EntityTypeBuilder<EventosPendentes> builder)
        {
            builder.ToTable("T_STO_EVENTOS_PENDENTES");

            builder.HasKey(u => u.IdEventoPendente);

            builder.Property(u => u.IdEventoPendente)
                .HasColumnName("CD_EVENTO_PENDENTE");

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasColumnName("NM_EVENTO");

            builder.Property(u => u.Categoria)
                   .HasColumnName("DS_CATEGORIA");

            builder.Property(u => u.Status)
                   .HasColumnName("DS_STATUS");

            builder.Property(u => u.Prioridade)
                .HasColumnName("ST_PRIORIDADE");

            builder.Property(u => u.TempoEstimado)
                   .HasColumnName("NM_TEMPO_ESTIMADO");

            builder.HasOne(x => x.Usuario)
                   .WithMany(u => u.EventosPendentes)
                   .HasForeignKey(x => x.IdUsuario)
                   .IsRequired();
        }
    }
}
