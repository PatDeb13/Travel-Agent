namespace Travel_Agent.Entities.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public string Description { get; set; }
        public Location location { get; set; } 
        
        
    }
}
