using System;
using System.Collections;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Interfaz IDeposito. La clase Depósito implementa esta interfaz con el fin de aplicar
    /// el principio OCP (leer archivo readme para más detalles).
    /// </summary>
    public interface IDeposito
    {
        /// <summary>
        /// Se obtiene laubicacion del depósito
        /// </summary>
        /// <value>ubicacion del depósito</value>
        string GetUbicacion {get; }

        /// <summary>
        /// Se obtiene nombre del depósito
        /// </summary>
        /// <value>Nombre del depósito</value>
        string GetNombre { get; }

        /// <summary>
        /// Se obtiene la ubicación del depósito
        /// </summary>
        /// <value>Ubicación del deposito</value>
        IEnumerable GetSecciones { get; }
        /// <summary>
        /// Creación de una sección dentro del depósito 
        /// </summary>
        /// <param name="nombre">Nombre de la sección</param>
        /// <param name="capacidad">Capacidad de la sección</param>
        /// <returns>La sección</returns>
        ISeccion CrearSeccion(string nombre, int capacidad);
    }
}