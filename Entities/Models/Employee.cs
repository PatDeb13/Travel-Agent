using System.ComponentModel.DataAnnotations.Schema;

namespace Travel_Agent.Entities.Models
{
    public class Employee :BaseEntity
    {
    
        public string EmployeeId {get; set;}
        public string FirstName { get; set; }
        public string LastName{get; set;}
        public string Description { get; set; }              
        public Location Location {get; set;}
         public int LocationId { get; set; }
        public int SubsidiaryId { get; set; }
        public Subsidiary Subsidiary { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    

    }

   
   
}
