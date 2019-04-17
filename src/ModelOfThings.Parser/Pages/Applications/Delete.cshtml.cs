using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;
using System.Threading.Tasks;

namespace ModelOfThings.Parser.Pages.Applications
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MddApplication MddApplication { get; set; }

        public async Task<IActionResult> OnGetAsync(string appId)
        {
            if (appId == null)
            {
                return NotFound();
            }

            MddApplication = await _context.MddApplications.FirstOrDefaultAsync(m => m.Id == appId);

            if (MddApplication == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string appId)
        {
            if (appId == null)
            {
                return NotFound();
            }

            MddApplication = await _context.MddApplications
                .Include(a => a.UmlUseCases)
                    .ThenInclude(b => b.MddComponents)
                    .ThenInclude(c => c.MddProperties)
                .Include(a => a.UmlUseCases)
                    .ThenInclude(b => b.Associations)
                .Include(a => a.CloudProviders)
                .Include(a => a.UmlModels)
                .SingleOrDefaultAsync(m => m.Id == appId);

            if (MddApplication != null)
            {
                _context.MddApplications.Remove(MddApplication);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
