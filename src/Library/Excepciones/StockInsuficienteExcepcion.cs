using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción para cuando el stock no es suficiente para realizar alguna operación con el mismo
    /// </summary>
    public class StockInsuficienteExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public StockInsuficienteExcepcion(string message) : base(message)
        {
        }
    }
}