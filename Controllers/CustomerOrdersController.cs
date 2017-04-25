using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mls.Models;
using mls.ViewModels;

namespace mls.Controllers
{
    public class CustomerOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomerOrders
        [Authorize]
        public ActionResult Index()
        {
            //return View(db.CustomerOrders.ToList());
            if (User.IsInRole("Admin"))
                return View("Index", db.CustomerOrders.ToList());
            else
                return View("ROIndex", db.CustomerOrders.ToList());
        }

        // GET: CustomerOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            if (customerOrder == null)
            {
                return HttpNotFound();
            }
            return View(customerOrder);
        }

        // GET: CustomerOrders/Create
        public ActionResult Create()
        {
            /*
            //var customers = GetAllCustomers();
            //var divisions = GetAllDivisions();
            //var mlsdivisions = GetAllMlsDivisions();
            //var statuss = GetAllStatuss();

            //var model = new Order();

            //model.Customers = GetSelectListItems(customers);
            //model.CustomerDivisions = GetSelectListItems(divisions);
            //model.MlsDivisions = GetSelectListItems(mlsdivisions);
            //model.Statuss = GetSelectListItems(statuss);
            */

            //drop down list feed coded here
            var customers = db.Customers.ToList();
            var customerdivisions = db.CustomerDivisions.ToList();
            var mlsdivisions = db.MlsDivisions.ToList();
            var orderstatuses = db.OrderStatuses.ToList();

            var viewModel = new SaveOrderViewModel()
            {
                Customers = customers,
                CustomerDivisions = customerdivisions,
                MlsDivisions = mlsdivisions,
                OrderStatuses = orderstatuses

            };

            return View("Create", viewModel);
        }

        // POST: CustomerOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "CustomerOrderId,CustomerOrderNumber,SoNumber,OrderDateTime,CustomerId,CustomerDivisionId,MlsDivisionId,CustomerPn,UhPn,PartDescription,PartPrice,OrderQty,ShipQty,RequestedDateTime,PromiseDateTime,ShipDateTime,StatusId,Notes")] CustomerOrder customerOrder)
        public ActionResult Create(CustomerOrder customerOrder)
        {
            if (ModelState.IsValid)
            {
                db.CustomerOrders.Add(customerOrder);
                db.SaveChanges();
                return RedirectToAction("Index", "CustomerOrders");
            }

            return View();
            //return View(customerOrder);
        }

        // GET: CustomerOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            var customerorders = db.CustomerOrders.SingleOrDefault(c => c.CustomerOrderId == id);

            var customers = db.Customers.ToList();
            var customerdivisions = db.CustomerDivisions.ToList();
            var mlsdivisions = db.MlsDivisions.ToList();
            var orderstatuses = db.OrderStatuses.ToList();

            var viewModel = new SaveOrderViewModel()
            {
                CustomerOrder = customerorders,
                Customers = customers,
                CustomerDivisions = customerdivisions,
                MlsDivisions = mlsdivisions,
                OrderStatuses = orderstatuses
            };
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            if (customerOrder == null)
            {
                return HttpNotFound();
            }

            return View("Edit", viewModel);
            //return View(customerOrder);
        }

        // POST: CustomerOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "CustomerOrderId,CustomerOrderNumber,SoNumber,OrderDateTime,CustomerId,CustomerDivisionId,MlsDivisionId,CustomerPn,UhPn,PartDescription,PartPrice,OrderQty,ShipQty,RequestedDateTime,PromiseDateTime,ShipDateTime,OrderStatusId,Notes")] CustomerOrder customerOrder)
        public ActionResult Edit(CustomerOrder customerOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: CustomerOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            if (customerOrder == null)
            {
                return HttpNotFound();
            }
            return View(customerOrder);
        }

        // POST: CustomerOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            db.CustomerOrders.Remove(customerOrder);
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
