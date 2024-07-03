using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando no es encontrada una venta individual
    /// </summary>
    public class VentaIndividualNoEncontradaExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public VentaIndividualNoEncontradaExcepcion(string message) : base(message)
        {
        }
    }
}