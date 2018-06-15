using Microsoft.EntityFrameworkCore;
using PMS.APP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.APP.Context
{
    public class PMSContext : DbContext
    {
        public PMSContext(DbContextOptions<PMSContext> options) : base(options)
        {
        }
        public PMSContext()
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Milestones> Milestones { get; set; }
    }
}
