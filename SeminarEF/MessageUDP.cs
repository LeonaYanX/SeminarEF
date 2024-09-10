using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace SeminarEF
{
   
    
        public enum Command
        {
            Register,
            Message,
            Confirmation
        }
        public class MessageUdp 
        {
     
            public Command Command { get; set; }
            public int? Id { get; set; }
            public string FromName { get; set; }
            public string ToName { get; set; }
            public string Text { get; set; }
            //To Json serialization method
            public string ToJson() 
            {
            return JsonSerializer.Serialize(this);
            }

            //From Json deserialization method
            public static MessageUdp? FromJson(string json)
            {
            return JsonSerializer.Deserialize<MessageUdp>(json);
            }


       
        }
    
}
