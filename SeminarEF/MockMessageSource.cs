using SeminarEF.Abstraction;
using SeminarEF.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SeminarEF
{ //  2:06:23 seminar@
    public class MockMessageSource:IMessageSource
    {
        private Queue<MessageUdp> messages = new Queue<MessageUdp>();
  

      public MockMessageSource()
        {
            
            messages.Enqueue(new MessageUdp() { Command = Command.Register, FromName="User1"  });
            messages.Enqueue(new MessageUdp() {Command = Command.Register,FromName="User2" });
            messages.Enqueue(new MessageUdp() { Command = Command.Message, FromName = "User1", ToName = "User2", Text = "Hello from User1" });
            messages.Enqueue(new MessageUdp() { Command= Command.Message,FromName="User2",ToName="User1",Text="Hello from User2" });
      }
       

        public MessageUdp Receive(ref IPEndPoint iPEndPoint)
        {
           return messages.Dequeue();
        }

        public void Send(MessageUdp messageUdp, IPEndPoint iPEndPoint) 
        {
            messages.Enqueue(messageUdp);
        }

        public void SendResieve(MessageUdp messageUdp)
        {
        
        }
    }
}
