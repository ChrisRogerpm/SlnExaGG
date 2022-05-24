using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TamayoConde_IIU.Models;

namespace TamayoConde_IIU.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        Producto objProducto = new Producto();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductoListarJson()
        {
            string mensaje = "";
            var data = new List<Producto>();
            try
            {
                data = objProducto.ProductoListar();
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