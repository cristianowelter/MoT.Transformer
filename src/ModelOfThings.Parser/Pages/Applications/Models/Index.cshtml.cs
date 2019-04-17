using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;

namespace ModelOfThings.Parser.Pages.Applications.Models
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MddApplication MddApplication { get; set; }

        public async Task<IActionResult> OnGetAsync(string appId)
        {
            if (appId == null)
            {
                return NotFound();
            }

            MddApplication = await _context.MddApplications
                .Include(m => m.UmlModels)
                .FirstOrDefaultAsync(m => m.Id == appId);

            if (MddApplication == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}