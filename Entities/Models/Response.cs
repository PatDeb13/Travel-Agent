using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Entities.Models
{
    public class Response<T> 
    {
        public string Message{get; set;}
        public bool IsSuccessful{get; set;}
        public T  Data {get; set;}
    }
}