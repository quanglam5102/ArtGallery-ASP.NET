using System.Diagnostics.Metrics;

namespace webmvc.Models
{
    public class Art
    {
        public int ArtId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int? ExhibitionId { get; set; }
        public int YearCreated { get; set; }
        public string Medium { get; set; }
        public decimal Price { get; set; }
        public string? UploadImage { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Exhibition? Exhibition { get; set; }
    }
}


