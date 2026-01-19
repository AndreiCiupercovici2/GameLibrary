using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Data;
using GameLibrary.Models;

namespace GameLibrary.Pages.Platforms
{
    public class DetailsModel : PageModel
    {
        private readonly GameLibrary.Data.GameLibraryContext _context;

        public DetailsModel(GameLibrary.Data.GameLibraryContext context)
        {
            _context = context;
        }

        public Platform Platform { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platform = await _context.Platform
                .Include(p => p.GamePlatforms)
                    .ThenInclude(gp => gp.Game)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (platform == null)
            {
                return NotFound();
            }
            else
            {
                Platform = platform;
            }
            return Page();
        }
    }
}
