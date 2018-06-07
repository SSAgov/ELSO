using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ELSO.Model;
using ELSO.Services;

namespace ELSO.Web.Controllers
{
    public class PeopleController : Controller
    {
        private PersonService _service;

        public PeopleController()
        {
            _service = new PersonService();
        }

        // GET: People
        public ActionResult Index()
        {
            return View(_service.GetAll());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            return null;
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,FirstName,MiddleName,LastName,PhoneNumber,Organization,SSA_PIN,CreatedDate,ModifiedDate")] Person person)
        {
            if (ModelState.IsValid)
            {
                _service.Save(person);
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _service.GetPersonByPin("pin");
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,FirstName,MiddleName,LastName,PhoneNumber,Organization,SSA_PIN,CreatedDate,ModifiedDate")] Person person)
        {
            if (ModelState.IsValid)
            {
                _service.Save(person);
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _service.GetPersonByPin("pin");
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = _service.GetPersonByPin("pin");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
