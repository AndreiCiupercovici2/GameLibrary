namespace GameLibrary.Models
{
    public class GameGenre
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public Game? Game { get; set; }
        public int GenreID { get; set; }
        public Genre? Genre { get; set; }
    }
}
