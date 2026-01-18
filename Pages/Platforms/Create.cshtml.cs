using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GameLibrary.Data;
using GameLibrary.Models;

namespace GameLibrary.Pages.Platforms
{
    public class CreateModel : PageModel
    {
        private readonly GameLibrary.Data.GameLibraryContext _context;

        public CreateModel(GameLibrary.Data.GameLibraryContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Platform Platform { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            _context.Platform.Add(Platform);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
