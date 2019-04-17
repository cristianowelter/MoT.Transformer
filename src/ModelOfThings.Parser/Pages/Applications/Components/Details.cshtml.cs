using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;

namespace ModelOfThings.Parser.Pages.Applications.Components
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MddComponent MddComponent { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MddComponent = await _context.MddComponents
                .Include(x => x.MddApplication)
                .Include(x => x.MddProperties).FirstOrDefaultAsync(m => m.Id == id);

            if (MddComponent == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MddProperties.UpdateRange(MddComponent.MddProperties);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = MddComponent.Id });
        }
    }
}