using SeminarEF.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SeminarEF
{
    public class MessageSource : IMessageSource
    {
        private  readonly UdpClient udpClient;

        public MessageSource(int port) => udpClient = new UdpClient(port);
        public MessageUdp Receive(ref IPEndPoint iPEndPoint)
        {
            
            var recieved = udpClient.Receive(ref iPEndPoint);

            return MessageUdp.FromJson(Encoding.UTF8.GetString(recieved)) ?? new MessageUdp();
        }

        public void Send(MessageUdp messageUdp , IPEndPoint iPEndPoint)
        {
            var json = messageUdp.ToJson();
            udpClient.Send(Encoding.UTF8.GetBytes(json), json.Length, iPEndPoint);
        }
    }
}
