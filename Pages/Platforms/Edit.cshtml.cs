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

namespace GameLibrary.Pages.Platforms
{
    public class EditModel : PageModel
    {
        private readonly GameLibrary.Data.GameLibraryContext _context;

        public EditModel(GameLibrary.Data.GameLibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Platform Platform { get; set; }

        [BindProperty]
        public List<int> SelectedGameIDs { get; set; }

        public List<SelectListItem> AllGames { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platform =  await _context.Platform
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gg => gg.Game)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (platform == null)
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

            SelectedGameIDs = platform.GamePlatforms?.Select(gg => gg.GameID).ToList() ?? new List<int>();
            Platform = platform;
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
            var platformToUpdate = await _context.Platform
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gg => gg.Game)
                .FirstOrDefaultAsync(g => g.ID == Platform.ID);

            if (platformToUpdate == null)
            {
                return NotFound();
            }    

            if (await TryUpdateModelAsync<Platform>(
                platformToUpdate,
                "Platform",
                g => g.Name))
            {
                UpdatePlatformGames(SelectedGameIDs, platformToUpdate);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            UpdatePlatformGames(SelectedGameIDs, platformToUpdate);
            return Page();
        }

        private void UpdatePlatformGames(List<int> selectedGameIDs, Platform platformToUpdate)
        {
            if (selectedGameIDs == null)
            {
                platformToUpdate.GamePlatforms = new List<GamePlatform>();
                return;
            }

            selectedGameIDs.Remove(-1);

            var selectedGamesHS = new HashSet<int>(selectedGameIDs);
            var currentGamesHS = new HashSet<int>(platformToUpdate.GamePlatforms.Select(gg => gg.GameID));

            foreach (var game in _context.Game)
            {
                if (selectedGamesHS.Contains(game.ID))
                {
                    if(!currentGamesHS.Contains(game.ID))
                    {
                        platformToUpdate.GamePlatforms.Add(new GamePlatform
                        {
                            PlatformID = platformToUpdate.ID,
                            GameID = game.ID
                        });
                    }
                }
                else
                {
                    if (currentGamesHS.Contains(game.ID))
                    {
                        var gameToRemove = platformToUpdate.GamePlatforms.FirstOrDefault(gg => gg.GameID == game.ID);
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
