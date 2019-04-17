using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;
using System;
using System.Threading.Tasks;

namespace ModelOfThings.Parser.Pages.Applications.CloudProviders
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public MddApplication MddApplication { get; set; }

        [BindProperty]
        public CloudProvider CloudProvider { get; set; }

        public async Task<IActionResult> OnGetAsync(string appId)
        {
            if (appId == null)
            {
                return NotFound();
            }

            MddApplication = await _context.MddApplications
                .FirstOrDefaultAsync(m => m.Id == appId);

            if (MddApplication == null)
            {
                return NotFound();
            }

            CloudProvider = new CloudProvider
            {
                MddApplicationId = MddApplication.Id
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CloudProviders.Add(CloudProvider);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}