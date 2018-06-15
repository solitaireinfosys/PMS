using Microsoft.EntityFrameworkCore;
using PMS.DATA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.DATA.Context
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
