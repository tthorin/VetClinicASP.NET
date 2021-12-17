// -----------------------------------------------------------------------------------------------
//  CustomerController.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using MongoDbAccess.Models;
    using VetClinic.Models;
    using static VetClinic.Mapper.ModelMapper;

    public class CustomerController : Controller
    {
        private MongoDbAccess.Database.MongoDbAccess db = MongoDbAccess.Factory.GetDataAccess();
        private List<CustomerViewModel> owners = new();

        // GET: PetOwnerController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListCustomers(string beginsWith)
        {
            List<Customer> data = new();
            if (beginsWith != null) data = await db.GetCustomersLastNameBeginsWith(beginsWith);
            else data = await db.GetAllCustomers();

            var thisUrl = Request.QueryString.HasValue ? Request.QueryString.Value[^1].ToString() : "0";
            ViewData["thisUrl"] = thisUrl;

            owners.Clear();
            foreach (var item in data)
            {
                owners.Add(ToCustomerViewModel(item));
            }
            return View(owners);
        }

        // GET: PetOwnerController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var data = await db.GetCustomerById(id);
            var viewPerson = ToCustomerViewModel(data);

            return View(viewPerson);
        }

        // GET: PetOwnerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PetOwnerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerViewModel owner)
        {
            if (ModelState.IsValid)
            {
                var result = ToCustomer(owner);
                await db.CreateOwner(result);
                return RedirectToAction(nameof(ListCustomers));
            }
            return View();
        }

        // GET: PetOwnerController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var data = await db.GetCustomerById(id);
            var editPerson = ToCustomerViewModel(data);
            if (editPerson.Pets?.Count > 0) TempData["Pets"] = 1;
            return View(editPerson);
        }

        // POST: PetOwnerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerViewModel owner)
        {
            if (ModelState.IsValid)
            {
                if (TempData.ContainsKey("Pets"))
                {
                    var _ = TempData["Pets"];
                    var containsPets = await db.GetCustomerById(owner.Id);
                    owner.Pets = containsPets.Pets;
                }
                var reMapped = ToCustomer(owner);
                await db.UpdateCustomer(reMapped);
                return RedirectToAction(nameof(ListCustomers));
            }
            if (owner.Pets?.Count > 0) TempData["Pets"] = owner.Pets;
            return View(owner);
        }

        // GET: PetOwnerController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var data = await db.GetCustomerById(id);
            var owner = ToCustomerViewModel(data);
            return View(owner);
        }

        // POST: PetOwnerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(CustomerViewModel ownerToDelete)
        {
            await db.DeleteCustomerById(ownerToDelete.Id);
            return RedirectToAction(nameof(ListCustomers));
        }
    }
}
