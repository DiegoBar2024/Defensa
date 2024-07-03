using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace ProyectoFinal
{
    /// <summary>
    /// Clase que representa la venta total (contiene la lista de ventas individuales).
    ///Se usa el patrón Creator. Creator viene de Usuario cuando hace una venta.
    /// También se usa el patrón Expert (leer archivo readme para más detalles).
    /// </summary>

    public class VentaTotal
    {
        /// <summary>
        /// Atributos de la clase
        /// </summary>

        private DateTime Fecha;
        private List<VentaIndividual> VentasIndividuales = new List<VentaIndividual>();
        private int ID_Venta;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="fecha">Fecha de la venta total</param>

        public VentaTotal(DateTime fecha)
        {
            // En caso que la fecha ingresada sea futura, que me levante una expeción FechaVentaTotalInvalidaExcepcion
            if (fecha > DateTime.Now)
            {
                throw new FechaInvalidaExcepcion("La fecha de la venta total no puede ser en el futuro.");
            }

            this.Fecha = fecha;
        }

        /// <summary>
        /// Método que me permita agregar ventas individuales a la venta total
        /// </summary>
        /// <param name="codigoProducto">Código del producto</param>
        /// <param name="cantidad">Cantidad del producto</param>
        /// <param name="nombreSeccion">Nombre de la sección</param>
        /// <param name="nombreDeposito">Nombre del depósito</param>
        /// <returns></returns>
        public VentaIndividual AgregarVenta(int codigoProducto, int cantidad, string nombreSeccion, string nombreDeposito)
        {
            // Creo una venta individual usando los parámetros que se me pasaron como entrada
            VentaIndividual ventaIndividual = new VentaIndividual(codigoProducto, cantidad, nombreSeccion, nombreDeposito);

            // Agrego la venta individual a la lista de ventas individuales
            this.VentasIndividuales.Add(ventaIndividual);

            // Retorno la venta individual
            return ventaIndividual;
        }

        /// <summary>
        /// Método que me permita agregar ventas individuales a la venta total
        /// </summary>
        /// <param name="ventaIndividual">Venta individual a eliminar</param>

        public void RemoveVenta(VentaIndividual ventaIndividual)
        {
            // En caso que las venta individual ingresada no esté en la lista de ventas individuales
            // que me levante una excepción del tipo VentaIndividualNoEncontradaExcepcion
            if (!this.VentasIndividuales.Contains(ventaIndividual))
            {
                throw new VentaIndividualNoEncontradaExcepcion("La venta individual no se encuentra en la venta total.");
            }

            this.VentasIndividuales.Remove(ventaIndividual);
        }

        /// <summary>
        /// Creo un método que me modifique el stock luego de la venta
        /// </summary>

        public void DisminuirStockTotal(StringBuilder cadenaAdvertencia)
        {
            // Itero para cada una de las ventas individuales
            foreach (VentaIndividual ventaIndividual in this.VentasIndividuales)
            {
                // Coloco dentro del bloque try el llamado a la disminución de stock total
                try
                {
                    // Modifico el stock de cada venta individual
                    ventaIndividual.DisminuirStockIndividual();
                }

                // En otro caso agrego la excepción al mensaje
                catch (Exception e)
                {
                    cadenaAdvertencia.AppendLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Método getter para la fecha de la venta (atributo privado)
        /// </summary>
        /// <value>Fecha de la venta</value>

        public DateTime GetFecha
        {
            get
            {
                return this.Fecha;
            }
        }
        
        /// <summary>
        /// Creo un método getter para acceder a la lista de Ventas Individuales
        /// </summary>
        /// <value>Lista de ventas individuales</value>
        public IEnumerable<VentaIndividual> GetVentasIndividuales
        {
            get
            {
                return this.VentasIndividuales;
            }
        }
    }
}
