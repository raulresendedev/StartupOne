using StartupOne.Models;
using System.Data.Entity.ModelConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StartupOne.Mapping
{
    public class EventosMarcadosMap : IEntityTypeConfiguration<EventosMarcados>
    {
        public void Configure(EntityTypeBuilder<EventosMarcados> builder)
        {
            builder.ToTable("T_STO_EVENTOS_MARCADOS");

            builder.HasKey(u => u.IdEventoMarcado);


            builder.Property(u => u.IdEventoMarcado)
                .HasColumnName("CD_EVENTO_MARCADO");

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasColumnName("NM_EVENTO");

            builder.Property(u => u.Inicio)
                .IsRequired()
                .HasColumnName("DT_INICIO");   

            builder.Property(u => u.Fim)
                .IsRequired()
                .HasColumnName("DT_FIM");

            builder.Property(u => u.Recorrente)
                .HasColumnName("ST_RECORRENTE");

            builder.Property(u => u.Categoria)
                .HasColumnName("DS_CATEGORIA");

            builder.HasOne(x => x.Usuario)
                   .WithMany(u => u.EventosMarcados)
                   .HasForeignKey(x => x.IdUsuario)
                   .IsRequired();
        }
    }
}
