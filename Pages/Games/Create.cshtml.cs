using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GameLibrary.Data;
using GameLibrary.Models;

namespace GameLibrary.Pages.Games
{
    public class CreateModel : PageModel
    {
        private readonly GameLibrary.Data.GameLibraryContext _context;

        public CreateModel(GameLibrary.Data.GameLibraryContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Game Game { get; set; }

        [BindProperty]
        public List<int> SelectedGenreIDs { get; set; }
        [BindProperty]
        public List<int> SelectedPlatformIDs { get; set; }

        public List<SelectListItem> GenreOptions { get; set; }
        public List<SelectListItem> PlatformOptions { get; set; }

        public IActionResult OnGet()
        {
            ViewData["StudioID"] = new SelectList(_context.Set<Studio>(), "ID", "Name");
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

            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Game.Add(Game);
            await _context.SaveChangesAsync();

            if (SelectedGenreIDs != null)
            {
                foreach (var genreID in SelectedGenreIDs)
                {
                    var gameGenre = new GameGenre
                    {
                        GameID = Game.ID,
                        GenreID = genreID
                    };
                    _context.GameGenre.Add(gameGenre);
                }
            }

            if (SelectedPlatformIDs != null)
            {
                foreach (var platformID in SelectedPlatformIDs)
                {
                    var gamePlatform = new GamePlatform
                    {
                        GameID = Game.ID,
                        PlatformID = platformID
                    };
                    _context.GamePlatform.Add(gamePlatform);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
