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

namespace GameLibrary.Pages.Studios
{
    public class EditModel : PageModel
    {
        private readonly GameLibrary.Data.GameLibraryContext _context;

        public EditModel(GameLibrary.Data.GameLibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Studio Studio { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studio =  await _context.Studio.FirstOrDefaultAsync(m => m.ID == id);
            if (studio == null)
            {
                return NotFound();
            }
            Studio = studio;
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

            _context.Attach(Studio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudioExists(Studio.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool StudioExists(int id)
        {
            return _context.Studio.Any(e => e.ID == id);
        }
    }
}
