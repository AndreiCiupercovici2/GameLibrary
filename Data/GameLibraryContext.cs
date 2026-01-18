using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;

namespace GameLibrary.Data
{
    public class GameLibraryContext : DbContext
    {
        public GameLibraryContext (DbContextOptions<GameLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<GameLibrary.Models.Game> Game { get; set; } = default!;
        public DbSet<GameLibrary.Models.Studio> Studio { get; set; } = default!;
        public DbSet<GameLibrary.Models.Genre> Genre { get; set; } = default!;
        public DbSet<GameLibrary.Models.GameGenre> GameGenre { get; set; } = default!;
        public DbSet<GameLibrary.Models.Platform> Platform { get; set; } = default!;
        public DbSet<GameLibrary.Models.GamePlatform> GamePlatform { get; set; } = default!;
    }
}
