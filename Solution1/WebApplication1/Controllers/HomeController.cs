using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.ViewModel.ItemVMs;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
       Day2DbContext _db { get; }

        public HomeController(Day2DbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Items.Where(x => !x.IsDeleted).Select(x => new ItemListItem 
            {
                Title=x.Title,
                Description=x.Description,
                ImagePath=x.ImagePath
            }).ToListAsync();
            return View(data);
        }

      
    }
}