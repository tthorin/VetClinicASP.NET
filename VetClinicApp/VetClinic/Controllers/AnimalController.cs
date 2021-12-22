// -----------------------------------------------------------------------------------------------
//  AnimalController.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace VetClinic.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using VetClinic.Models;
    using X.PagedList;
    using static Mapper.ModelMapper;
    using static MongoDbAccess.Factory;

    public class AnimalController : Controller
    {
        private readonly MongoDbAccess.Interfaces.IAnimalCrud db = GetIAnimalCrud();
        // GET: AnimalController
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null) page = 1;
            else searchString = currentFilter;
            ViewData["CurrentFilter"] = searchString;

            var data = string.IsNullOrEmpty(searchString)
                ? await db.GetAnimalsByNameBeginsWith("a")
                : await db.GetAnimalsByNameBeginsWith(searchString);
            data = sortOrder switch
            {
                "name_desc" => data.OrderByDescending(x => x.Name).ToList(),
                _ => data.OrderBy(x => x.Name).ToList()
            };

            List<AnimalViewModel> animals = new();
            foreach (var animal in data)
            {
                animals.Add(ToAnimalViewModel(animal));
            }

            const int pageSize = 8;
            int pageNumber = (page ?? 1);

            return View(animals.ToPagedList(pageNumber, pageSize));
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
            var createAnimal = new AnimalViewModel() { OwnerId = ownerId };
            return View(createAnimal);
        }

        // POST: AnimalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AnimalViewModel animal)
        {
            if (ModelState.IsValid)
            {
                var output = ToAnimal(animal);
                await db.CreateAnimal(output);
                ViewData["Success"] = $"Animal {animal.Name} added successfully.";
                return View(animal);
            }
            return View(animal);
        }

        // GET: AnimalController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var viewAnimal = ToAnimalViewModel(await db.GetAnimalById(id));

            return View(viewAnimal);
        }

        // POST: AnimalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AnimalViewModel viewAnimal)
        {
            if (ModelState.IsValid)
            {
                await db.UpdateAnimal(ToAnimal(viewAnimal));
                ViewData["Success"] = "Animal updated.";
            }
            return View(viewAnimal);
        }

        // GET: AnimalController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var dbAnimal = await db.GetAnimalById(id);
            var viewAnimal = ToAnimalViewModel(dbAnimal);
            ViewData["OwnerId"] = viewAnimal.OwnerId;
            return View(viewAnimal);
        }

        // POST: AnimalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(AnimalViewModel animal)
        {
            ViewData["OwnerId"] = animal.OwnerId;
            if (animal.OwnerId != null)
            {
                await db.DeleteAnimal(ToAnimal(animal));
                ViewData["Success"] = "Animal deleted.";
            }
            return View();
        }
    }
}
