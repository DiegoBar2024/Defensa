using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Excepción que se "activa" cuando el tipo de funcionario no es el correcto
    /// </summary>
    public class TipoFuncionarioNoEncontradoExcepcion : Exception
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public TipoFuncionarioNoEncontradoExcepcion(string message) : base(message)
        {
        }
    }
}