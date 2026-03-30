namespace Travel_Agent.Entities.Models
{
    public class Drivers
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Employee employeesId { get; set; } 
        public string LicenseNumber { get; set; }

    }
}
