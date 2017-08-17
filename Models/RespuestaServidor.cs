using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RespuestaServidor
    {
	    public dynamic result{get;set;}
	    public bool respuesta{get;set;}
	    public string mensaje{get;set;}
	    public string href {get;set;}
	    public string funcion {get;set;}

        public RespuestaServidor() 
        {
            this.respuesta = false;
            this.mensaje = "Ocurrio un error inesperado";
        }
	
	    public void SetResponse(bool r, string m = "")
	    {
		    this.respuesta = r;
		    this.mensaje = m;

		    if(!r && m == "") this.mensaje = "Ocurrio un error inesperado";
	    }
    }
}
