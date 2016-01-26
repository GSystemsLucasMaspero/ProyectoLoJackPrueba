using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoLojackABM.Models;

namespace ProyectoLojackABM.Controllers
{
    public class EquipoTipoController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();
        private static int last_edit_id = 0;
        private static int last_delete_id = 0;
        private static int last_id = 0;

        public ActionResult Index()
        {
            return View(db.EquipoTipoes.ToList());
        }

        public ActionResult Create()
        {
            if (last_id == 0)
            {
                last_id = db.EquipoTipoes.ToArray().Last().idEquipoTipo;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EquipoTipo equipotipo)
        {
            equipotipo.fechaAlta = DateTime.Now;
            equipotipo.idEquipoTipo = ++last_id;
            if (ModelState.IsValid)
            {
                db.EquipoTipoes.Add(equipotipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipotipo);
        }

        public ActionResult Edit(int id = 0)
        {
            EquipoTipo equipotipo = db.EquipoTipoes.Find(id);
            if (equipotipo == null)
            {
                return HttpNotFound();
            }
            last_edit_id = id;
            return View(equipotipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EquipoTipo equipotipo)
        {
            equipotipo.idEquipoTipo = last_edit_id;
            if (ModelState.IsValid)
            {
                var equipoTipoToUpdate = db.EquipoTipoes.SingleOrDefault(ns => ns.idEquipoTipo == equipotipo.idEquipoTipo);
                if (equipoTipoToUpdate != null)
                {
                    equipoTipoToUpdate.descripcion = equipotipo.descripcion;
                    equipoTipoToUpdate.usuarioAlta = equipotipo.usuarioAlta;
                    equipoTipoToUpdate.cantSensores = equipotipo.cantSensores;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(equipotipo);
        }

        public ActionResult Delete(int id = 0)
        {
            EquipoTipo equipotipo = db.EquipoTipoes.Find(id);
            if (equipotipo == null)
            {
                return HttpNotFound();
            }
            last_delete_id = id;
            return View(equipotipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EquipoTipo equipotipo)
        {
            equipotipo.idEquipoTipo = last_delete_id;
            if (ModelState.IsValid)
            {
                var equipoTipoToUpdate = db.EquipoTipoes.SingleOrDefault(ns => ns.idEquipoTipo == equipotipo.idEquipoTipo);
                if (equipoTipoToUpdate != null)
                {
                    equipoTipoToUpdate.usuarioBaja = equipotipo.usuarioBaja;
                    equipoTipoToUpdate.fechaBaja = DateTime.Now;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(equipotipo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}