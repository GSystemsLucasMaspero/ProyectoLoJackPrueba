using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoLojackABM.Models;
using System.Threading;
using System.Globalization;

namespace ProyectoLojackABM.Controllers
{
    public class NivelServicioController : Controller
    {
        protected CodeDB DB = new CodeDB();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDataNivelServicio(NivelServicioModel f)
        {
            if (ModelState.IsValid)
            {
                
                DB.Open();
                var originalCulture = Thread.CurrentThread.CurrentCulture;
                var usCulture = new CultureInfo("en-US");
                if (!originalCulture.Equals(usCulture)) Thread.CurrentThread.CurrentCulture = usCulture; 
                var fechaAlta = f.fechaAlta.ToShortDateString();
                var fechaBaja = f.fechaBaja != null ? "'" + f.fechaBaja.ToString() + "'" : "null";
                var usuarioBaja = f.usuarioBaja != null ? "'" + f.usuarioBaja.ToString() + "'" : "null";
                var consult = string.Format(@"INSERT INTO 
                    NivelServicio(descripcion,fechaAlta,usuarioAlta,fechaBaja,usuarioBaja) 
                    VALUES('{0}','{1}','{2}',{3},{4})", f.descripcion, fechaAlta, f.usuarioAlta, fechaBaja, usuarioBaja);
                //string consult1 = "INSERT INTO NivelServicio(descripcion,fechaAlta,usuarioAlta,fechaBaja,usuarioBaja) VALUES("'" + f.descripcion + "','" + fechaAlta + "','" + f.usuarioAlta + "','" + f.fechaBaja + "','" + f.usuarioBaja + "')";
                int i = DB.DataInsert(consult);
                if (i > 0)
                {
                    ModelState.AddModelError("Success", "Save Success");
                }
                else
                {
                    ModelState.AddModelError("Error", "Save Error");
                }
                DB.Close();
            }
            return View("Index");
        }

    }
}
