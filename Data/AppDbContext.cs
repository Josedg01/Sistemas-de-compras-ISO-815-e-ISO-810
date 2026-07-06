using Microsoft.EntityFrameworkCore;
using SistemaDeCompras.Models.Entities;

namespace SistemaDeCompras.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<UnidadMedida> UnidadesMedida => Set<UnidadMedida>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Articulo> Articulos => Set<Articulo>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<OrdenCompra> OrdenesCompra => Set<OrdenCompra>();
    public DbSet<OrdenCompraDetalle> DetallesOrdenCompra => Set<OrdenCompraDetalle>();
    public DbSet<AsientoContable> AsientosContables => Set<AsientoContable>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departamento>(e =>
        {
            e.HasIndex(d => d.Nombre).IsUnique();
        });

        modelBuilder.Entity<UnidadMedida>(e =>
        {
            e.HasIndex(u => u.Descripcion).IsUnique();
        });

        modelBuilder.Entity<Proveedor>(e =>
        {
            e.HasIndex(p => p.CedulaRnc).IsUnique();
            e.Property(p => p.CedulaRnc).HasMaxLength(20);
        });

        modelBuilder.Entity<Articulo>(e =>
        {
            e.Property(a => a.Existencia).HasColumnType("decimal(18,2)");
            e.HasOne(a => a.UnidadMedida)
                .WithMany(u => u.Articulos)
                .HasForeignKey(a => a.UnidadMedidaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Empleado>(e =>
        {
            e.HasOne(emp => emp.Departamento)
                .WithMany(d => d.Empleados)
                .HasForeignKey(emp => emp.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrdenCompra>(e =>
        {
            e.HasKey(o => o.Numero);
            e.Property(o => o.Numero).ValueGeneratedOnAdd();

            e.HasOne(o => o.Proveedor)
                .WithMany(p => p.OrdenesCompra)
                .HasForeignKey(o => o.ProveedorId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.Departamento)
                .WithMany(d => d.OrdenesCompra)
                .HasForeignKey(o => o.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.Empleado)
                .WithMany(emp => emp.OrdenesCompra)
                .HasForeignKey(o => o.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrdenCompraDetalle>(e =>
        {
            e.Property(d => d.Cantidad).HasColumnType("decimal(18,2)");
            e.Property(d => d.CostoUnitario).HasColumnType("decimal(18,2)");

            e.HasOne(d => d.OrdenCompra)
                .WithMany(o => o.Detalles)
                .HasForeignKey(d => d.OrdenCompraNumero)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(d => d.Articulo)
                .WithMany(a => a.DetallesOrdenCompra)
                .HasForeignKey(d => d.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(d => d.UnidadMedida)
                .WithMany()
                .HasForeignKey(d => d.UnidadMedidaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AsientoContable>(e =>
        {
            e.Property(a => a.MontoAsiento).HasColumnType("decimal(18,2)");
            e.Property(a => a.CuentaContable).HasMaxLength(30);

            e.HasOne(a => a.OrdenCompra)
                .WithMany(o => o.AsientosContables)
                .HasForeignKey(a => a.OrdenCompraNumero)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
