namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Experiencia")]
    public partial class Experiencia
    {
        public int id { get; set; }

        public int Usuario_id { get; set; }

        public byte Tipo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Desde { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Hasta { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Descripcion { get; set; }

        public virtual Usuario Usuario { get; set; }

        public List<Experiencia> Listado(int tipo, int usuario_id, Filtro filtro)
        {
            var lista = new List<Experiencia>();
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                     var consulta = from t in bbdd.Experiencia.Where(t => t.Tipo == tipo)
                                   .Where(t => t.Usuario_id == usuario_id).OrderBy(t=>t.Desde)
                                   select t;

                    //aplicar filtros
                    if (filtro.porNombre != null)
                        consulta = consulta.Where(t => t.Nombre.Contains(filtro.porNombre));
                    if (filtro.porTitulo != null)
                        consulta = consulta.Where(t => t.Titulo.Contains(filtro.porTitulo));
                    if (filtro.nombreOrderBy == "Desc")
                        consulta = consulta.OrderByDescending(t => t.Nombre);
                    if (filtro.nombreOrderBy == "Asc")
                        consulta = consulta.OrderBy(t => t.Nombre);
                    if (filtro.tituloOrderBy == "Desc")
                        consulta = consulta.OrderByDescending(t => t.Titulo);
                    if (filtro.tituloOrderBy == "Asc")
                        consulta = consulta.OrderBy(t => t.Titulo);
                    if (filtro.desdeOrderBy == "Desc")
                        consulta = consulta.OrderByDescending(t => t.Desde);
                    if (filtro.desdeOrderBy == "Asc")
                        consulta = consulta.OrderBy(t => t.Desde);
                    if (filtro.hastaOrderBy == "Desc")
                        consulta = consulta.OrderByDescending(t => t.Hasta);
                    if (filtro.hastaOrderBy == "Asc")
                        consulta = consulta.OrderBy(t => t.Hasta);

                    //preparar lista
                    foreach (var exp in consulta)
                    {
                        lista.Add(exp);
                    }
                }
            }
            catch (Exception)
            {

                return lista;
            }
            return lista;
        }
        public Experiencia ObtenerExperiencia(int id)
        {
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    var experiencia = bbdd.Experiencia.Where(e => e.id == id).SingleOrDefault();
                    return experiencia;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarExperiencia()
        {
            bool result = false;
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                   if (this.id == 0)//nuevo regitro
                    {
                        bbdd.Entry(this).State = EntityState.Added;
                        bbdd.SaveChanges();
                        result = true;
                    }
                    else //modificacion registro
                    {
                        bbdd.Entry(this).State = EntityState.Modified;
                        bbdd.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {

                return result;
            }
            return result;
        }

        public bool EliminaExperiencia(int id)
        {
            bool result = false;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    var experiencia = bbdd.Experiencia.Where(e => e.id == id).SingleOrDefault();
                    bbdd.Entry(experiencia).State = EntityState.Deleted;
                    bbdd.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                return result;
            }
            return result;
        }
    }
   
}
