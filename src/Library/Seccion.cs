using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace ProyectoFinal
{

    /// <summary>
    /// Clase que implementa la interfaz ISeccion. Se utiliza el principio DIP.
    /// También se utiliza el patrón Creator (para más detalles leer el archivo readme)
    /// </summary>

    public class Seccion : ISeccion
    {
        /// <summary>
        /// Atributos de la clase
        /// </summary>

        private string Nombre;
        private int Capacidad;
        private Dictionary<IProducto, int> ListaProductos = new Dictionary<IProducto, int>() {};

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombre">Nombre de la sección</param>
        /// <param name="capacidad">Capacidad de la sección</param>

        public Seccion(string nombre, int capacidad)
        {
            this.Nombre = nombre;
            this.Capacidad = capacidad;
        }

        /// <summary>
        /// Usando el patrón Creator hago que la clase seccion me de de alta un producto
        /// </summary>
        /// <param name="nombre">Nombre del producto</param>
        /// <param name="precio">Precio del producto</param>
        /// <param name="codigo">Código del producto</param>
        /// <param name="marca">Marca del producto</param>
        /// <param name="categorias">Categoría del producto</param>
        /// <param name="stock">Stock del producto</param>

        public void AltaProducto(string nombre, double precio, int codigo, string marca, List<string> categorias, int stock)
        {
            // Creo un producto usando los parámetros de entrada
            IProducto producto = new Producto(nombre, precio, codigo, marca, categorias);

            // Si ya tengo ese producto en la seccion, le sumo el stock que le estoy pasando
            if (this.ListaProductos.ContainsKey(producto))
            {
                this.ModificarStock(codigo, stock);
            }

            // En caso que no exista el producto en la seccion, lo agrego con su stock
            else
            {
                this.ListaProductos.Add(producto, stock);
            }
        }

        /// <summary>
        /// Propiedad que me diga si la sección contiene un determinado producto o no
        /// </summary>
        /// <value>Lista de productos</value>

        public IEnumerable<IProducto> Productos
        {
            get
            {
                return this.ListaProductos.Keys;
            }
        }

        /// <summary>
        ///  Método que me retorne la cantidad de stock asociado a un producto
        /// </summary>
        /// <param name="codigoProducto">Código del producto</param>
        /// <returns>Cantidad de stock o 0</returns>

        public int CantidadStock(int codigoProducto)
        {
            // En caso que se haya dado de alta el producto alguna vez en la sección, que me devuelva la cantidad correspondiente
            foreach (IProducto producto in this.ListaProductos.Keys)
            {
                // Si hay un producto cuyo codigo coincide con el ingresado, que pare
                if (producto.GetCodigo.Equals(codigoProducto))
                {
                    return this.ListaProductos[producto];
                }
            }

            // Si no existe ningún producto, que retorne 0
            return 0;
        }
        
        public int CantidadStock(string nombreProducto)
        {
            // En caso que se haya dado de alta el producto alguna vez en la sección, que me devuelva la cantidad correspondiente
            foreach (IProducto producto in this.ListaProductos.Keys)
            {
                // Si hay un producto cuyo codigo coincide con el ingresado, que pare
                if (producto.GetNombre.Equals(nombreProducto))
                {
                    return this.ListaProductos[producto];
                }
            }

            // Si no existe ningún producto, que retorne 0
            return 0;
        }

        /// <summary>
        /// Creo un método que me permita modificar el stock de un producto ya existente
        /// Obs. Si el valor del stock que se pasa como parámetro es negativo, estoy disminuyendo el stock
        /// </summary>
        /// <param name="codigoProducto">Código del producto</param>
        /// <param name="stock">Nuevo stock del producto</param>

        public void ModificarStock(int codigoProducto, int stock)
        {
            // Creo una variable booleana que me diga si el codigo del producto que ingresé existe en la seccion
            bool existeProducto = false;

            // Itero producto por producto en la lista de productos de la seccion
            foreach (IProducto producto in this.Productos)
            {
                // En caso que el producto tenga el codigo indicado, modifico el stock y termino ejecución
                if (producto.GetCodigo.Equals(codigoProducto))
                {
                    // Seteo la bandera booleana existeProducto a true
                    existeProducto = true;

                    // En caso que la cantidad en stock ingresada desborde la capacidad de la sección, que levante una excepción
                    if (this.CantidadProductos + stock > this.Capacidad)
                    {
                        throw new CapacidadInsuficienteExcepcion("Se supera la cantidad máxima de productos que se pueden almacenar en la sección");
                    }

                    // En caso que la cantidad en stock que estoy pidiendo sea mayor a la que tengo, que levante una excepcion
                    if (this.CantidadStock(codigoProducto) + stock < 0)
                    {
                        throw new StockInsuficienteExcepcion($"Para el còdigo de producto {codigoProducto} tiene únicamente {this.CantidadStock(codigoProducto)} disponibles en stock.");
                    }

                    // Modifico el stock del producto
                    this.ListaProductos[producto] += stock;
                    break;
                }
            }

            // En caso que el producto no exista en la sección, que me levante una excepción
            if (!existeProducto)
            {
                throw new Exception($"El producto de código {codigoProducto} no existe en ésta seccion");
            }
        }

        /// <summary>
        /// Creo un metodo getter para el nombre de la seccion
        /// </summary>
        /// <value>Nombre de la sección</value>

        public string GetNombre
        {
            get
            {
                return this.Nombre;
            }
        }

        /// <summary>
        /// Creo un método getter para la capacidad de la seccion
        /// </summary>
        /// <value>Capacidad de la sección</value>

        public int GetCapacidad
        {
            get
            {
                return this.Capacidad;
            }
        }

        /// <summary>
        ///  Creo un método getter que me de la cantidad de productos totales que tiene la seccion
        /// </summary>
        /// <value>Cantidad de productos en la sección</value>

        public int CantidadProductos
        {
            get
            {
                // Inicializo una variable local en donde voy a almacenar la cantidad de productos
                int cantidadProductos = 0;

                // Itero producto por producto que tengo guardado en la seccion
                foreach (IProducto producto in this.ListaProductos.Keys)
                {
                    // Actualizo la variable donde almaceno la cantidad total de productos
                    cantidadProductos += this.ListaProductos[producto];
                }

                // Retorno la variable
                return cantidadProductos;
            }
        }
    }
}