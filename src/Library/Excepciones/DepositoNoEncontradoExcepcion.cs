using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando un depósito no es encontrado
    /// </summary>
    public class DepositoNoEncontradoExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public DepositoNoEncontradoExcepcion(string message) : base(message)
        {
        }
    }
}