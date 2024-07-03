using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando se intenta crear una categoría que ya existe.
    /// </summary>
    public class CategoriaExistenteExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CategoriaExistenteExcepcion(string message) : base(message)
        {
        }
    }
}