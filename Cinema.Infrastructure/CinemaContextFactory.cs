using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure
{
    public class CinemaContextFactory : IDesignTimeDbContextFactory<CinemaContext>
    {
        public CinemaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CinemaContext>();
            optionsBuilder.UseSqlite("Data Source=cinema.db");

            return new CinemaContext(optionsBuilder.Options);
        }
    }
}
