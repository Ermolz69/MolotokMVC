using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MolotokMvc.Models;

namespace MolotokMvc.Data
{
    public class MolotokDbContext : DbContext
    {
        public MolotokDbContext (DbContextOptions<MolotokDbContext> options)
            : base(options)
        {
        }

        public DbSet<MolotokMvc.Models.User> User { get; set; } = default!;
        public DbSet<MolotokMvc.Models.Skill> Skill { get; set; } = default!;
        public DbSet<MolotokMvc.Models.Tag> Tag { get; set; } = default!;
        public DbSet<MolotokMvc.Models.Resume> Resume { get; set; } = default!;
        public DbSet<MolotokMvc.Models.Vacancy> Vacancy { get; set; } = default!;
    }
}
