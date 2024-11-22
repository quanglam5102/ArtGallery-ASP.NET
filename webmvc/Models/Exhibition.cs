using System;
using System.ComponentModel.DataAnnotations;

namespace webmvc.Models
{
    public class Exhibition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(150)]
        public string Location { get; set; }
    }
}
