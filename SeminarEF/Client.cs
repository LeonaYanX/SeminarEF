using SeminarEF.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace SeminarEF
{
    public class Client
    {
        private readonly IMessageSource _messageSource;

        private readonly IPEndPoint _ipEndPoint;

        private readonly string _name;

        public Client (IMessageSource messageSource, IPEndPoint ipEndPoint, string name)
        {
            _messageSource = messageSource;
            _ipEndPoint = ipEndPoint;
            _name = name;
        }

        public void RegisterMe() 
        {
            var messageUdp = new MessageUdp { Command = Command.Register, FromName = _name };
            var server = new Server(new MessageSource (8080)); // new UdpClient(8080);
            _messageSource.Send(messageUdp, _ipEndPoint);
        }

        public void ClientSender()
        {
           

            while (true) 
            {
                Console.WriteLine("Enter message: ");
                string message = Console.ReadLine() ?? "";
                Console.WriteLine("Enter the recipient: ");
                string recipientTo = Console.ReadLine() ?? "";
                if (String.IsNullOrEmpty(message) || String.IsNullOrEmpty(recipientTo)) 
                { continue; }

                var messageUdp = new MessageUdp { Command = Command.Message, FromName = _name, Text = message , ToName = recipientTo};
               /* if (messageUdp.Text == "exit") 
                {
                    break;
                }*/

                _messageSource.Send(messageUdp, _ipEndPoint);
            
            }
        }

        public void ClientListener()
        {
            RegisterMe();

            var ipEP = new IPEndPoint(_ipEndPoint.Address, _ipEndPoint.Port);

            while (true) 
            {    
              var mesUdp = _messageSource.Receive(ref ipEP);

              Console.WriteLine(mesUdp.ToString());
            }
        }

    }
}
