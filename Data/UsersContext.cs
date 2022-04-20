#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Penkoff_ASP.NET_Core_.Models;

namespace Penkoff_ASP.NET_Core_.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext (DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        //public DbSet<Penkoff_ASP.NET_Core_.Models.User> User { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
