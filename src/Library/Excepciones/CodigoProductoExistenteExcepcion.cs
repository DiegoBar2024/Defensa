using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    ///  Excepción que se "activa" cuando se intenta agregar un producto con un código que ya existe
    /// </summary>
    public class CodigoProductoExistenteExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CodigoProductoExistenteExcepcion(string message) : base(message)
        {
        }
    }
}