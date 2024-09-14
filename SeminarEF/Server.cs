using SeminarEF.Abstraction;
using SeminarEF.Db.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SeminarEF
{
    public class Server
    {
        Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();

       //private readonly UdpClient _udpClient;
        IMessageSource _messageSource;

        public Server(IMessageSource messageSource)
        {
            _messageSource = messageSource;
            //_udpClient = new UdpClient(8080);

        }


        public void RegisterClient(MessageUdp messageUdp, IPEndPoint fromiPEndPoint)
        {
            Console.WriteLine("RegisterClient called:" + messageUdp.FromName);

            clients.Add(messageUdp.FromName, fromiPEndPoint);

            using (ChatDbContext db = new ChatDbContext())
            {
                if (db.Users.FirstOrDefault(x => x.Name == messageUdp.FromName) != null)
                {
                    return;
                }
                else
                {
                    db.Users.Add(new Users() { Name = messageUdp.FromName });
                    db.SaveChanges();
                }
            }

        }
        public void Confirmation(int? id)
        {
            Console.WriteLine("Confirmation message with id:" + id);

            using (ChatDbContext db = new ChatDbContext())
            {
                var message = db.Messages.FirstOrDefault(x => x.Id == id);
                if (message != null)
                {
                    message.IsReceived = true;
                    db.SaveChanges();
                }
            }

        }

        public void SendMessage(MessageUdp messagesUdp)
        {
            int? id = null;
            if (clients.TryGetValue(messagesUdp.ToName, out IPEndPoint? iPEndPoint))
            {


                using (ChatDbContext db = new ChatDbContext())
                {
                    var fromUser = db.Users.First(x => x.Name == messagesUdp.FromName);
                    var toUser = db.Users.First(x => x.Name == messagesUdp.ToName);
                    var message = new Messages()
                    {
                        FromUser = fromUser,
                        ToUser = toUser,
                        Text = messagesUdp.Text,
                        IsReceived = false

                    };
                    db.Messages.Add(message);
                    db.SaveChanges();
                    id = message.Id;

                    var forwardMessageUdp = new MessageUdp()
                    {
                        Command = Command.Message,
                        Id = id,
                        FromName = messagesUdp.FromName,
                        ToName = messagesUdp.ToName,
                        Text = messagesUdp.Text

                    };

                    _messageSource.Send(forwardMessageUdp, iPEndPoint);

                   // _udpClient.Send(Encoding.UTF8.GetBytes(forwardMessageUdp), forwardMessageUdp.Length, iPEndPoint);

                    Console.WriteLine($"Message Relied, from = {messagesUdp.FromName} to = {messagesUdp.ToName}");
                }
            }
            else
            {
                Console.WriteLine($"Client with name {messagesUdp.ToName} not found");
            }
        }

        public void ProcessMessage(MessageUdp messageUdp, IPEndPoint fromiPEndPoint)
        {
            Console.WriteLine($"Processing message from:{messageUdp.FromName} to:{messageUdp.ToName}");

            switch (messageUdp.Command)
            {
                case Command.Register:
                    Console.WriteLine("Registering client");
                    RegisterClient(messageUdp, fromiPEndPoint);
                    break;
                case Command.Message:
                    Console.WriteLine("Message sending");
                    SendMessage(messageUdp);
                    break;
                case Command.Confirmation:
                    Console.WriteLine("Confirming message");
                    Confirmation(messageUdp.Id);
                    break;
                case Command.SendListOfUnreadMessages:
                    Console.WriteLine("Sending list of unread messages");
                    SendListOfUnreadMessages(messageUdp.FromName);
                    break;
                default:
                    break;
            }
        }

        public void Start()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Server started");
            while (true)
            {
               
                            

                try
                {
                    var messageUdp = _messageSource.Receive(ref iPEndPoint);   //_udpClient.Receive(ref iPEndPoint);
                    Console.WriteLine(messageUdp.ToString());
                    ProcessMessage(messageUdp, iPEndPoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while receiving message:" + e.Message);
                }


            }
        }
        // Реализуйте тип сообщений List, при котором клиент будет получать все непрочитанные сообщения с сервера.
        // метод отпрвки листа непрочитанных сообщений пользователю 
        public void SendListOfUnreadMessages(string userName)
        {
            // проверка наличия пользователя в списке клиентов
            if (clients.TryGetValue(userName, out IPEndPoint? iPEndPoint))
            {
                using (ChatDbContext db = new ChatDbContext())
                {
                    var user = db.Users.First(x => x.Name == userName);
                    // получение списка непрочитанных сообщений
                    if (user != null)
                    {
                        var unreadMessages = db.Messages.Where(x => x.IsReceived == false && x.ToUserId == user.Id).ToList();
                        List<MessageUdp> messageUdps = new List<MessageUdp>();
                        foreach (var message in unreadMessages)
                        {
                            var messageUdp = new MessageUdp()
                            {
                                Command = Command.Message,
                                Id = message.Id,
                                FromName = message.FromUser.Name,
                                ToName = message.ToUser.Name,
                                Text = message.Text
                            };

                            messageUdps.Add(messageUdp);
                        }
                        // отправка листа непрочитанных сообщений
                        foreach (var messageUdp in messageUdps)
                        {
                            var message = messageUdp.ToJson();
                            _messageSource.Send(messageUdp, iPEndPoint);
                           // _udpClient.Send(Encoding.UTF8.GetBytes(message), message.Length, iPEndPoint);
                        }

                        foreach (var message in unreadMessages)
                        {
                            message.IsReceived = true;
                        }
                        db.SaveChanges();
                    }
                }

            }
            else 
            {
                Console.WriteLine("Client is not registered please register first");
            }
        }

       
    }
}
