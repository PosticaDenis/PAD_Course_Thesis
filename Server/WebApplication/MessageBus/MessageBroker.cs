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

        public void Subscribe<T>(string exchange, Action<T> onEvent)
        {
            var queueName = _channel.QueueDeclare().QueueName;
            ExchangeDeclare(exchange);
            BindQueue<T>(exchange, queueName);
            
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);

                var obj = JsonConvert.DeserializeObject<T>(message);
                onEvent(obj);
            };

            _channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }

        private void BindQueue<T>(string exchange, string queueName)
        {
            _channel.QueueBind(queue: queueName,
                exchange: exchange,
                routingKey: "");
        }

        public void Publish(string exchange, object message)
        {
            ExchangeDeclare(exchange);

            var serializeObject = JsonConvert.SerializeObject(message);
            _channel.BasicPublish(exchange: exchange,
                routingKey: string.Empty,
                basicProperties: null,
                body: Encoding.ASCII.GetBytes(serializeObject));
        }

        private void ExchangeDeclare(string exchange)
        {
            _channel.ExchangeDeclare(exchange, "fanout");
        }
    }
}