using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando se proporciona una categoría que no es válida
    /// </summary>
    public class CategoriaInvalidaExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CategoriaInvalidaExcepcion(string message) : base(message)
        {
        }
    }
}