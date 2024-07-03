using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Representa la venta individual de un producto. Delegará la responsabilidad de 
    /// modificar el stock a la clase Seccion. (para más detalles leer el archivo readme)
    /// </summary>

    public class VentaIndividual
    {
        /// <summary>
        /// Quiero que la clase conozca el Producto, la cantidad y la sección donde sacó el producto
        /// </summary>
        
        /// <summary>
        /// Producto que se quiere vender
        /// </summary>
        private int CodigoProducto;
        
        /// <summary>
        /// Cantidad del producto que se va a vender
        /// </summary>
        private int Cantidad;

        /// <summary>
        /// Seccion de donde se vende el producto
        /// </summary>
        private ISeccion Seccion;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigoProducto">Código del producto</param>
        /// <param name="cantidad">Cantidad del producto que se va a vender</param>
        /// <param name="nombreSeccion">Nombre de la seccion del producto que se vende</param>
        /// <param name="nombreDeposito">Nombre del deposito del producto que se vende</param>

        public VentaIndividual(int codigoProducto, int cantidad, string nombreSeccion, string nombreDeposito)
        {
            // Instancio un buscador de secciones
            IBuscador<ISeccion> buscadorSecciones = new BuscadorSecciones(nombreDeposito, nombreSeccion);

            // Hago la búsqueda de la seccion en base al depósito y en base a la seccion
            ISeccion seccion = buscadorSecciones.Buscar();

            this.CodigoProducto = codigoProducto;

            this.Seccion = seccion;

            this.Cantidad = cantidad;
        }

        /// <summary>
        /// Creo un método que me disminuye el stock de la venta individual
        /// </summary>

        public void DisminuirStockIndividual()
        {
            this.Seccion.ModificarStock(this.CodigoProducto, - this.Cantidad);
        }

        /// <summary>
        /// Creo un método getter que me retorne la sección asociada a la venta individual
        /// </summary>
        /// <value>Seccion asociada a la venta</value>
        public ISeccion GetSeccion
        {
            get
            {
                return this.Seccion;
            }
        }
        
        /// <summary>
        /// Creo un método getter que retorne la cantidad en stock que vendí
        /// </summary>
        /// <value>Cantidad de stock vendido</value>
        public int GetCantidad
        {
            get
            {
                return this.Cantidad;
            }
        }
    }
}