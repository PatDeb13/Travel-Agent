using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Agent.Models.DTO
{
    public class ResponseDto<T>
    {

        public bool IsSuccessful{get; set;}
        public string Message{get; set;} =string.Empty;
          public T Data { get; set; }
        
    }

} 