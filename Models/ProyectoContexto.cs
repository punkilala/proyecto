namespace Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ProyectoContexto : DbContext
    {
        public ProyectoContexto()
            : base("name=ProyectoContexto")
        {
        }

        public virtual DbSet<Experiencia> Experiencia { get; set; }
        public virtual DbSet<Habilidad> Habilidad { get; set; }
        public virtual DbSet<TablaDato> TablaDato { get; set; }
        public virtual DbSet<Testimonio> Testimonio { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Experiencia)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Habilidad)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Testimonio)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.Usuario_id)
                .WillCascadeOnDelete(false);
        }
    }
}
