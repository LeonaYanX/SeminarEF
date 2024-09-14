using SeminarEF.Abstraction;
using SeminarEF;
using System.Net;

namespace ChatAppTests
{ 
    public class Tests
    {
        IMessageSource _messageSource;
        IPEndPoint _ipEndPoint;
        [SetUp]
        public void Setup()
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            
        }

        [Test]
        public void TestReceiver()
        {
            _messageSource = new MockMessageSource();

            var result = _messageSource.Receive(ref _ipEndPoint);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Text);
            Assert.AreEqual(result.FromName, "User1");
            Assert.That(Command.Register, Is.EqualTo(result.Command));
            Assert.Pass();
        }
        [Test]
        public void RegisterMeTest()
        {
           var name = "User1";
            var messageUdp = new MessageUdp { Command = Command.Register, FromName = name };

           _messageSource= new MockMessageSource();
            _messageSource.Send(messageUdp, _ipEndPoint);
            var result = _messageSource.Receive(ref _ipEndPoint);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Text);
            Assert.That(result.FromName, Is.EqualTo(name));
            Assert.That(result.Command, Is.EqualTo(Command.Register));
            Assert.Pass();
            
        }
        [Test]
        public void ClientSenderTest()
        {
                string name = "User1";

                string message = "Buuuuuuu";

                Assert.IsNotNull(message);
                Assert.IsNotEmpty(message);

                string recipientTo = "muuuuuuuuu";

                 Assert.IsNotNull(recipientTo);
                 Assert.IsNotEmpty(recipientTo);

                var messageUdp = new MessageUdp { Command = Command.Message, FromName = name, Text = message, ToName = recipientTo };

            _messageSource = new MockMessageSource();

            _messageSource.Send(messageUdp, _ipEndPoint);


            

            Assert.IsNotNull(messageUdp);
            Assert.That(messageUdp.Text , Is.EqualTo(message));
            Assert.That(messageUdp.FromName, Is.EqualTo(name));
            Assert.That(messageUdp.ToName, Is.EqualTo(recipientTo));
            Assert.That(messageUdp.Command, Is.EqualTo(Command.Message));
            Assert.Pass();
            
        }
        [Test]
        public void ClientListener()
        {
            RegisterMeTest();

            var ipEP = new IPEndPoint(_ipEndPoint.Address, _ipEndPoint.Port);

           
                var mesUdp = _messageSource.Receive(ref ipEP);

                Assert.IsNotNull(mesUdp);
                Assert.That(mesUdp.Text, Is.EqualTo(null));
                Assert.That(mesUdp.FromName, Is.EqualTo("User1"));
                Assert.That(mesUdp.ToName, Is.EqualTo(null));
                Assert.That(mesUdp.Command, Is.EqualTo(Command.Register));
                Assert.Pass();
            
        }
    }
}