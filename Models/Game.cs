using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Game
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Game Title is mandatory")]
        [Display(Name = "Game Title")]
        public string? Title { get; set; }
        [Range(0.00, 59.99, ErrorMessage = "Price must be between 0.00 and 59.99")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int StudioID { get; set; }
        public Studio? Studio { get; set; } //navigation property

        public ICollection<GamePlatform>? GamePlatforms { get; set; } //navigation property
        public ICollection<GameGenre>? GameGenres { get; set; } //navigation property
    }
}
