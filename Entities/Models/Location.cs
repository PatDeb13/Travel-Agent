namespace Travel_Agent.Entities.Models
{
    public class Location :BaseEntity
    {

        public string Address{ get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string AssignAdmin { get; set; }
        
        public ICollection<Employee> Employees{get; set;}
        
    }
}
