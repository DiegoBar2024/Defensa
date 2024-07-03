using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Serialization;

using System.Runtime.CompilerServices;


namespace ProyectoFinal
{
    /// <summary>
    /// Esta clase implementa la interfaz IProducto con el fin de aplicar el principio DIP.
    /// Para más detalles sobre la implementación de esta clase leer el archivo readme
    /// </summary>
    public class Producto : IProducto
    {
        /// <summary>
        /// Atributos de la clase
        /// </summary>

        private string Nombre;
        private double Precio;
        private int Codigo;
        private string Marca;
        private List<string> Categorias = new List<string>();

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombre">Nombre del producto</param>
        /// <param name="precio">Precio del producto</param>
        /// <param name="codigo">Código del producto</param>
        /// <param name="marca">Marca del producto</param>
        /// <param name="categorias">Categoría del producto</param>

        public Producto(string nombre, double precio, int codigo, string marca, List<string> categorias)
        {
            this.Nombre = nombre;
            this.Precio = precio;

            // En caso que el codigo del producto
            this.Codigo = codigo;
            this.Marca = marca;
            this.Categorias = categorias;
        }

        /// <summary>
        /// Método que agrega una categoría
        /// </summary>
        /// <param name="categoria">Nombre de la categoría que se agrega</param>

        public void AgregarCategoria(string categoria)
        {
            // En caso que ya exista la categoría a agregar, que me levante una excepción CategoriaExistenteExcepcion
            if (this.ExisteCategoria(categoria))
            {
                throw new CategoriaExistenteExcepcion($"La categoría '{categoria}' ya existe para este producto.");
            }

            this.Categorias.Add(categoria);
        }

        /// <summary>
        /// Método que verifica si existe la categoría
        /// </summary>
        /// <param name="categoria">Categoría a verificar</param>
        /// <returns>true or false</returns>

        public bool ExisteCategoria(string categoria)
        {
            return this.Categorias.Contains(categoria);
        }

        /// <summary>
        /// Método getter para el código del producto
        /// </summary>
        /// <value>El código del producto</value>

        public int GetCodigo
        {
            get
            {
                return this.Codigo;
            }
        }

        /// <summary>
        /// Método getter para obtener las categorías de un producto
        /// </summary>
        /// <value>La categoría del producto</value>

        public IEnumerable GetCategorias
        {
            get
            {
                return this.Categorias;
            }
        }
    }
}
