﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelOfThings.Parser.Data;
using ModelOfThings.Parser.Models;

namespace ModelOfThings.Parser.Pages.Applications
{
    public class EditModel : PageModel
    {
        private readonly ModelOfThings.Parser.Data.ApplicationDbContext _context;

        public EditModel(ModelOfThings.Parser.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MddApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MddApplicationExists(MddApplication.Id))
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

        private bool MddApplicationExists(string id)
        {
            return _context.MddApplications.Any(e => e.Id == id);
        }
    }
}
