using System;
using System.Collections.Generic;
using System.Dynamic; 
using System.Xml.Serialization;  

namespace ProyectoFinal
{
    /// <summary>
    /// Clase que se comporta como un contenedor para almacenar una lista de proveedores
    /// Se utiliza el patrón Creator (leer archivo readme para más detalles)
    /// </summary>
    public class ContenedorProveedores
    {
        /// <summary>
        /// Lista estática que contiene los objetos del tipo Proveedor
        /// </summary>
        /// <typeparam name="Proveedor"></typeparam>
        /// <returns></returns>
        private static List<Proveedor> Proveedores = new List<Proveedor>();
        
        /// <summary>
        /// Uso el patrón Creator para crear un proveedor en base a los datos de entrada
        /// </summary>
        /// <param name="nombre">Nombre del proveedor</param>
        /// <returns>El proveedor</returns>
        public static Proveedor AltaProveedor(string nombre)
        {
            /// <summary>
            /// Creo un proveedor en base a los atributos dados de entrada
            /// </summary>
            /// <returns>El proveedor</returns>
            Proveedor proveedor = new Proveedor(nombre);

            // Agrego el proveedor a la lista de proveedores
            ContenedorProveedores.Proveedores.Add(proveedor);

            /// <summary>
            ///  Retorno el proveedor
            /// </summary>
            return proveedor;
        }

        /// <summary>
        /// Método getter para la lista de proveedores
        /// </summary>
        /// <value>Proveedores</value>
        public static IEnumerable<Proveedor> GetProveedores
        {
            get
            {
                return ContenedorProveedores.Proveedores;
            }
        }
    }
}