using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;

namespace ModelOfThings.Parser.Pages.Applications
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MddApplication MddApplication { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MddApplication = await _context.MddApplications.FirstOrDefaultAsync(m => m.Id == id);

            if (MddApplication == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
