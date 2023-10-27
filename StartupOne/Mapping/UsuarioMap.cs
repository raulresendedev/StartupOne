using StartupOne.Models;
using System.Data.Entity.ModelConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StartupOne.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("T_STO_USUARIO");

            builder.HasKey(u => u.IdUsuario);


            builder.Property(u => u.IdUsuario)
                .HasColumnName("CD_USUARIO");
            builder.Property(u => u.Nome)
                .IsRequired()
                .HasColumnName("NM_USUARIO");
            builder.Property(u => u.Email).IsRequired()
                .IsRequired()
                .HasColumnName("DS_EMAIL");
            builder.Property(u => u.Password).IsRequired()
                .IsRequired()
                .HasColumnName("HC_SENHA");
        }
    }
}
