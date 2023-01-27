using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using EzyPeezy.Models;

namespace EzyPeezy.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products
        public ActionResult Shop()
        {
            if (this.Session["CartData"] == null)
            {
                var cartData = new Cart { };
                this.Session["CartData"] = cartData;
            }

            return View(db.Products.ToList());
        }



        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ID,Name,Description,Price,Quantity,Image")] Product product, 
            HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                UploadImage(product, Image);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ID,Name,Description,Price,Quantity,Image")] Product product, 
            HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                UploadImage(product, Image);

                db.Products.Attach(product);

                var entry = db.Entry(product);
                entry.State = EntityState.Modified;

                if (Image == null)
                {
                    entry.Property(e => e.Image).IsModified = false;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void UploadImage(Product product, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                string imagePath = "~/Images/";
                if (!System.IO.Directory.Exists(Server.MapPath(imagePath)))
                    System.IO.Directory.CreateDirectory(Server.MapPath(imagePath));

                Image.SaveAs(HttpContext.Server.MapPath(imagePath) + Image.FileName);
                product.Image = Image.FileName;
            }
        }
    }
}
