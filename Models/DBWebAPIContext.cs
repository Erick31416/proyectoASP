using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi.Models
{
    public partial class dbWebAPIContext : DbContext
    {
        public dbWebAPIContext()
        {
        }

        public dbWebAPIContext(DbContextOptions<dbWebAPIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetallesPedido> DetallesPedido { get; set; }
        public virtual DbSet<Negocios> Negocios { get; set; }
        public virtual DbSet<Pedidos> Pedidos { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<ProductosTienda> ProductosTienda { get; set; }
        public virtual DbSet<Tipos> Tipos { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=dbWebAPI;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetallesPedido>(entity =>
            {
                entity.HasKey(e => e.IdDetalle);

                entity.Property(e => e.PedidosIdPedido).HasColumnName("Pedidos_IdPedido");

                entity.Property(e => e.ProductosIdProducto).HasColumnName("Productos_IdProducto");

                entity.HasOne(d => d.PedidosIdPedidoNavigation)
                    .WithMany(p => p.DetallesPedido)
                    .HasForeignKey(d => d.PedidosIdPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetallesPedido_Pedidos_IdPedido");

                entity.HasOne(d => d.ProductosIdProductoNavigation)
                    .WithMany(p => p.DetallesPedido)
                    .HasForeignKey(d => d.ProductosIdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetallesPedido_Productos_IdProducto");
            });

            modelBuilder.Entity<Negocios>(entity =>
            {
                entity.HasKey(e => e.IdNegocio);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.Lat).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.Lng).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TiposIdTipo).HasColumnName("Tipos_IdTipo");

                entity.HasOne(d => d.TiposIdTipoNavigation)
                    .WithMany(p => p.Negocios)
                    .HasForeignKey(d => d.TiposIdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Negocios_Tipos_IdTipo");
            });

            modelBuilder.Entity<Pedidos>(entity =>
            {
                entity.HasKey(e => e.IdPedido);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Intent)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PaypalStatus)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Productos>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Precio).HasColumnType("decimal(6, 2)");
            });

            modelBuilder.Entity<ProductosTienda>(entity =>
            {
                entity.HasKey(e => e.IdProducto);

                entity.Property(e => e.Descripcion).HasMaxLength(200);

                entity.Property(e => e.Imagen)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Tipos>(entity =>
            {
                entity.HasKey(e => e.IdTipo);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
