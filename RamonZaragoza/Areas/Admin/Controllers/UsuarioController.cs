using Helper;
using Models;
using RamonZaragoza.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Areas.Admin.Controllers
{
    [Autenticado]
    public class UsuarioController : Controller
    {
        private TablaDato mListadoAdicional= new TablaDato();
        private Usuario mUsuarios = new Usuario();
        private RespuestaServidor mRespuestaAjax;
        public ActionResult Index()
        {
             var paises = mListadoAdicional.Listado("pais");
            ViewBag.Pais_Id = new SelectList(paises,"valor","descripcion");
            return View(mUsuarios.UsuarioActivo(SesionHelper.GetUser()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarUsuario(Usuario modelo)
        {
            mRespuestaAjax = new RespuestaServidor();
            //Eliminamos la validación del camo Password puesto que no se la pasamos
            //por motivos de seguridad y asi poder validar el resto de campos sin que de error
            ModelState.Remove("Password");
            //Solo se utilizan para cambiar contraseña
            ModelState.Remove("PassActual");
            ModelState.Remove("PassNuevo");
            ModelState.Remove("PassRepetir");

            if (ModelState.IsValid)
            {
                bool guardado=modelo.GuardarUsuario();
                if (guardado)
                {
                    mRespuestaAjax.SetResponse(true, "Modificaciones realizadas correctmente");
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error al intentar modificar los datos");
                }
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "Error al procesar los datos");
            }
            return Json(mRespuestaAjax);
        }
        public ActionResult FotoPerfil()
        {
            return View();
        }
        public PartialViewResult _CambiarFoto()
        {
            var  usuarioActivo = mUsuarios.UsuarioActivo(SesionHelper.GetUser());
            ViewBag.Foto = usuarioActivo.Foto;
            return PartialView(usuarioActivo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult _CambiarFoto(HttpPostedFileBase Foto, Usuario modelo)
        {
            mRespuestaAjax = new RespuestaServidor();
            bool result = false;
            if (Foto == null)
            {
                mRespuestaAjax.SetResponse(false, "debe seleccionar un archivo");
            }
            else
            {
                //nombre archivo, lo renombro
                string foto = modelo.Nombre + DateTime.Now.ToString("-ddss-")
                    + Path.GetFileName(Foto.FileName);
                //subir archivo
                Foto.SaveAs(Server.MapPath("~/Uploads/" + foto));

                result = mUsuarios.GuardarFoto(modelo.id, foto);
                if (result)
                {
                    mRespuestaAjax.funcion = "RecargarFoto();";
                    mRespuestaAjax.SetResponse(true);
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error al guardar el fichero");
                }
            }
            return Json(mRespuestaAjax);
        }
        public ActionResult CambioDePass()
        {
            var usuarioActual = mUsuarios.UsuarioActivo(SesionHelper.GetUser());
            return View(usuarioActual);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CambioDePass(int id, string PassActual, string PassNuevo)
        {
            int result;
            mRespuestaAjax = new RespuestaServidor();
            if (ModelState.IsValid)
            {
                result = mUsuarios.CambiarPassword(id, PassActual, PassNuevo);
                if (result == 0) 
                {
                    mRespuestaAjax.SetResponse(false, "Contraseña Actual no coincide");
                }
                else if (result == 1)
                {
                    mRespuestaAjax.SetResponse(true, "Cambio realizado OK");
                }
                else
                {
                    mRespuestaAjax.SetResponse(false, "Error al grabar Password");
                }
            }
            else
            {
                mRespuestaAjax.SetResponse(false, "Error inesplicable");
            }
            return Json(mRespuestaAjax);

        }
        

    }
}