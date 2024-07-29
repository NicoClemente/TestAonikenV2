using System.Collections.Generic;
using System.Threading.Tasks;
using TestAoniken.Models;

namespace TestAoniken.Servicios
{
    public interface IPublicacionService
    {
        Task<Result<List<Publicacion>, Error>> ObtenerPublicacionesPendientesAsync();
        Task<Result<bool, Error>> AprobarPublicacionAsync(int idPublicacion);
        Task<Result<bool, Error>> RechazarPublicacionAsync(int idPublicacion);
        Task<Result<Publicacion, Error>> ActualizarPublicacionAsync(int id, Publicacion publicacionActualizada);
        Task<Result<bool, Error>> EliminarPublicacionAsync(int id);
    }
}