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
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre");
            ViewBag.idEquipoTipo = new SelectList(db.EquipoTipoes, "idEquipoTipo", "descripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Equipo equipo)
        {
            equipo.fechaAlta = DateTime.Now;
            equipo.fechaModificacion = DateTime.Now;
            if (ModelState.IsValid)
            {
                equipo.usuarioAlta = usuarioPrueba;
                equipo.usuarioModificacion = usuarioPrueba;
                db.Equipoes.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", equipo.idCuenta);
            ViewBag.idNivelServicio = new SelectList(db.EquipoTipoes, "idEquipoTipo", "descripcion", equipo.idEquipoTipo);
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
            last_edit_id = id;
            return View(equipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Equipo equipo)
        {
            equipo.idEquipo = last_edit_id;
            if (ModelState.IsValid)
            {
                var equipoToUpdate = db.Equipoes.SingleOrDefault(ns => ns.idEquipo == equipo.idEquipo);
                if (equipoToUpdate != null)
                {
                    equipoToUpdate.identificador = equipo.identificador;
                    equipoToUpdate.nroSerie = equipo.nroSerie;
                    equipoToUpdate.primario = equipo.primario;
                    equipoToUpdate.cadencia = equipo.cadencia;
                    equipoToUpdate.versionFirmware = equipo.versionFirmware;
                    equipoToUpdate.versionProgramacion = equipo.versionProgramacion;
                    equipoToUpdate.estadoSd = equipo.estadoSd;
                    equipoToUpdate.portable = equipo.portable;
                    equipoToUpdate.idEquipoTipo = equipo.idEquipoTipo;
                    equipoToUpdate.idCuenta = equipo.idCuenta;
                    equipoToUpdate.fechaModificacion = DateTime.Now;
                    equipoToUpdate.usuarioModificacion = usuarioPrueba;
                    db.SaveChanges();
                }
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
            last_delete_id = id;
            return View(equipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Equipo equipo)
        {
            equipo.idEquipo = last_delete_id;
            if (ModelState.IsValid)
            {
                var equipoToUpdate = db.Equipoes.SingleOrDefault(ns => ns.idEquipo == equipo.idEquipo);
                if (equipoToUpdate != null)
                {
                    equipoToUpdate.fechaBaja = DateTime.Now;
                    equipoToUpdate.usuarioBaja = usuarioPrueba; // Hasta que este hecho el log-in
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(equipo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}