using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando el pin no es válido
    /// </summary>
    public class PINInvalidoExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public PINInvalidoExcepcion(string message) : base(message)
        {
        }
    }
}