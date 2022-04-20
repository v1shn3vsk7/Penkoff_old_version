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
    public class IndexModel : PageModel
    {
        private readonly Penkoff_ASP.NET_Core_.Data.UsersContext _context;

        public IndexModel(Penkoff_ASP.NET_Core_.Data.UsersContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; }

        public async Task OnGetAsync()
        {
            User = await _context.Users.ToListAsync();
        }
    }
}
