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

//ALTER TABLE Arts
//ADD UploadImage VARCHAR(50);
//ALTER TABLE Arts
//ADD ExhibitionId INT NULL;
//ALTER TABLE Arts
//ADD CONSTRAINT FK_Arts_Exhibition
//FOREIGN KEY (ExhibitionId) REFERENCES Exhibitions(Id);


