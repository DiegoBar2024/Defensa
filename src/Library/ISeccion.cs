using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Interfaz ISeccion. La clase Seccion implementa esta interfaz, con el fin de utilizar 
    /// el principio DIP (para más detalles leer el readme)
    /// </summary>
    public interface ISeccion
    {
        /// <summary>
        /// Se obtiene el nombre de la sección
        /// </summary>
        /// <value>El nombre de la sección</value>
        string GetNombre { get; }

        /// <summary>
        /// Se obtiene la capacidad de una sección 
        /// </summary>
        /// <value>La capacidad de una sección</value>

        int GetCapacidad { get; }

        /// <summary>
        /// Método para dar de alta un producto 
        /// </summary>
        /// <param name="nombre">Nombre del producto</param>
        /// <param name="precio">Precio del producto</param>
        /// <param name="codigo">Código del producto</param>
        /// <param name="marca">Marca del producto</param>
        /// <param name="categorias">Lista de categorías de un producto</param>
        /// <param name="stock">Stock de un producto</param>
        void AltaProducto(string nombre, double precio, int codigo, string marca, List<string> categorias, int stock);

        /// <summary>
        /// Método para modificar el stock de un producto de la sección
        /// </summary>
        /// <param name="codigoProducto">Código del producto</param>
        /// <param name="stock">Nueva cantidad de stock</param>
        void ModificarStock(int codigoProducto, int stock);

        /// <summary>
        /// Se obtiene la lista de productos de una sección
        /// </summary>
        /// <value>Lista de productos de una sección</value>
        IEnumerable<IProducto> Productos { get; }

        /// <summary>
        /// Se obtiene la cantidad de stock de un producto de una sección 
        /// </summary>
        /// <param name="codigoProducto">Código del producto</param>
        /// <returns>Cantidad de stock de un producto</returns>
        int CantidadStock(int codigoProducto);

        /// <summary>
        /// Cantidad de productos en total
        /// </summary>
        /// <value>Cantidad total de productos</value>
        public int CantidadProductos { get; }
    }
}