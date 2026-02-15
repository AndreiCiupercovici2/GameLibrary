using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Game
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Error_TitleRequired")]
        [Display(Name = "GameTitle")]
        public string? Title { get; set; }
        [Range(0.00, 59.99, ErrorMessage = "Error_PriceRange")]
        [DataType(DataType.Currency)]
        [Display(Name = "Price" )]
        public decimal Price { get; set; }
        [Display(Name = "StudioName")]
        public int StudioID { get; set; }
        public Studio? Studio { get; set; } //navigation property

        [Display(Name = "Platform")]
        public ICollection<GamePlatform>? GamePlatforms { get; set; } //navigation property
        [Display(Name = "Genre")]
        public ICollection<GameGenre>? GameGenres { get; set; } //navigation property
    }
}
