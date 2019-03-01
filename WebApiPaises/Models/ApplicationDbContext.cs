using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApiPaises.Models
{
    //heredamos del identity
    public class ApplicationDbContext : IdentityDbContext<ApplicationUsercs>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) //devuelve a su padre las opciones que le pasamos al constructor
        {

        }

        public DbSet <Pais> Paises { get; set; }
        public DbSet <Provincia> provincias { get; set; }
    }
}
