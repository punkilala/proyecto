namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("TablaDato")]
    public partial class TablaDato
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Relacion { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Valor { get; set; }

        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        public int Orden { get; set; }

        public int conexion()
        {
            int result;
            using(var bbdd= new ProyectoContexto())
            {
                result = bbdd.TablaDato.Count();
            }
            return result;
        }
        public List<TablaDato> Listado(string relacion)
        {
            var datos = new List<TablaDato>();
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    datos = bbdd.TablaDato.OrderBy(tb=>tb.Descripcion)
                        .Where(tb => tb.Relacion == relacion).ToList();
                }
            }
            catch (Exception)
            {

                return datos;
            }
            return datos;
        }
    }
}
