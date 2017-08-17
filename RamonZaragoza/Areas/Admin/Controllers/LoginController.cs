using Helper;
using Models;
using RamonZaragoza.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RamonZaragoza.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private RespuestaServidor mResp;
        private Usuario mUsuario = new Usuario();
        [NoLogin]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(string Email, string Password)
        {
            mResp = new RespuestaServidor();
            Usuario usuario;
            usuario = mUsuario.Acceder(Email, Password);
            if (usuario!= null)
            {
                mResp.SetResponse(true, "Redirigiendo... espere por favor");
                mResp.href = Url.Content("~/admin/usuario");
            }
            else
            {
                mResp.SetResponse(false, "Correo o contraseña incorrecta");
            }
            return Json(mResp);
            
        }

        public ActionResult Logout() 
        {
            SesionHelper.DestroyUserSession();
            return Redirect("~/");
        }
    }
}