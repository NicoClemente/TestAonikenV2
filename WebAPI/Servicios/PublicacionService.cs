using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestAoniken.Data;
using TestAoniken.Models;
using TestAoniken.Errors;
using TestAoniken.Servicios;

namespace TestAoniken.Servicios
{
    public class PublicacionService : IPublicacionService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IServiceBusSender _serviceBusSender;

        public PublicacionService(AppDbContext context, IEmailService emailService, IServiceBusSender serviceBusSender)
        {
            _context = context;
            _emailService = emailService;
            _serviceBusSender = serviceBusSender;
        }

        public async Task<Result<List<Publicacion>, Error>> ObtenerPublicacionesPendientesAsync()
        {
            try
            {
                var publicaciones = await _context.Publicaciones
                    .AsNoTracking()
                    .Include(p => p.Autor)
                    .Where(p => p.PendienteAprobacion)
                    .ToListAsync();
                return publicaciones;
            }
            catch (Exception)
            {
                return PublicacionErrors.ErrorDesconocido;
            }
        }

        public async Task<Result<bool, Error>> AprobarPublicacionAsync(int idPublicacion)
        {
            try
            {
                if (idPublicacion <= 0)
                    return PublicacionErrors.PublicacionInvalida;

                var publicacion = await _context.Publicaciones
                    .Include(p => p.Autor)
                    .FirstOrDefaultAsync(p => p.Id == idPublicacion);

                if (publicacion == null)
                    return PublicacionErrors.PublicacionNoEncontrada;

                if (!publicacion.PendienteAprobacion)
                    return PublicacionErrors.PublicacionYaAprobada;

                publicacion.PendienteAprobacion = false;
                await _context.SaveChangesAsync();

                await _emailService.SendEmailAsync(publicacion.AutorId, "Publicación aprobada",
                    $"Tu publicación '{publicacion.Titulo}' ha sido aprobada.");

                await _serviceBusSender.SendMessageAsync(publicacion);

                return true;
            }
            catch (DbUpdateException)
            {
                return PublicacionErrors.ErrorActualizando;
            }
            catch (Exception)
            {
                return PublicacionErrors.ErrorDesconocido;
            }
        }

        public async Task<Result<bool, Error>> RechazarPublicacionAsync(int idPublicacion)
        {
            try
            {
                if (idPublicacion <= 0)
                    return PublicacionErrors.PublicacionInvalida;

                var publicacion = await _context.Publicaciones.FindAsync(idPublicacion);
                if (publicacion == null)
                    return PublicacionErrors.PublicacionNoEncontrada;

                _context.Publicaciones.Remove(publicacion);
                await _context.SaveChangesAsync();

                await _emailService.SendEmailAsync(publicacion.AutorId, "Publicación rechazada",
                    $"Tu publicación '{publicacion.Titulo}' fue rechazada.");

                return true;
            }
            catch (DbUpdateException)
            {
                return PublicacionErrors.ErrorActualizando;
            }
            catch (Exception)
            {
                return PublicacionErrors.ErrorDesconocido;
            }
        }

        public async Task<Result<Publicacion, Error>> ActualizarPublicacionAsync(int id, Publicacion publicacionActualizada)
        {
            try
            {
                if (id <= 0 || publicacionActualizada == null ||
                    string.IsNullOrWhiteSpace(publicacionActualizada.Titulo) ||
                    string.IsNullOrWhiteSpace(publicacionActualizada.Contenido))
                    return PublicacionErrors.PublicacionInvalida;

                var publicacion = await _context.Publicaciones.FindAsync(id);
                if (publicacion == null)
                    return PublicacionErrors.PublicacionNoEncontrada;

                publicacion.Titulo = publicacionActualizada.Titulo;
                publicacion.Contenido = publicacionActualizada.Contenido;
                await _context.SaveChangesAsync();

                return publicacion;
            }
            catch (DbUpdateException)
            {
                return PublicacionErrors.ErrorActualizando;
            }
            catch (Exception)
            {
                return PublicacionErrors.ErrorDesconocido;
            }
        }

        public async Task<Result<bool, Error>> EliminarPublicacionAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return PublicacionErrors.PublicacionInvalida;

                var publicacion = await _context.Publicaciones.FindAsync(id);
                if (publicacion == null)
                    return PublicacionErrors.PublicacionNoEncontrada;

                _context.Publicaciones.Remove(publicacion);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException)
            {
                return PublicacionErrors.ErrorActualizando;
            }
            catch (Exception)
            {
                return PublicacionErrors.ErrorDesconocido;
            }
        }
    }
}