using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using webmvc.Models;

namespace webmvc.Controllers
{
    public class ExhibitionController : Controller
    {
        private readonly ArtContext _context;

        public ExhibitionController(ArtContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var viewModel = new ExhibitionSearchViewModel
            {
                SearchTerm = string.Empty, // Default to empty
                SortOrder = "Name", // Default sort by Name
                SortOrderOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Name", Text = "Name" },
                    new SelectListItem { Value = "Status", Text = "Status" },
                    new SelectListItem { Value = "StartDate", Text = "Start Date" },
                    new SelectListItem { Value = "EndDate", Text = "End Date" },
                    new SelectListItem { Value = "Location", Text = "Location" }
                },
                Exhibitions = _context.Exhibitions.ToList()
            };

            return View(viewModel);
        }

        // This action will be used for Ajax calls
        public IActionResult GetExhibitions(string searchTerm, string sortOrder)
        {
            var exhibitions = _context.Exhibitions.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                exhibitions = exhibitions.Where(a => a.Name.Contains(searchTerm));
            }

            // Sort based on the selected sort order
            if (sortOrder == "Name")
            {
                exhibitions = exhibitions.OrderBy(a => a.Name);
            }
            else if (sortOrder == "Status")
            {
                exhibitions = exhibitions.OrderBy(a => a.Status);
            }
            else if (sortOrder == "StartDate")
            {
                exhibitions = exhibitions.OrderBy(a => a.StartDate);
            }
            else if (sortOrder == "EndDate")
            {
                exhibitions = exhibitions.OrderBy(a => a.EndDate);
            }
            else if (sortOrder == "Location")
            {
                exhibitions = exhibitions.OrderBy(a => a.Location);
            }

            // Return a partial view with the updated artworks
            return PartialView("_ExhibitionList", exhibitions.ToList());
        }
    }
}
