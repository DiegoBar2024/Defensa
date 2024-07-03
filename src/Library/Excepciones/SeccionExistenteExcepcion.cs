using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando la sección a crear ya existe
    /// </summary>
    public class SeccionExistenteExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public SeccionExistenteExcepcion(string message) : base(message)
        {
        }
    }
}