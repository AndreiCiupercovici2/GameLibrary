using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Studio
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Studio Name is mandatory")]
        [Display(Name = "Studio Name")]
        public string? Name { get; set; }
        public string? Country { get; set; }
        public ICollection<Game>? Games { get; set; }
    }
}
