using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Platform
    {
        public int ID { get; set; }
        [Display(Name = "Platform")]
        [Required(ErrorMessage = "Error_TitleRequired")]
        public required string Name { get; set; }
        [Display(Name = "Manufacturer")]
        public string? Manufacturer { get; set; }
        [Display(Name = "Games")]

        public ICollection<GamePlatform>? GamePlatforms { get; set; }
    }
}
