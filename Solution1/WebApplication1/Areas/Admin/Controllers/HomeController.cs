using Microsoft.AspNetCore.Mvc;
using WebApplication1.Areas.Admin.ViewModels.ItemVMs;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        Day2DbContext _db { get; }

        public HomeController(Day2DbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var data = _db.Items.Select(x => new ItemListItem
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ImagePath = x.ImagePath,
                IsDeleted = x.IsDeleted
            }).ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ItemCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            _db.Items.AddAsync(new Item
            {
                Title = vm.Title,
                Description = vm.Description,
                ImagePath = vm.ImagePath,
            });
            _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var data = await _db.Items.FindAsync(id);
            return View(new ItemUpdateVM
            {
                Title = data.Title,
                Description = data.Description,
                ImagePath = data.ImagePath,
            });

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, ItemUpdateVM vm)
        {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Items.FindAsync(id);
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.ImagePath = vm.ImagePath;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var data =await _db.Items.FindAsync(id);
            if (data == null) throw new Exception("NotFound");
            _db.Items.Remove(data);
            _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

