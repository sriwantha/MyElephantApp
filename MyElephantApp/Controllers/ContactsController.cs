using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyElephantApp.DB;
using Amazon.XRay.Recorder.Core;

namespace MyElephantApp.Controllers
{
    public class ContactsController : Controller
    {
        private MyElephantDBEntities db = new MyElephantDBEntities();

        private ActionResult Execute(string name, Func<ActionResult> a)
        {
            string id = Amazon.XRay.Recorder.Core.Internal.Entities.TraceId.NewId();
  
            AWSXRayRecorder.Instance.BeginSegment(name,id);
            AWSXRayRecorder.Instance.BeginSubsegment("Level-2");
            System.Threading.Thread.Sleep(5000);
            AWSXRayRecorder.Instance.BeginSubsegment("Level-3");
            System.Threading.Thread.Sleep(2000);
            AWSXRayRecorder.Instance.BeginSubsegment("Level-4");
            System.Threading.Thread.Sleep(3000);
            AWSXRayRecorder.Instance.EndSubsegment();
            AWSXRayRecorder.Instance.EndSubsegment();
            AWSXRayRecorder.Instance.EndSubsegment();
            try
            {

                return a();
            }
            catch (Exception e)
            {
                AWSXRayRecorder.Instance.AddException(e);
                throw e;
            }
            finally
            {
                AWSXRayRecorder.Instance.EndSegment();
            }
        }
        // GET: Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            return Execute("Details", () =>
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                return View(contact);
            });
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,Email")] Contact contact)
        {
            return Execute("Create", () => 
            {

                AWSXRayRecorder.Instance.BeginSubsegment("Create method");
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Contacts.Add(contact);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception e)
                {
                    AWSXRayRecorder.Instance.AddException(e);
                }
                finally
                {
                    AWSXRayRecorder.Instance.EndSubsegment();
                }


                return View(contact); }
                );
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,Email")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
