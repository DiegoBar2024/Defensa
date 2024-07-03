using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Interfaz IProducto. La clase Producto implementa esta interfaz con el fin de usar el 
    /// principio OCP (para más detalles leer el archivo readme).
    /// </summary>
    public interface IProducto
    {
        /// <summary>
        /// Obtiene el código del producto
        /// </summary>
        /// <value>El código del producto</value>
        int GetCodigo { get; }

        string GetNombre { get; }
        
        /// <summary>
        /// Se obtienen las categorías de los productos
        /// </summary>
        /// <value>Categoría producto</value>
        IEnumerable GetCategorias { get; }

        /// <summary>
        /// Se agrega la categoría al producto
        /// </summary>
        /// <param name="categoria">Categoría</param>
        void AgregarCategoria(string categoria);

        /// <summary>
        /// Se verifica la existencia de la categoría
        /// </summary>
        /// <param name="categoria">Categoría a ser verificada</param>
        /// <returns>true or false</returns>
        bool ExisteCategoria(string categoria);
    }
}