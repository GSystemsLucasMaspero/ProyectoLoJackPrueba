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
    public class EntidadController : Controller
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
            ViewBag.NombreSortParm = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var entidades = from s in db.Entidads
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                entidades = entidades.Where(s => s.nombre.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "id_desc":
                    entidades = entidades.OrderBy(s => s.idEntidad);
                    break;
                case "nombre_desc":
                    entidades = entidades.OrderByDescending(s => s.nombre);
                    break;
                case "Date":
                    entidades = entidades.OrderBy(s => s.fechaAlta);
                    break;
                case "date_desc":
                    entidades = entidades.OrderByDescending(s => s.fechaAlta);
                    break;
                default:
                    entidades = entidades.OrderBy(s => s.idEntidad);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(entidades.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            if (last_id == 0)
            {
                last_id = db.Entidads.ToArray().Last().idEntidad;
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre");
            ViewBag.idNivelServicio = new SelectList(db.NivelServicios, "idNivelServicio", "descripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entidad entidad)
        {
            entidad.fechaAlta = DateTime.Now;
            entidad.fechaModificacion = DateTime.Now;
            entidad.idEntidad = ++last_id;
            if (ModelState.IsValid)
            {
                entidad.usuarioAlta = usuarioPrueba;
                entidad.usuarioModificacion = usuarioPrueba;
                db.Entidads.Add(entidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", entidad.idCuenta);
            ViewBag.idNivelServicio = new SelectList(db.NivelServicios, "idNivelServicio", "descripcion", entidad.idNivelServicio);
            return View(entidad);
        }

        public ActionResult Edit(int id = 0)
        {
            Entidad entidad = db.Entidads.Find(id);
            if (entidad == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", entidad.idCuenta);
            ViewBag.idNivelServicio = new SelectList(db.NivelServicios, "idNivelServicio", "descripcion", entidad.idNivelServicio);
            last_edit_id = id; 
            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Entidad entidad)
        {
            entidad.idEntidad = last_edit_id;
            if (ModelState.IsValid)
            {
                var entidadToUpdate = db.Entidads.SingleOrDefault(ns => ns.idEntidad == entidad.idEntidad);
                if (entidadToUpdate != null)
                {
                    entidadToUpdate.nombre = entidad.nombre;
                    entidadToUpdate.estado = entidad.estado;
                    entidadToUpdate.idNivelServicio = entidad.idNivelServicio;
                    entidadToUpdate.plantillaSuceso = entidad.plantillaSuceso;
                    entidadToUpdate.cadenciaReporte = entidad.cadenciaReporte;
                    entidadToUpdate.comentario = entidad.comentario;
                    entidadToUpdate.telefono = entidad.telefono;
                    entidadToUpdate.idProcedimiento = entidad.idProcedimiento;
                    entidadToUpdate.idCuenta = entidad.idCuenta;
                    entidadToUpdate.fechaModificacion = DateTime.Now;
                    entidadToUpdate.usuarioModificacion = usuarioPrueba;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", entidad.idCuenta);
            ViewBag.idNivelServicio = new SelectList(db.NivelServicios, "idNivelServicio", "descripcion", entidad.idNivelServicio);
            return View(entidad);
        }

        public ActionResult Delete(int id = 0)
        {
            Entidad entidad = db.Entidads.Find(id);
            if (entidad == null)
            {
                return HttpNotFound();
            }
            last_delete_id = id;
            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Entidad entidad)
        {
            entidad.idEntidad = last_delete_id;
            if (ModelState.IsValid)
            {
                var entidadToUpdate = db.Entidads.SingleOrDefault(ns => ns.idEntidad == entidad.idEntidad);
                if (entidadToUpdate != null)
                {
                    entidadToUpdate.fechaBaja = DateTime.Now;
                    entidadToUpdate.usuarioBaja = usuarioPrueba; // Hasta que este hecho el log-in
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(entidad);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}