using Microsoft.EntityFrameworkCore;

namespace Clientes.Infrastructure.Leitura;

public sealed class ContextoLeitura : DbContext
{
    public DbSet<ClienteLeitura> Clientes => Set<ClienteLeitura>();
    public ContextoLeitura(DbContextOptions<ContextoLeitura> options) : base(options) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<ClienteLeitura>();
        e.ToTable("clientes_leitura");
        e.HasKey(x => x.Id);
        e.Property(x => x.NomeOuRazaoSocial).IsRequired();
        e.Property(x => x.Documento).IsRequired();
        e.Property(x => x.Email).IsRequired();
        e.HasIndex(x => x.Documento).IsUnique();
        e.HasIndex(x => x.Email).IsUnique();
    }
}