#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Penkoff_ASP.NET_Core_.Data;
using Penkoff_ASP.NET_Core_.Models;

namespace Penkoff_ASP.NET_Core_.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly Penkoff_ASP.NET_Core_.Data.UsersContext _context;

        public DetailsModel(Penkoff_ASP.NET_Core_.Data.UsersContext context)
        {
            _context = context;
        }

        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
