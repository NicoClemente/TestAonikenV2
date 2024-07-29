using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TestAoniken.Data;
using TestAoniken.Models;

namespace TestAoniken.Servicios
{
    // Clase que implementa el servicio de envío de correos electrónicos
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        // Constructor que recibe el contexto de configuración y de la base de datos
        public EmailService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // Método asincrónico para mandar correo electrónico
        public async Task SendEmailAsync(int usuarioId, string subject, string message)
        {
            // Busca el usuario por su ID en la base de datos
            Usuario usuario = await _context.Usuarios
                .AsNoTracking()  // No rastrear cambios para optimizar rendimiento
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            // Si no se encuentra usuario, tira excepción
            if (usuario == null)
            {
                throw new ArgumentException($"No se encontró un usuario con el ID {usuarioId}.", nameof(usuarioId));
            }

            // Si el usuario no tiene correo electrónico válido, tira excepción
            if (string.IsNullOrWhiteSpace(usuario.Email))
            {
                throw new InvalidOperationException($"El usuario con ID {usuarioId} no tiene un correo electrónico válido.");
            }

            // Configura el cliente SMTP
            using (var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"]))
            {
                smtpClient.Port = int.Parse(_configuration["Email:SmtpPort"]);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_configuration["Email:SmtpUser"], _configuration["Email:SmtpPass"]);

                // Configura el mensaje de correo
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_configuration["Email:FromAddress"]);
                    mailMessage.To.Add(usuario.Email);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;

                    try
                    {
                        // Intenta enviar el correo de manera asincrónica
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                    catch (SmtpException smtpEx)
                    {
                        // Maneja errores específicos de SMTP y tira una nueva excepción
                        Console.WriteLine($"Error SMTP: {smtpEx.StatusCode}, {smtpEx.Message}");
                        throw new ApplicationException($"Error SMTP al enviar el correo: {smtpEx.StatusCode}", smtpEx);
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier otro tipo de error y tira una nueva excepción
                        Console.WriteLine($"Error general: {ex.Message}");
                        throw new ApplicationException("Error al enviar el correo electrónico.", ex);
                    }
                }
            }
        }
    }
}
