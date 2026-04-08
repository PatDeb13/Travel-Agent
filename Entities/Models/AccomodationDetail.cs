using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Entities.Models
{
    public class AccomodationDetail
    {
      public int Id { get; set; }

    public int TravelRequestId { get; set; }

    public string HotelName { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public string RoomType { get; set; }
    public TravelRequest TravelRequest{get; set;}
    }
}