// -----------------------------------------------------------------------------------------------
//  AnimalController.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using VetClinic.Models;
    using static Mapper.ModelMapper;
    using static MongoDbAccess.Factory;

    public class AnimalController : Controller
    {
        private readonly MongoDbAccess.Database.MongoDbAccess db = GetDataAccess();
        // GET: AnimalController
        public ActionResult Index()
        {
            var withoutAnnimals= db.GetCustomersWithoutAnimals();
            return View();
        }

        // GET: AnimalController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var animal = await db.GetAnimalById(id);
            var viewAnimal = ToAnimalViewModel(animal);
            return View(viewAnimal);
        }

        // GET: AnimalController/Create
        public ActionResult Create(string ownerId)
        {
            if (!string.IsNullOrWhiteSpace(ownerId)) TempData["ownerId"] = ownerId;
            
            return View();
        }

        // POST: AnimalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AnimalViewModel animal)
        {
            if (ModelState.IsValid)
            {
                if (TempData.ContainsKey("ownerId") && TempData["ownerId"] != null) animal.OwnerId = TempData["ownerId"] as string ?? "0";
                var output = ToAnimal(animal);
                await db.CreateAnimal(output);
                ViewData["Success"] = $"Animal {animal.Name} added successfully.";
                return View();
            }
            return View(animal);
        }

        // GET: AnimalController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AnimalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AnimalController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var dbAnimal = await db.GetAnimalById(id);
            var viewAnimal = ToAnimalViewModel(dbAnimal);
            TempData["ownerId"] = viewAnimal.OwnerId;
            return View(viewAnimal);
        }

        // POST: AnimalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(AnimalViewModel animal)
        {
            var ownerId = TempData["ownerId"]?.ToString();
            if (ownerId != null) await db.DeleteAnimalById(animal.Id, ownerId);
            return RedirectToAction("Details", "Customer", new { id = ownerId });
        }
    }
}
