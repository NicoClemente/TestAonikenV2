using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TestAoniken.Models;

namespace TestAoniken.Servicios
{
    public class ServiceBusSender : IServiceBusSender
    {
        private readonly ServiceBusClient _client;
        private readonly Azure.Messaging.ServiceBus.ServiceBusSender _sender;

        public ServiceBusSender(IConfiguration configuration)
        {
            var connectionString = configuration["AzureServiceBus:ConnectionString"];
            var topicName = configuration["AzureServiceBus:TopicName"];
            _client = new ServiceBusClient(connectionString);
            _sender = _client.CreateSender(topicName);
        }

        public async Task SendMessageAsync(Publicacion publicacion)
        {
            var messageEntity = new MessageEntity
            {
                Content = JsonConvert.SerializeObject(publicacion),
                ContentType = "application/json",
                To = "103077", // La suscripción a quien enviar el mensaje
                Proveedor = TipoProveedorEnum.MercadoPagoPoint,
                EventType = TipoOperacionEnum.Notification,
                RolType = TipoOperacionRolEnum.Tienda
            };

            var messageBody = JsonConvert.SerializeObject(messageEntity);
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody))
            {
                ContentType = "application/json",
                Subject = "103077"
            };

            await _sender.SendMessageAsync(message);
        }
    }
}
