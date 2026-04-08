using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Entities.Models
{
    public class Dashboard:BaseEntity
    {
        
        public string? TotalTravelRequest{get; set;}
        public string?PendingTravelRequest{get; set;}
        public string?ApprovedTravelRequest{get;set;}
        public string DeclinedTrvelRequest{get; set;}
    }
}