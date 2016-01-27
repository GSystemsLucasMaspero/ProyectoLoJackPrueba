using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoLojackABM.Models;
using PagedList;

namespace ProyectoLojackABM.Controllers
{
    public class EquipoTipoController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();
        private static int last_edit_id = 0;
        private static int last_delete_id = 0;
        private static int last_id = 0;

        // Hasta que este hecho el log-in
        private static int usuarioPrueba = 20;

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParm = "id_desc";
            ViewBag.SensoresSortParm = "sensores_desc";
            ViewBag.DescriptionSortParm = String.IsNullOrEmpty(sortOrder) ? "desc_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var equipotipos = from s in db.EquipoTipoes
                           select s;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                equipotipos = equipotipos.Where(s => s.descripcion.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "sensores_desc":
                    equipotipos = equipotipos.OrderBy(s => s.cantSensores);
                    break;
                case "id_desc":
                    equipotipos = equipotipos.OrderBy(s => s.idEquipoTipo);
                    break;
                case "desc_desc":
                    equipotipos = equipotipos.OrderByDescending(s => s.descripcion);
                    break;
                case "Date":
                    equipotipos = equipotipos.OrderBy(s => s.fechaAlta);
                    break;
                case "date_desc":
                    equipotipos = equipotipos.OrderByDescending(s => s.fechaAlta);
                    break;
                default:
                    equipotipos = equipotipos.OrderBy(s => s.idEquipoTipo);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(equipotipos.ToPagedList(pageNumber, pageSize));
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
                equipotipo.usuarioAlta = usuarioPrueba; // Hasta que este hecho el log-in
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
                    equipoTipoToUpdate.fechaBaja = DateTime.Now;
                    equipoTipoToUpdate.usuarioBaja = usuarioPrueba; // Hasta que este hecho el log-in
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