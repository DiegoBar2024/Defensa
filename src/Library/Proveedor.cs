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

        // Atributo que me guarde los depósitos a los que tiene acceso el proveedor
        private List<IDeposito> DepositosProveedor = new List<IDeposito>();

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombre">Nombre del proveedor</param>
        public Proveedor(string nombre, List<string> depositosProveedor)
        {
            this.Nombre = nombre;

            // Itero para cada uno de los nombres de los depositos en la lista de entrada
            foreach (string nombreDeposito in depositosProveedor)
            {
                // Obtengo un buscador
                IBuscador<IDeposito> buscadorDeposito = BuscadorDepositos.GetBuscadorDepositos(nombreDeposito);

                // Le digo al buscador que busque
                IDeposito deposito = buscadorDeposito.Buscar();

                // En caso que el deposito no exista, que levante una excepcion
                if (deposito == null)
                {
                    throw new Exception($"El deposito de nombre {nombreDeposito} no existe");
                }

                // Agrego el deposito a la lista de depositos permitidos
                this.DepositosProveedor.Add(deposito);
            }
        }

        public Proveedor()
        {
        }

        /// <summary>
        /// Método para visualizar el stock en todos los depósitos
        /// </summary>
        /// <param name="codigo">Código del producto</param>
        /// <returns>Diccionario que tiene el stock total de un determinado producto</returns>
        public string VisualizarStock(int codigo)
        {
            // Creo una cadena en donde voy a almacenar el resultado
            StringBuilder cadenaStock = new StringBuilder();

            // Itero para cada uno de los depósitos de la lista de depósitos permitidos
            foreach (IDeposito deposito in this.DepositosProveedor)
            {
                cadenaStock.AppendLine($"Para el depósito '{deposito.GetNombre}':");

                // Itero para cada una de las secciones de la lista de secciones
                foreach (ISeccion seccion in deposito.GetSecciones)
                {
                    // Agrego una cadena con la cantidad en stock del mismo
                    cadenaStock.AppendLine($"   Cantidad en stock sección '{seccion.GetNombre}': {seccion.CantidadStock(codigo)}");
                }
            } 

            // Retorno el diccionario con la información de stock total
            return cadenaStock.ToString();
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

