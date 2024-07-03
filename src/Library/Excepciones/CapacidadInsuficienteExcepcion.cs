using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando la capacidad no es suficiente
    /// </summary>
    public class CapacidadInsuficienteExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CapacidadInsuficienteExcepcion(string message) : base(message)
        {
        }
    }
}