using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using webmvc.Models;

namespace webmvc.Controllers
{
    public class ArtController : Controller
    {
        private readonly ArtContext _context;

        public ArtController(ArtContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            var viewModel = new ArtworkSearchViewModel
            {
                SearchTerm = string.Empty, // Default to empty
                SortOrder = "Price", // Default sort by Price
                SortOrderOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Price", Text = "Price" },
                    new SelectListItem { Value = "Title", Text = "Title" },
                    new SelectListItem { Value = "YearCreated", Text = "Year Created" }
                },
                Artworks = _context.Arts.ToList()
            };

            return View(viewModel);
        }

        // This action will be used for Ajax calls
        public IActionResult GetArtworks(string searchTerm, string sortOrder)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            var artworks = _context.Arts.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                artworks = artworks.Where(a => a.Title.Contains(searchTerm));
            }

            // Sort based on the selected sort order
            if (sortOrder == "Price")
            {
                artworks = artworks.OrderBy(a => a.Price);
            }
            else if (sortOrder == "Title")
            {
                artworks = artworks.OrderBy(a => a.Title);
            }
            else if (sortOrder == "YearCreated")
            {
                artworks = artworks.OrderBy(a => a.YearCreated);
            }

            // Return a partial view with the updated artworks
            return PartialView("_ArtworkList", artworks.ToList());
        }

        public ActionResult Details(int artistId)
        {
            var arts = _context.Arts
                .Where(e => e.ArtistId == artistId)
                .ToList();

            if (arts == null || !arts.Any())
            {
                return NotFound();
            }

            return View(arts);
        }

        public ActionResult Delete(int artId)
        {
            var art = _context.Arts.FirstOrDefault(e => e.ArtId == artId);

            if (art == null)
            {
                return NotFound();
            }

            _context.Arts.Remove(art);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult View(int id)
        {
            var artwork = _context.Arts.FirstOrDefault(a => a.ArtId == id);
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            if (artwork == null)
            {
                return NotFound();
            }

            return View(artwork);
        }

        [HttpPost]
        public IActionResult UploadImage(int artId, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Generate a unique file name
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                // Save the file to the wwwroot/Images folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // Save the file name in the database
                var art = _context.Arts.FirstOrDefault(a => a.ArtId == artId);
                if (art != null)
                {
                    art.UploadImage = fileName;
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        // Main view with initial data
        public IActionResult FilterArts()
        {
            ViewBag.Artists = new SelectList(_context.Artists, "ArtistId", "Name");
            return View();
        }

        // Fetch Exhibitions by Artist
        [HttpGet]
        public JsonResult GetExhibitionsByArtist(int artistId)
        {
            var exhibitions = _context.Exhibitions
                .Where(e => _context.Arts.Any(a => a.ArtistId == artistId && a.ExhibitionId == e.Id))
                .Select(e => new { e.Id, e.Name })
                .ToList();

            return Json(exhibitions);
        }

        // Fetch Arts by Artist and Exhibition
        [HttpGet]
        public JsonResult GetArtsByArtistAndExhibition(int artistId, int exhibitionId)
        {
            var arts = _context.Arts
                .Where(a => a.ArtistId == artistId && a.ExhibitionId == exhibitionId)
                .Select(a => new { a.ArtId, a.Title })
                .ToList();
            return Json(arts);
        }


    }
}
