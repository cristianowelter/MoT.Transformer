using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelOfThings.Parser.Pages.Applications
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MddApplication> MddApplications { get; set; }

        public async Task OnGetAsync()
        {
            MddApplications = await _context.MddApplications.ToListAsync();
        }
    }
}
