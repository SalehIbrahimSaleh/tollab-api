using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperKotob.Admin.Data;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class StudentContentsController : Controller
    {
        private TollabContext db = new TollabContext();

        // GET: StudentContents
        public async Task<ActionResult> Index()
        {
            var studentContents = db.StudentContents.Include(s => s.Content).Include(s => s.Student);
            return View(await studentContents.ToListAsync());
        }

        // GET: StudentContents/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentContent studentContent = await db.StudentContents.FindAsync(id);
            if (studentContent == null)
            {
                return HttpNotFound();
            }
            return View(studentContent);
        }

        // GET: StudentContents/Create
        public ActionResult Create()
        {
            ViewBag.ContentId = new SelectList(db.Contents, "Id", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name");
            return View();
        }

        // POST: StudentContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ContentId,StudentId")] StudentContent studentContent)
        {
            if (ModelState.IsValid)
            {
                db.StudentContents.Add(studentContent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ContentId = new SelectList(db.Contents, "Id", "Name", studentContent.ContentId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", studentContent.StudentId);
            return View(studentContent);
        }

        // GET: StudentContents/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentContent studentContent = await db.StudentContents.FindAsync(id);
            if (studentContent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentId = new SelectList(db.Contents, "Id", "Name", studentContent.ContentId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", studentContent.StudentId);
            return View(studentContent);
        }

        // POST: StudentContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ContentId,StudentId")] StudentContent studentContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentContent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ContentId = new SelectList(db.Contents, "Id", "Name", studentContent.ContentId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", studentContent.StudentId);
            return View(studentContent);
        }

        // GET: StudentContents/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentContent studentContent = await db.StudentContents.FindAsync(id);
            if (studentContent == null)
            {
                return HttpNotFound();
            }
            return View(studentContent);
        }

        // POST: StudentContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            StudentContent studentContent = await db.StudentContents.FindAsync(id);
            db.StudentContents.Remove(studentContent);
            await db.SaveChangesAsync();
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
