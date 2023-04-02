using Microsoft.EntityFrameworkCore;
using StartupOne.Models;

namespace StartupOne.Mapping
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        public DataBaseContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle("Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = oracle.fiap.com.br)(PORT = 1521)))(CONNECT_DATA = (SID = orcl)));Persist Security Info=True;User ID=RM92916;Password=270300;Pooling=True;Connection Timeout=60;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new EventosMarcadosMap());
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<EventosMarcados> EventosMarcados { get; set; }

    }
}
