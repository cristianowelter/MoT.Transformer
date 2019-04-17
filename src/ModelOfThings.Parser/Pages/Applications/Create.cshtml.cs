using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;

namespace ModelOfThings.Parser.Pages.Applications
{
    public class CreateModel : PageModel
    {
        private readonly ModelOfThings.Parser.Data.ApplicationDbContext _context;

        public CreateModel(ModelOfThings.Parser.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public MddApplication MddApplication { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MddApplications.Add(MddApplication);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = MddApplication.Id });
        }
    }
}