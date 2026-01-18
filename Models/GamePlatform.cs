namespace GameLibrary.Models
{
    public class GamePlatform
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public Game? Game { get; set; }
        public int PlatformID { get; set; }
        public Platform? Platform { get; set; }
    }
}
