using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel_Agent.Entities.Enums;

namespace Travel_Agent.Entities.Models
{
    public class Document
    {
        public int TravelRequestId{get; set;}
        public DocumentType Type{get; set;}
        public string FileName{get;set;}
        public string FilePath{get;set;}
        public TravelRequest TravelRequest{get; set;}
    }
}