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
    public class SectorController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();
        //
        // GET: /Sector/

        public ActionResult Index(int ?Order, string currentFilter, string searchString, int? page)
        {
            ViewBag.filtro = false;
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var query = from sectores in db.Sectors select sectores;
           
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.nombre.Contains(searchString));
                ViewBag.filtro = true;
            }

            switch (Order)
            {
                case 0:
                    query = query.OrderBy(s => s.idSector);
                    break;
                case 1:
                    query = query.OrderBy(s => s.nombre);
                    break;
                case 2:
                    query = query.OrderBy(s => s.idCuenta);
                    break;
                default:
                    query = query.OrderBy(s => s.idSector);
                    break;
            }

            if (query.Count() == 0 && ViewBag.filtro == true)
            {
                return Content("<p>NO SE HAN ENCONTRADO RESULTADOS</p>");
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, pageSize));
        }
        //INDEX CHECK

        //
        // GET: /Sector/Details/5


        public ActionResult Create()
        {
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre");
            return View();
        }

        //
        // POST: /Sector/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sector sector)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO Sector (nombre,fechaAlta,usuarioAlta,idCuenta)VALUES(@p0,@p1,@p2,@p3)",sector.nombre,DateTime.Now.ToString("yyyy-MM-dd h:mm:ss.f"),66/*TEMPORAL*/,sector.idCuenta);
                return RedirectToAction("Index");
            }

            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", sector.idCuenta);
            return View(sector);
        }

        //CREATE LISTO

        //
        // GET: /Sector/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Sector sector = db.Sectors.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", sector.idCuenta);
            return View(sector);
        }

        //
        // POST: /Sector/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sector sector)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("UPDATE Sector SET nombre = @P0, idCuenta = @p1 WHERE idSector = @p2", sector.nombre, sector.idCuenta,sector.idSector);

                return RedirectToAction("Index");
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", sector.idCuenta);
            return View(sector);
        }

        //EDIT CHECK
        //
        // GET: /Sector/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Sector sector = db.Sectors.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        //
        // POST: /Sector/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sector sector = db.Sectors.Find(id);
            db.Database.ExecuteSqlCommand("UPDATE Sector SET fechaBaja = @P0, usuarioBaja = @p1 WHERE idSector = @p2", DateTime.Now.ToString("yyyy-MM-dd h:mm:ss.f"), 66/*TEMPORAL*/, sector.idSector);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}