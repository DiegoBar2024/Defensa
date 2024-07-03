using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Creo una clase cuya responsabilidad sea la de crear categorías de productos
    /// </summary>
    public class CreadorCategorias
    {
        /// <summary>
        /// Método para crear categorías
        /// </summary>
        /// <param name="categoria">Categoría de los productos</param>
        /// <param name="codigosProductos">Lista de códigos de productos</param>
        public static void CrearCategoria(string categoria, List<int> codigosProductos)
        {
            // Creo una variable booleana que me diga si existe al menos un codigo que no existe
            bool alMenosUnoNoExiste = true;

            // Inicializo una cadena vacía en donde voy a guardar el mensaje de advertencia
            StringBuilder cadenaAdvertencia = new StringBuilder();

            // En caso que la categoría ingresada sea vacía, que me levante una excepción del tipo CategoriaInvalidaExcepcion
            if (string.IsNullOrWhiteSpace(categoria))
            {
                throw new CategoriaInvalidaExcepcion("La categoría proporcionada es inválida.");
            }

            // En caso que la lista de codigos de productos sea vacía, que me levante una excepción del tipo ListaCodigosVaciaExcepcion
            if (codigosProductos.Count.Equals(0))
            {
                throw new ListaCodigosVaciaExcepcion("La lista de códigos de productos está vacía");
            }

            // Itero para cada uno de los productos en la lista de productos que pasé como parámetro
            foreach (int codigo in codigosProductos)
            {
                // Creo una variable local booleana que me diga si el codigo ingresado existe o no
                // Lo seteo por defecto como falso
                bool existeProducto = false;

                // Itero deposito por deposito en la lista de depositos
                foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
                {
                    // Itero seccion por seccion dentro de cada deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // Itero producto por producto en la lista de productos de la sección
                        foreach (IProducto producto in seccion.Productos)
                        {
                            // En caso que el producto tenga el codigo ingresado, le agrego la categoría
                            if (producto.GetCodigo.Equals(codigo))
                            {
                                // Como existe el producto yo le digo que el producto existe
                                existeProducto = true;

                                // Delego al método de AgregarCategoria
                                producto.AgregarCategoria(categoria);
                            }
                        }
                    }
                }

                // En caso que el producto ingresado no exista, que me agregue el código al mensaje de advertencia
                if (!existeProducto)
                {
                    // Agrego el codigo del producto que no encuentra
                    cadenaAdvertencia.AppendLine($"El producto de código {codigo} no existe");

                    // Voy actualizando alMenosUnoNoExiste dependiendo si al menos un codigo no está ingresado
                    // Entonces hago una máscara con existeProducto
                    alMenosUnoNoExiste = alMenosUnoNoExiste && existeProducto;
                }
            }

            // En caso que al menos un producto no exista, que arroje una excepción y muestre la cadena de advertencia
            if (!alMenosUnoNoExiste)
            {
                throw new Exception(cadenaAdvertencia.ToString());
            }
        }
    }
}
