using Helper;
using Models;
using PagedList;
using RamonZaragoza.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Areas.Admin.Controllers
{
    [Autenticado]
    public class HabilidadesController : Controller
    {
        Habilidad mHabilidad = new Habilidad();
        RespuestaServidor mRespuestaAjax;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Listado(Filtro filtro, int[] idEliminar, int? displayNum, int?pagina)
        {
            if (idEliminar != null)
            {
                bool result = true;
                for (int i = 0; i < idEliminar.Count() && result == true; i++)
                {
                    result = mHabilidad.EliminarHabilidad(idEliminar[i]);
                }
            }
            int maxPag = displayNum ?? 5;
            int numPag = pagina ?? 1;

            ViewBag.Listado = mHabilidad.ListaHabilidades(SesionHelper.GetUser(),filtro)
                .ToPagedList(numPag, maxPag);
            return PartialView();
        }
        public ActionResult Acciones (int id = 0)
        {
            if (id == 0)
            {
                ViewBag.Title = "Nueva Habilidad";
                mHabilidad.id = id;
                mHabilidad.Usuario_id = SesionHelper.GetUser();
            }
            else
            {
                ViewBag.Title = "Editar Habilidad";
                mHabilidad = mHabilidad.ObtenerHabilidad(id);

            }
            return View(mHabilidad);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Acciones (Habilidad modelo)
        {
            mRespuestaAjax = new RespuestaServidor();
            bool result = false;
            if (ModelState.IsValid)
            {
                result = modelo.GuardarHabilidad();
                if (result)
                {
                    mRespuestaAjax.SetResponse(true, "Datos guardados");
                    mRespuestaAjax.href = Url.Content("~/admin/habilidades");
                }else
                {
                    mRespuestaAjax.SetResponse(false, "Error al guardar los datos");
                }
                   
            } 
            return Json(mRespuestaAjax);
        }
    }
}