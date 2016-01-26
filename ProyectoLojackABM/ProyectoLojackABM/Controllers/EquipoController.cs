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
    public class EquipoController : Controller
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
            ViewBag.IdentificadorSortParm = String.IsNullOrEmpty(sortOrder) ? "identificador_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var equipos = from s in db.Equipoes
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                equipos = equipos.Where(s => s.identificador.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "id_desc":
                    equipos = equipos.OrderBy(s => s.idEquipo);
                    break;
                case "identificador_desc":
                    equipos = equipos.OrderByDescending(s => s.identificador);
                    break;
                case "Date":
                    equipos = equipos.OrderBy(s => s.fechaAlta);
                    break;
                case "date_desc":
                    equipos = equipos.OrderByDescending(s => s.fechaAlta);
                    break;
                default:
                    equipos = equipos.OrderBy(s => s.idEquipo);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(equipos.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            if (last_id == 0)
            {
                last_id = db.Equipoes.ToArray().Last().idEquipo;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Equipo equipo)
        {
            equipo.fechaAlta = DateTime.Now;
            equipo.idEquipoTipo = ++last_id;
            if (ModelState.IsValid)
            {
                equipo.usuarioAlta = usuarioPrueba; // Hasta que este hecho el log-in
                db.Equipoes.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipo);
        }

        public ActionResult Edit(int id = 0)
        {
            Equipo equipo = db.Equipoes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", equipo.idCuenta);
            ViewBag.idEquipoTipo = new SelectList(db.EquipoTipoes, "idEquipoTipo", "descripcion", equipo.idEquipoTipo);
            return View(equipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", equipo.idCuenta);
            ViewBag.idEquipoTipo = new SelectList(db.EquipoTipoes, "idEquipoTipo", "descripcion", equipo.idEquipoTipo);
            return View(equipo);
        }

        public ActionResult Delete(int id = 0)
        {
            Equipo equipo = db.Equipoes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipo equipo = db.Equipoes.Find(id);
            db.Equipoes.Remove(equipo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}