using Microsoft.AspNetCore.Mvc.Rendering;

namespace webmvc.Models
{
    public class ExhibitionSearchViewModel
    {
        public string SearchTerm { get; set; }
        public string SortOrder { get; set; }
        public List<SelectListItem> SortOrderOptions { get; set; }

        public List<Exhibition> Exhibitions { get; set; }
    }
}
