using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel_Agent.Entities.Enums;

namespace Travel_Agent.Entities.Models
{
    public class TravelRequest
    {
        public int Id{get; set;}
        public Status Status{get; set;}=Status.pending;
        public DateTime DepatureDate{get; set;}
        public TimeSpan DepartureTime{get; set;}
    }
}