using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace CampManagement.Web.Controllers
{
    public class CampController : Controller
    {
        // GET: Camp
        public ActionResult Index()
        {
            return View();
        }

        // GET: Camp/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Camp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Camp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Camp/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Camp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Camp/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Camp/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}