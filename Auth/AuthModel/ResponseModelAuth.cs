using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Auth
{
    public class ResponseModelAuth <T>
    {
     public string Message {get; set;}  
     public bool IsSuccessful {get; set;}
     public T    Data{get; set;}

      
    }
}