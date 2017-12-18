using System;
using System.Data.Common;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBus
{
    public class MessageBroker
    {
        private readonly IModel _channel;
        private readonly IConnection _createConnection;
        private readonly ConnectionFactory _connectionFactory;

        public MessageBroker(String connectionString)
        {
            var dbConnectionStringBuilder = new DbConnectionStringBuilder();
            dbConnectionStringBuilder.ConnectionString = connectionString;

            var hostName = dbConnectionStringBuilder["Host"].ToString();
            Helper.WaitForPortOpen(1000, hostName, 5672);
            _connectionFactory =
                new ConnectionFactory()
                {
                    HostName = hostName,
                    UserName = dbConnectionStringBuilder["Username"].ToString(),
                    Password = dbConnectionStringBuilder["Password"].ToString()
                };
            _createConnection = _connectionFactory.CreateConnection();
            _channel = _createConnection.CreateModel();
        }

        public void Subscribe<T>(string queue, Action<T> onEvent)
        {
            Declare(queue);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);

                var obj = JsonConvert.DeserializeObject<T>(message);
                onEvent(obj);
            };

            _channel.BasicConsume(queue: queue,
                autoAck: true,
                consumer: consumer);
        }

        public void Publish(string queue, object message)
        {
            Declare(queue);
            var serializeObject = JsonConvert.SerializeObject(message);
            _channel.BasicPublish(exchange: "",
                routingKey: queue,
                basicProperties: null,
                body: Encoding.ASCII.GetBytes(serializeObject));
        }

        private void Declare(string queue)
        {
            _channel.QueueDeclare(queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
    }
}