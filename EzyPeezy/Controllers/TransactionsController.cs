using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EzyPeezy.Models;
using EzyPeezy.ViewModels;

namespace EzyPeezy.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
       
        public ActionResult Index()
        {
            return View(db.Transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        public ActionResult SaleReceipt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Checkout
        public ActionResult Checkout()
        {
            var viewModel = new TransactionCustomerViewModel();
            viewModel.Customers = db.Customers;
            return View(viewModel);
        }

        // POST: Transactions/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout([Bind(Include = "ID")] Transaction transaction, TransactionCustomerViewModel viewModel)
        {
            if (ModelState.IsValid && this.Session["CartData"] != null)
            {
                // Cart contents and total cost
                Cart cart = this.Session["CartData"] as Cart;

                // Only proceed if there's actually things in the Cart
                if (cart.TotalCost > 0)
                {
                    transaction.Amount = cart.TotalCost;
                    transaction.Items = cart.ToString();
                    this.Session["CartData"] = null;

                    // Current date and time
                    transaction.TransactionDateTime = DateTime.Now;

                    // (Optional) purchaser
                    if (viewModel.PickedCustomerID != null)
                    {
                        Customer purchaser = db.Customers.Find(viewModel.PickedCustomerID);
                        if (purchaser != null)
                        {
                            // Associate this Transaction with the Customer
                            transaction.Customer = purchaser;
                        }
                    }

                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                    return RedirectToAction("SaleReceipt", new { id = transaction.ID });
                }
            }

            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
