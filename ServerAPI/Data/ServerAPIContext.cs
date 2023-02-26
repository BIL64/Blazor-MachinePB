using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerAPI.Entities;

namespace ServerAPI.Data
{
    public class ServerAPIContext : DbContext
    {
        public ServerAPIContext (DbContextOptions<ServerAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Machine> Machine { get; set; } = default!;
    }
}
