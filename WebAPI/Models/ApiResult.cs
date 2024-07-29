#nullable enable
using System;

namespace TestAoniken.Models
{
    // Registro inmutable para representar un error
    public sealed record Error(string Code, string? Message = null);

    // Clase genérica para encapsular el resultado de una operación
    public class Result<TValue, TError>
    {
        // Propiedad para almacenar el valor de éxito
        public TValue? Value { get; }

        // Propiedad para almacenar el error en caso de falla
        public TError? Error { get; }

        // Indica si la operación fue exitosa
        public bool IsSuccess { get; }

        // Constructor privado para crear un resultado exitoso
        private Result(TValue value)
        {
            IsSuccess = true;
            Value = value;
            Error = default;
        }

        // Constructor privado para crear un resultado fallido
        private Result(TError error)
        {
            IsSuccess = false;
            Value = default;
            Error = error;
        }

        // Operador implícito para crear un resultado exitoso
        public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

        // Operador implícito para crear un resultado fallido
        public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);

        // Método para manejar el resultado basado en el estado de éxito o falla
        public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
        {
            return IsSuccess ? success(Value!) : failure(Error!);
        }
    }
}