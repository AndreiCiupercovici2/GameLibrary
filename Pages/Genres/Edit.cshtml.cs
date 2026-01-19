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

namespace GameLibrary.Pages.Genres
{
    public class EditModel : PageModel
    {
        private readonly GameLibrary.Data.GameLibraryContext _context;

        public EditModel(GameLibrary.Data.GameLibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Genre Genre { get; set; }

        [BindProperty]
        public List<int> SelectedGameIds { get; set; }
        public List<SelectListItem> AllGames { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre =  await _context.Genre
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Game)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (genre == null)
            {
                return NotFound();
            }

            AllGames = await _context.Game
                .Select(g => new SelectListItem
                {
                    Value = g.ID.ToString(),
                    Text = g.Title
                })
                .ToListAsync();

            AllGames.Insert(0, new SelectListItem
            {
                Value = "-1",
                Text = "-- None --"
            });

            SelectedGameIds = genre.GameGenres?.Select(gg => gg.GameID).ToList() ?? new List<int>();
            Genre = genre;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var genreToUpdate = await _context.Genre
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Game)
                .FirstOrDefaultAsync(g => g.ID == Genre.ID);

            if (genreToUpdate == null)
            {
                return NotFound();
            }
            
            if (await TryUpdateModelAsync<Genre>(
                genreToUpdate,
                "Genre",
                g => g.Name))
            {
                UpdateGenreGames(SelectedGameIds, genreToUpdate);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            UpdateGenreGames(SelectedGameIds, genreToUpdate);
            return Page();
        }

        private void UpdateGenreGames(List<int> selectedGameIDs, Genre genreToUpdate)
        {
            if (selectedGameIDs == null)
            {
                genreToUpdate.GameGenres = new List<GameGenre>();
                return;
            }

            selectedGameIDs.Remove(-1);

            var selectedGamesHS = new HashSet<int>(selectedGameIDs);
            var currentGamesHS = new HashSet<int>(genreToUpdate.GameGenres.Select(gg => gg.GameID));

            foreach (var game in _context.Game)
            {
                if (selectedGamesHS.Contains(game.ID))
                {
                    if (!currentGamesHS.Contains(game.ID))
                    {
                        genreToUpdate.GameGenres.Add(new GameGenre
                        {
                            GenreID = genreToUpdate.ID,
                            GameID = game.ID
                        });
                    }
                }
                else
                {
                    if (currentGamesHS.Contains(game.ID))
                    {
                        var gameToRemove = genreToUpdate.GameGenres.FirstOrDefault(gg => gg.GameID == game.ID);
                        if (gameToRemove != null)
                        {
                            _context.Remove(gameToRemove);
                        }
                    }
                }
            }
        }
    }
}
