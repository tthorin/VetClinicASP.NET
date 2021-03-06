// -----------------------------------------------------------------------------------------------
//  CustomerController.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDbAccess.Models;
    using Models;
    using static Mapper.ModelMapper;
    using System.Linq;
    //using PagedList;
    using X.PagedList;

    public class CustomerController : Controller
    {
        readonly private MongoDbAccess.Interfaces.ICustomerCrud db = MongoDbAccess.Factory.GetICustomerCrud();

        // GET: CustomerController
        public async Task<ActionResult> Index(string sortOrder,string currentFilter, string searchString,int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            ViewData["FirstNameSortParam"] = sortOrder == "firstName" ? "firstName_desc" : "firstName";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewData["CurrentFilter"] = searchString;

            List<Customer> data = string.IsNullOrEmpty(searchString)
                ? await db.GetCustomersEitherNameBeginsWith("a")
                : await db.GetCustomersEitherNameBeginsWith(searchString);
            data = sortOrder switch
            {
                "lastName_desc" => data.OrderByDescending(x => x.LastName).ToList(),
                "firstName" => data.OrderBy(x => x.FirstName).ToList(),
                "firstName_desc" => data.OrderByDescending(x => x.FirstName).ToList(),
                _ => data.OrderBy(x => x.LastName).ToList()
            };

            List<CustomerViewModel> customers = new();
            foreach (var item in data)
            {
                customers.Add(ToCustomerViewModel(item));
            }

            const int pageSize = 8;
            int pageNumber = (page ?? 1);

            return View(customers.ToPagedList(pageNumber,pageSize));
        }

        // GET: CustomerController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var data = await db.GetCustomerById(id);
            var viewPerson = ToCustomerViewModel(data);

            return View(viewPerson);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerViewModel owner)
        {
            if (!ModelState.IsValid) return View();
            var result = ToCustomer(owner);
            await db.CreateCustomer(result);

            if (result.Id != null) ViewData["Success"] = result.Id;

            return View();
        }

        // GET: CustomerController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var data = await db.GetCustomerById(id);
            var editPerson = ToCustomerViewModel(data);
            return View(editPerson);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerViewModel owner)
        {
            if (ModelState.IsValid)
            {
                var reMapped = ToCustomer(owner);
                var result = await db.UpdateCustomer(reMapped);
                if (result) ViewData["Success"] = $"User {owner.FirstName} {owner.LastName} updated successfully.";
                return View(owner);
            }
            return View(owner);
        }

        // GET: CustomerController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var data = await db.GetCustomerById(id);
            var owner = ToCustomerViewModel(data);
            return View(owner);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(CustomerViewModel ownerToDelete)
        {
            var deletionSuccessful = await db.DeleteCustomerById(ownerToDelete.Id);
            if (deletionSuccessful) ViewData["Success"] = $"User {ownerToDelete.FirstName} {ownerToDelete.LastName} deleted.";
            return View();
        }
    }
}