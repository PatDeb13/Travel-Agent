namespace Travel_Agent.Entities.Models
{
    public class Driver:BaseEntity
    {
    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public Vehicle Vehicle { get; set; }
       
                
        
    }
}
