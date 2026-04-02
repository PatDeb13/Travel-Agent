using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Entities.Models
{
    public class Subsidiary:BaseEntity
    {
        public int Id {get; set;}
        public string Name{get; set;}
        public ICollection<Employee> Employees { get; set; }
    }
}