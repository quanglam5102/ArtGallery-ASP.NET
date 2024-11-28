using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using webmvc.Models;

namespace webmvc.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ArtContext _context;

        public ArtistController(ArtContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            var viewModel = new ArtistSearchViewModel
            {
                SearchTerm = string.Empty, // Default to empty
                SortOrder = "Name", // Default sort by Artist Name
                SortOrderOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Name", Text = "Name" },
                    new SelectListItem { Value = "Nationality", Text = "Nationality" },
                    new SelectListItem { Value = "BirthYear", Text = "Birth Year" }
                },
                Artists = _context.Artists.ToList()
            };
            return View(viewModel);
        }

        // This action will be used for Ajax calls
        public IActionResult GetArtists(string searchTerm, string sortOrder)
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;

            var artists = _context.Artists.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                artists = artists.Where(a => a.Name.Contains(searchTerm));
            }

            // Sort based on the selected sort order
            if (sortOrder == "Name")
            {
                artists = artists.OrderBy(a => a.Name);
            }
            else if (sortOrder == "Nationality")
            {
                artists = artists.OrderBy(a => a.Nationality);
            }
            else if (sortOrder == "BirthYear")
            {
                artists = artists.OrderBy(a => a.BirthYear);
            }

            // Return a partial view with the updated artworks
            return PartialView("_ArtistList", artists.ToList());
        }

        //Add New Artist
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Artist artist)
        {
            if (ModelState.IsValid)
            {
                _context.Artists.Add(artist);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(artist);
        }

        //Edit Artist
        [HttpGet]
        public ActionResult Edit(int artistId)
        {
            var artist = _context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);  // Send artist to the Edit view
        }

        // POST: Artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Artist artist)
        {
            if (ModelState.IsValid)
            {
                var artistInDb = _context.Artists.FirstOrDefault(a => a.ArtistId == artist.ArtistId);
                if (artistInDb == null)
                {
                    return NotFound();
                }

                // Update artist fields
                artistInDb.Name = artist.Name;
                artistInDb.BirthYear = artist.BirthYear;
                artistInDb.Nationality = artist.Nationality;
                artistInDb.Biography = artist.Biography;

                // Save changes to database
                _context.SaveChanges();

                // Redirect to the artist index after successful update
                return RedirectToAction("Index");
            }
            // If validation fails, return the same view with validation messages
            return View(artist);
        }
    }
}
