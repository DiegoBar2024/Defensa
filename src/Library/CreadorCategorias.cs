using System;
using System.Collections.Generic;
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
                // Itero deposito por deposito en la lista de depositos
                foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
                {
                    // Itero seccion por seccion dentro de cada deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // Itero producto por producto en la lista de productos
                        foreach (IProducto producto in seccion.Productos)
                        {
                            // En caso que el producto tenga el codigo ingresado, le agrego la categoría
                            if (producto.GetCodigo.Equals(codigo))
                            {
                                producto.AgregarCategoria(categoria);
                            }
                        }
                    }
                }
            }
        }
    }
}
