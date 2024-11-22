namespace webmvc.Models
{
    public class Art
    {
        public int ArtId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int YearCreated { get; set; }
        public string Medium { get; set; }
        public decimal Price { get; set; }

        public virtual Artist Artist { get; set; }
    }

}
