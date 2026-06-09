using Microsoft.EntityFrameworkCore;
using BibliotecaV1.Models;

namespace BibliotecaV1.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================
            // USUARIO
            // ==========================
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.NomeCompleto)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(u => u.Senha)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Ativo)
                      .HasDefaultValue(true);

                entity.HasIndex(u => u.Email)
                      .IsUnique();
            });

            // ==========================
            // LIVRO
            // ==========================
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.NomeLivro)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(l => l.Autor)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(l => l.Categoria)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(l => l.QuantidadeEstoque)
                      .IsRequired();

                entity.Property(l => l.FaixaEtariaPermitida)
                      .IsRequired();

                entity.Property(l => l.AnoPublicacao)
                      .IsRequired();
            });

            // ==========================
            // EMPRESTIMO
            // ==========================
            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DataEmprestimo)
                      .IsRequired();

                entity.Property(e => e.DataPrevistaDevolucao)
                      .IsRequired();

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.Multa)
                      .HasPrecision(10, 2);

      
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Emprestimos)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);

          
                entity.HasOne(e => e.Livro)
                      .WithMany(l => l.Emprestimos)
                      .HasForeignKey(e => e.LivroId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}