using Travel_Agent.Entities.Enums;

namespace Travel_Agent.Entities.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string NumberofSeat { get; set; }
        public string Year { get; set; }
        public int Colour { get; set; }
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public Location location { get; set; } 
        public Driver Driver {get; set; }
        public VechicleMode Mode{get; set;}
        public TravelRequest TravelRequest{get; set;}
                
        
    }
}
