using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Entities.Models
{
    public abstract class BaseEntity
    {
        public int Id{get; set;}
        public DateTime CreatedAt{get; set;}=DateTime.Now;
         public DateTime UpdatedAt{get; set;}

    }
}