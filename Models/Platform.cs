using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Platform
    {
        public int ID { get; set; }
        [Display(Name = "Platform Name")]
        [Required(ErrorMessage = "Platform Name is mandatory")]
        public required string Name { get; set; }
        public string? Manufacturer { get; set; }

        public ICollection<GamePlatform>? GamePlatforms { get; set; }
    }
}
