using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Aquí se aplica el principio de SRP,ya que la única responsabilidad es la de visualizar
    /// el stock de un determinado producto para todos los depósitos
    /// </summary>
    public class Proveedor
    {
        /// <summary>
        /// Atributos de la clase
        /// </summary>
        private string Nombre;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombre">Nombre del proveedor</param>
        public Proveedor(string nombre)
        {
            this.Nombre = nombre;
        }

        /// <summary>
        /// Método para visualizar el stock en todos los depósitos
        /// </summary>
        /// <param name="codigo">Código del producto</param>
        /// <returns>Diccionario que tiene el stock total de un determinado producto</returns>
        public Dictionary<string, Dictionary<string, int>> VisualizarStock(int codigo)
        {
            // Genero un diccionario vacío que me contenga otro diccionario
            Dictionary<string, Dictionary<string, int>> stockProducto = new Dictionary<string, Dictionary<string, int>>() {};

            // Itero depósito por depósito en el contenedor de depositos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // Itero seccion por seccion dentro del depósito
                foreach (ISeccion seccion in deposito.GetSecciones)
                {
                    // En caso que el diccionario stock ya contenga el deposito
                    if (stockProducto.Keys.Contains(deposito.GetNombre))
                    {
                        stockProducto[deposito.GetNombre].Add(seccion.GetNombre, seccion.CantidadStock(codigo));
                    }

                    // En caso que el diccionario stock no contenga el deposito
                    else
                    {
                        stockProducto.Add(deposito.GetNombre, new Dictionary<string, int> {{seccion.GetNombre, seccion.CantidadStock(codigo)}});
                    }
                }
            }

            // Creo una cadena en donde voy a almacenar el resultado
            StringBuilder cadenaStock = new StringBuilder();

            // Itero para cada uno de los depósitos de la lista de depósitos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                cadenaStock.AppendLine($"Para el depósito {deposito.GetNombre}:");

                // Itero para cada una de las secciones de la lista de secciones
                foreach (ISeccion seccion in deposito.GetSecciones)
                {
                    // Agrego una cadena con la cantidad en stock del mismo
                    cadenaStock.AppendLine($"   Cantidad en stock sección '{seccion.GetNombre}': {seccion.CantidadStock(codigo)}");
                }
            } 
            
            // Imprimo en consola la cadena resultante
            System.Console.WriteLine(cadenaStock);

            // Retorno el diccionario con la información de stock total
            return stockProducto;
        }

        /// <summary>
        /// Método getter para el nombre del proveedor
        /// </summary>
        /// <value>Nombre del proveedor</value>
        public string GetNombre
        {
            get
            {
                return this.Nombre;
            }
        }
    }
}

