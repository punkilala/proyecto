namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Habilidad")]
    public partial class Habilidad
    {
        public int id { get; set; }

        public int Usuario_id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage ="valor entre 1 y 100")]
        public int Dominio { get; set; }

        public virtual Usuario Usuario { get; set; }

        // OPERACIONES
        public List<Habilidad> ListaHabilidades(int usuario_id, Filtro filtro)
        {
            var lista = new List<Habilidad>();
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    //  lista = bbdd.Habilidad.Where(h => h.Usuario_id == usuario_id).ToList();
                    var consulta = from h in bbdd.Habilidad
                                   .Where(h => h.Usuario_id == usuario_id)
                                   select h;
                    if (filtro.porNombre != null)
                        consulta = consulta.Where(h => h.Nombre.Contains(filtro.porNombre));
                    if (filtro.nombreOrderBy == "Desc")
                        consulta = consulta.OrderByDescending(h => h.Nombre);
                    if (filtro.nombreOrderBy == "Asc")
                        consulta = consulta.OrderBy(h => h.Nombre);
                    if (filtro.tituloOrderBy == "Desc")
                        consulta = consulta.OrderByDescending(h => h.Dominio);
                    if (filtro.tituloOrderBy == "Asc")
                        consulta = consulta.OrderBy(h => h.Dominio);

                    foreach(var registro in consulta)
                    {
                        lista.Add(registro);
                    }
                }
            }
            catch (Exception)
            {

                return lista; ;
            }
            return lista;
        }
        public Habilidad ObtenerHabilidad(int id)
        {
            try
            {
                using(var bbdd=new ProyectoContexto())
                {
                    var habilidad = bbdd.Habilidad.Where(h => h.id == id).SingleOrDefault();
                    return habilidad;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool GuardarHabilidad()
        {
            bool result = false;
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    if (this.id == 0)
                    {
                        bbdd.Entry(this).State = EntityState.Added;
                        bbdd.SaveChanges();
                        result = true;
                    }
                    else
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
        public bool EliminarHabilidad(int id)
        {
            bool result = false;
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    var habilidad = bbdd.Habilidad.Where(h => h.id == id).SingleOrDefault();
                    bbdd.Entry(habilidad).State = EntityState.Deleted;
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
