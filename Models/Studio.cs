using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Studio
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Studio Name is mandatory")]
        [Display(Name = "StudioName")]
        public string? Name { get; set; }
        [Display(Name = "Country")]
        public string? Country { get; set; }
        [Display(Name = "Games")]
        public ICollection<Game>? Games { get; set; }
    }
}
