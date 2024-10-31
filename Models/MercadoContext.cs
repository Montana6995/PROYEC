using Microsoft.EntityFrameworkCore;
using MVC.Models;

public class MercadoContext : DbContext
{
    public DbSet<Puesto> Puestos { get; set; }
    public DbSet<Personal> Personales { get; set; }
    public DbSet<TipoServicio> TiposServicio { get; set; }
    public DbSet<Pago> Pagos { get; set; }

    public MercadoContext(DbContextOptions<MercadoContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Claves primarias
        modelBuilder.Entity<Puesto>().HasKey(p => p.IdPuesto);
        modelBuilder.Entity<Personal>().HasKey(p => p.IdPersonal);
        modelBuilder.Entity<TipoServicio>().HasKey(t => t.IdTipoServicio);
        modelBuilder.Entity<Pago>().HasKey(pg => pg.IdPago);

        // Relaciones
        modelBuilder.Entity<Pago>()
            .HasOne(pg => pg.Puesto)
            .WithMany()
            .HasForeignKey(pg => pg.IdPuesto);

        modelBuilder.Entity<Pago>()
            .HasOne(pg => pg.Personal)
            .WithMany()
            .HasForeignKey(pg => pg.IdPersonal);

        modelBuilder.Entity<Pago>()
            .HasOne(pg => pg.TipoServicio)
            .WithMany()
            .HasForeignKey(pg => pg.IdTipoServicio);
    }
}
