using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travel_Agent.Auth;

namespace Travel_Agent.Entities.Models.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
            
        }
        public DbSet<Employee> Employees {get; set;}
        public DbSet<Driver> Drivers {get; set;}
        public DbSet<Location>  Locations {get; set;}
         public DbSet<Vehicle> Vehicles {get; set;}
         public DbSet<Subsidiary> Subsidiaries{get; set;}
         public DbSet<Position>Positions {get; set;}
          public DbSet<Dashboard> Dashboards{get; set;}


        
    }

}