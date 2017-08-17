using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace RamonZaragoza.Controllers
{
    public class DefaultController : Controller
    {
        TablaDato datos = new TablaDato();
        public int Index()
        {
            return  datos.conexion();
        }
    }
}