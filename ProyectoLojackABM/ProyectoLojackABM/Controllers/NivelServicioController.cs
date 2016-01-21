using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoLojackABM.Models;
using System.Threading;
using System.Globalization;
using System.Data;

namespace ProyectoLojackABM.Controllers
{
    public class NivelServicioController : Controller
    {
        protected DBManager DB = new DBManager();

        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowList(NivelServicioModel f)
        {
            DB.Open();

            DataTable list = DB.GetData("NivelServicio");
            if (list != null)
            {
                
            }

            DB.Close();
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDataNivelServicio(NivelServicioModel f)
        {
            if (ModelState.IsValid)
            {
                DB.Open();

                // Formatear la fecha para que la base de datos la reconozca
                var fechaAlta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
              
                var sqlCommandInsert = string.Format(@"INSERT INTO 
                    NivelServicio(descripcion,fechaAlta,usuarioAlta,fechaBaja,usuarioBaja) 
                    VALUES('{0}','{1}','{2}',{3},{4})", f.descripcion, fechaAlta, f.usuarioAlta, "null", "null");
                
                // Si inserte exitosamente,
                int i = DB.DataInsert(sqlCommandInsert);
                if (i <= 0) 
                    ModelState.AddModelError("Error", "Save Error");

                DB.Close();
            }
            return View("Index");
        }

    }
}
