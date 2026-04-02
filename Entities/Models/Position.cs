using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Entities.Models
{
    public class Position:BaseEntity
    {
        public string NamePost {get; set;}

        public ICollection<Employee> Employees { get; set; }
    
        
    }
}