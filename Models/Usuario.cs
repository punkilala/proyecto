namespace Models
{
    using Helper;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.Entity;
    using System.Web;
    using System.IO;

    [Table("Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Experiencia = new HashSet<Experiencia>();
            Habilidad = new HashSet<Habilidad>();
            Testimonio = new HashSet<Testimonio>();
        }

        public int id { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(100,ErrorMessage ="Maximo 100 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage ="El campo es requerido")]
        [StringLength(100,ErrorMessage ="Maximo 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(10,ErrorMessage ="Maximo 10 caracteres")]
        public string Password { get; set; }

        [Column(TypeName = "text")]
        public string Direccion { get; set; }

        [StringLength(50)]
        public string Ciudad { get; set; }

        public int? Pais_id { get; set; }

        [StringLength(32,ErrorMessage ="Maximo 32 caracteres")]
        public string Telefono { get; set; }

        [StringLength(100)]
        public string Facebook { get; set; }

        [StringLength(100)]
        public string Twitter { get; set; }

        [StringLength(100)]
        public string YouTube { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(32, ErrorMessage = "Maximo 32 caracteres")]
        public string PassActual { get; set; }
   

        [NotMapped]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(32, ErrorMessage = "Maximo 32 caracteres")]
        public string PassNuevo { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(32, ErrorMessage = "Maximo 10 caracteres")]
        [Compare("PassNuevo", ErrorMessage ="Nueva Contraseña y Repetir Contraseña no coinciden")]
        public string PassRepetir { get; set; }


        public virtual ICollection<Experiencia> Experiencia { get; set; }
        public virtual ICollection<Habilidad> Habilidad { get; set; }
        public virtual ICollection<Testimonio> Testimonio { get; set; }



        public Usuario Acceder(string email, string password)
        {

            Usuario usuario = (Usuario) null;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                    //convertir password a md5
                    password = HashHelper.MD5(password);
                     usuario = bbdd.Usuario.Where(u => u.Email == email)
                        .Where(u => u.Password == password).SingleOrDefault();
                    if (usuario != null)
                    {
                        SesionHelper.AddUserASesion(usuario.id.ToString(), 
                            usuario.Nombre, usuario.Foto);
                    }
                }
            }
            catch (Exception)
            {

                return usuario;
            }
            return usuario;
        }
        public Usuario UsuarioActivo(int id)
        {
            Usuario activo = null;
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    activo = bbdd.Usuario.Where(ua => ua.id == id).SingleOrDefault();
                }
            }
            catch (Exception)
            {

                return activo;
            }
            return activo;
        }
        public bool GuardarUsuario()
        {
            bool result = false;
            try
            {
                using(var bbdd= new ProyectoContexto())
                {

                    var usuario = bbdd.Entry(this);
                    usuario.State = EntityState.Modified;

                    //El campo password decimos que no se modifica ya que no se lo pasamos.
                    usuario.Property(u => u.Password).IsModified = false;
 
                    //Para que no de error al grabar puesto que no pasamos Password
                    bbdd.Configuration.ValidateOnSaveEnabled = false;
                    bbdd.SaveChanges();

                    HttpContext.Current.Session["nombre"] = this.Nombre;
                    result = true;
                }
            }
            catch (Exception)
            {

                return result;
            }

            return result;
        }
        public bool GuardarFoto(int id, string nuevaFoto)
        {
            bool result = false;
            try
            {
                using (var bbdd = new ProyectoContexto())
                {
                   var usuario = bbdd.Usuario.Where(u => u.id == id).SingleOrDefault();

                    string fotoAntigua = usuario.Foto;
                    usuario.Foto = nuevaFoto;

                    bbdd.Entry(usuario).State = EntityState.Modified;
                    bbdd.Configuration.ValidateOnSaveEnabled = false;
                    bbdd.SaveChanges();
                    result = true;
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads");
                    string fullPath = Path.Combine(path, fotoAntigua);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    HttpContext.Current.Session["foto"] = nuevaFoto;
                }
            }
            catch (Exception ex)
            {

                return result;
            }
            return result;
        }
        public int CambiarPassword(int id, string actualPass, string nuevoPass)
        {
            int result = -1;
            try
            {
                using(var bbdd= new ProyectoContexto())
                {
                    var usuario = bbdd.Usuario.Where(u => u.id == id).SingleOrDefault();
                    actualPass = HashHelper.MD5(actualPass);
                    if (actualPass == usuario.Password)
                    {
                        usuario.Password = HashHelper.MD5(nuevoPass);
                        bbdd.Configuration.ValidateOnSaveEnabled = false;
                        bbdd.SaveChanges();
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
    }

}
