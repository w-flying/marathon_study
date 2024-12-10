using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marathon.Models;
using Marathon.Models;

namespace Marathon.Data
{
    public class MarathonContext : DbContext
    {
        public MarathonContext (DbContextOptions<MarathonContext> options)
            : base(options)
        {
        }

        public DbSet<marathon.Models.User> User { get; set; } = default!;
        public DbSet<marathon.Models.Manager> Manager { get; set; } = default!;
        public DbSet<marathon.Models.Marathonentity> Marathonentity { get; set; } = default!;
        public DbSet<marathon.Models.Enroll> Enroll { get; set; } = default!;
        public DbSet<marathon.Models.Check> Check { get; set; } = default!;
        public DbSet<Marathon.Models.Post> Post { get; set; } = default!;
    }
}
