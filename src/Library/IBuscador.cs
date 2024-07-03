using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Se define una interfaz genérica 
    /// Para más detalle leer el archivo readme
    /// </summary>
    /// <typeparam name="T">Tipo de objeto que será buscado</typeparam>
    public interface IBuscador<T>
    {
        /// <summary>
        /// Método para buscar un objeto del tipo T
        /// </summary>
        /// <returns>Objeto que se encuentra de tipo T</returns>
        T Buscar();
    }
}