using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Data;
using GameLibrary.Models;

namespace GameLibrary.Pages.Games
{
    public class EditModel : PageModel
    {
        private readonly GameLibrary.Data.GameLibraryContext _context;

        public EditModel(GameLibrary.Data.GameLibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game Game { get; set; }
        [BindProperty]
        public List<int> SelectedGenreIds { get; set; }

        [BindProperty]
        public List<int> SelectedPlatformIds { get; set; }

        public List<SelectListItem> GenreOptions { get; set; }
        public List<SelectListItem> PlatformOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game =  await _context.Game
                .Include(g => g.Studio)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Game == null)
            {
                return NotFound();
            }
            GenreOptions = _context.Genre.Select(a => new SelectListItem
            {
                Value = a.ID.ToString(),
                Text = a.Name
            }).ToList();
            PlatformOptions = _context.Platform.Select(a => new SelectListItem
            {
                Value = a.ID.ToString(),
                Text = a.Name
            }).ToList();

            SelectedGenreIds = Game.GameGenres.Select(gg => gg.GenreID).ToList();
            SelectedPlatformIds = Game.GamePlatforms.Select(gp => gp.PlatformID).ToList();

            ViewData["StudioID"] = new SelectList(_context.Set<Studio>(), "ID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                 return NotFound();
            }

            var gameToUpdate = await _context.Game
                .Include(g => g.GameGenres)
                .Include(g => g.GamePlatforms)
                .Include(g => g.Studio)
                .FirstOrDefaultAsync(g => g.ID == id);

            if (gameToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Game>(
                gameToUpdate,
                "Game",
                g => g.Title, g => g.Price, g => g.StudioID))
            {
                UpdateGameGenres(SelectedGenreIds, gameToUpdate);
                UpdateGamePlatforms(SelectedPlatformIds, gameToUpdate);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");

            }

            UpdateGameGenres(SelectedGenreIds, gameToUpdate);
            return Page();
        }

        private void UpdateGameGenres(List<int> selectedGenreIds, Game gameToUpdate)
        {
            if (selectedGenreIds == null)
            {
                gameToUpdate.GameGenres = new List<GameGenre>();
                return;
            }

            var selectedGenresHS = new HashSet<int>(selectedGenreIds);
            var currentGenresHS = new HashSet<int>(gameToUpdate.GameGenres.Select(gg => gg.GenreID));

            foreach (var genre in _context.Genre)
            {
                if (selectedGenresHS.Contains(genre.ID))
                {
                    if (!currentGenresHS.Contains(genre.ID))
                    {
                        gameToUpdate.GameGenres.Add(new GameGenre { GameID = gameToUpdate.ID, GenreID = genre.ID });
                    }
                }
                else
                {
                    if (currentGenresHS.Contains(genre.ID))
                    {
                        var genreToRemove = gameToUpdate.GameGenres.FirstOrDefault(gg => gg.GenreID == genre.ID);
                        _context.Remove(genreToRemove);
                    }
                }
            }
        }

        private void UpdateGamePlatforms(List<int> selectedPlatformIds, Game gameToUpdate)
        {
            if (selectedPlatformIds == null)
            {
                gameToUpdate.GamePlatforms = new List<GamePlatform>();
                return;
            }
            var selectedPlatformsHS = new HashSet<int>(selectedPlatformIds);
            var currentPlatformsHS = new HashSet<int>(gameToUpdate.GamePlatforms.Select(gp => gp.PlatformID));
            foreach (var platform in _context.Platform)
            {
                if (selectedPlatformsHS.Contains(platform.ID))
                {
                    if (!currentPlatformsHS.Contains(platform.ID))
                    {
                        gameToUpdate.GamePlatforms.Add(new GamePlatform { GameID = gameToUpdate.ID, PlatformID = platform.ID });
                    }
                }
                else
                {
                    if (currentPlatformsHS.Contains(platform.ID))
                    {
                        var platformToRemove = gameToUpdate.GamePlatforms.FirstOrDefault(gp => gp.PlatformID == platform.ID);
                        _context.Remove(platformToRemove);
                    }
                }
            }
        }
    }
}
