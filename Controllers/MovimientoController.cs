using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TamayoConde_IIU.Models;

namespace TamayoConde_IIU.Controllers
{
    public class MovimientoController : Controller
    {
        // GET: Movimiento
        Movimiento objMovimiento = new Movimiento();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Consulta()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MovimientoRegistrarJson(Movimiento obj)
        {
            bool respuesta = false;
            string mensaje = "";
            try
            {
                objMovimiento.MovimientoRegistrar(obj);
                respuesta = true;
                mensaje = "Se ha registrado exitosamente";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje });
        }
        [HttpPost]
        public ActionResult MovimientoEliminarJson(Movimiento obj)
        {
            bool respuesta = false;
            string mensaje = "";
            try
            {
                objMovimiento.MovimientoEliminar(obj);
                respuesta = true;
                mensaje = "Se ha eliminado exitosamente";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje });
        }

        public ActionResult MovimientoListarJson()
        {
            string mensaje = "";
            var data = new List<Movimiento>();
            try
            {
                data = objMovimiento.MovimientoListar();
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            var result = new { data = data, mensaje = mensaje };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MovimientoListarFiltrar(int Filtro,int IdProducto)
        {
            string mensaje = "";
            var data = new List<Movimiento>();
            try
            {
                data = objMovimiento.MovimientoListarFiltrar(Filtro,IdProducto);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            var result = new { data = data, mensaje = mensaje };
            return Json(result, JsonRequestBehavior.AllowGet);
        }        
    }
}