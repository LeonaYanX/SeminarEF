using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SeminarEF.Abstraction
{
    public interface IMessageSource
    {
        void Send(MessageUdp messageUdp , IPEndPoint iPEndPoint);

         MessageUdp Receive(ref IPEndPoint iPEndPoint);
    }
}
