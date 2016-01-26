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
    public class NivelServicioController : Controller
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
            ViewBag.DescriptionSortParm = String.IsNullOrEmpty(sortOrder) ? "desc_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var nivelservicios = from s in db.NivelServicios
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                nivelservicios = nivelservicios.Where(s => s.descripcion.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "id_desc":
                    nivelservicios = nivelservicios.OrderBy(s => s.idNivelServicio);
                    break;
                case "desc_desc":
                    nivelservicios = nivelservicios.OrderByDescending(s => s.descripcion);
                    break;
                case "Date":
                    nivelservicios = nivelservicios.OrderBy(s => s.fechaAlta);
                    break;
                case "date_desc":
                    nivelservicios = nivelservicios.OrderByDescending(s => s.fechaAlta);
                    break;
                default:
                    nivelservicios = nivelservicios.OrderBy(s => s.idNivelServicio);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(nivelservicios.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            if (last_id == 0)
            {
                last_id = db.NivelServicios.ToArray().Last().idNivelServicio;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NivelServicio nivelservicio)
        {
            nivelservicio.fechaAlta = DateTime.Now;
            nivelservicio.idNivelServicio = ++last_id;
            if (ModelState.IsValid)
            {
                nivelservicio.usuarioAlta = usuarioPrueba;  // Hasta que este hecho el log-in
                db.NivelServicios.Add(nivelservicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nivelservicio);
        }

        public ActionResult Edit(int id = 0)
        {
            NivelServicio nivelservicio = db.NivelServicios.Find(id);
            if (nivelservicio == null)
            {
                return HttpNotFound();
            }
            last_edit_id = id;
            return View(nivelservicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NivelServicio nivelservicio)
        {
            nivelservicio.idNivelServicio = last_edit_id;
            if (ModelState.IsValid)
            {
                var nivelServicioToUpdate = db.NivelServicios.SingleOrDefault(ns => ns.idNivelServicio == nivelservicio.idNivelServicio);
                if (nivelServicioToUpdate != null)
                {
                    nivelServicioToUpdate.descripcion = nivelservicio.descripcion;
                    nivelServicioToUpdate.usuarioAlta = nivelservicio.usuarioAlta;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(nivelservicio);
        }

        public ActionResult Delete(int id = 0)
        {
            NivelServicio nivelservicio = db.NivelServicios.Find(id);
            if (nivelservicio == null)
            {
                return HttpNotFound();
            }
            last_delete_id = id;
            return View(nivelservicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(NivelServicio nivelservicio)
        {
            nivelservicio.idNivelServicio = last_delete_id;
            if (ModelState.IsValid)
            {
                var nivelServicioToUpdate = db.NivelServicios.SingleOrDefault(ns => ns.idNivelServicio == nivelservicio.idNivelServicio);
                if (nivelServicioToUpdate != null)
                {
                    nivelServicioToUpdate.usuarioBaja = usuarioPrueba; // Hasta que este hecho el log-in
                    nivelServicioToUpdate.fechaBaja = DateTime.Now;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(nivelservicio);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}