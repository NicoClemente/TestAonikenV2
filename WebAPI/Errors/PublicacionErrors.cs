using TestAoniken.Models;

namespace TestAoniken.Errors
{
    public static class PublicacionErrors
    {
        public static readonly Error PublicacionNoEncontrada = new("Publicacion.NoEncontrada", "La publicación no fue encontrada.");
        public static readonly Error PublicacionYaAprobada = new("Publicacion.YaAprobada", "La publicación ya está aprobada.");
        public static readonly Error PublicacionInvalida = new("Publicacion.Invalida", "La publicación es inválida.");
        public static readonly Error ErrorActualizando = new("Publicacion.ErrorActualizando", "Error al actualizar la base de datos.");
        public static readonly Error ErrorEnviandoCorreo = new("Publicacion.ErrorEnviandoCorreo", "Error al enviar el correo electrónico.");
        public static readonly Error ErrorDesconocido = new("Publicacion.ErrorDesconocido", "Ocurrió un error inesperado.");
    }
}
