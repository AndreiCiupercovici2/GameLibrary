using System.ComponentModel.DataAnnotations;

namespace GameLibrary.Models
{
    public class Genre
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Genre Name is mandatory")]
        [Display(Name = "Genre Name")]
        public string? Name { get; set; }
        public ICollection<GameGenre>? GameGenres { get; set; }
    }
}
