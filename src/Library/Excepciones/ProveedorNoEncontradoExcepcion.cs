using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando el proveedor no es encontrado
    /// </summary>
    public class ProveedorNoEncontrado : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ProveedorNoEncontrado(string message) : base(message)
        {
        }
    }
}