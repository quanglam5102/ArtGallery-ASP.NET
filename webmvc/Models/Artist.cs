namespace webmvc.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public int BirthYear { get; set; }
        public string Nationality { get; set; }
        public string Biography { get; set; }

        public virtual ICollection<Art> ArtPieces { get; set; } = new List<Art>();
    }

}
