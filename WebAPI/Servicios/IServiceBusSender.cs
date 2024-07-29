namespace TestAoniken.Servicios
{
    using System.Threading.Tasks;
    using TestAoniken.Models;

    public interface IServiceBusSender
    {
        Task SendMessageAsync(Publicacion publicacion);
    }
}
