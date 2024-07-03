using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando la lista de códigos estpa vacía
    /// </summary>
    public class ListaCodigosVaciaExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ListaCodigosVaciaExcepcion(string message) : base(message)
        {
        }
    }
}