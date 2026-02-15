using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Genre
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Error_TitleRequired")]
        [Display(Name = "Genre")]
        public string? Name { get; set; }
        [Display(Name = "Games")]
        public ICollection<GameGenre>? GameGenres { get; set; }
    }
}
