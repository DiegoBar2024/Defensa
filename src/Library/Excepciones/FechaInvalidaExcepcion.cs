using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando la fecha no es válida
    /// </summary>
    public class FechaInvalidaExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public FechaInvalidaExcepcion(string message) : base(message)
        {
        }
    }
}