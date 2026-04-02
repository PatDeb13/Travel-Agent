namespace Travel_Agent.Entities.Models
{
    public class Location :BaseEntity
    {

        public string Branchcode{ get; set; }
        public string Branch { get; set; }
        
        public ICollection<Employee> Employees{get; set;}
        
    }
}
