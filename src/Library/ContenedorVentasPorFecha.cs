using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Creo una nueva clase que contenga una correspondencia entre una fecha y las ventas realizadas
    /// Por el patrón Expert, se asigna la responsabilidad a ésta clase de realizar la búsqueda
    /// de todas las ventas en un determinado día (leer archivo readme para más detalles)
    /// </summary>
    public class ContenedorVentasPorFecha
    {
        private static Dictionary<DateTime, List<VentaTotal>> VentasPorFecha = new Dictionary<DateTime, List<VentaTotal>>() {};

        /// <summary>
        /// Usando el patrón Expert asigno la responsabilidad a ésta clase de hacer las búsquedas por fecha
        /// </summary>
        /// <param name="fecha">Fecha para las ventas que quiero</param>
        /// <returns>Lista de las ventas para la fecha dada</returns>
        public static List<VentaTotal> VentasEnDia(DateTime fecha)
        {
            /// <summary>
            /// Compruebo si existe ventas para la fecha dada
            /// </summary>
            /// <returns>Lista de ventas para la fecha especificada</returns>
            if (ContenedorVentasPorFecha.VentasPorFecha.ContainsKey(fecha))
            {
                return ContenedorVentasPorFecha.VentasPorFecha[fecha];
            }
            
            // En caso que no exista la fecha buscada en el diccionario VentasPorFecha, que me levante una excepción
            else
            {
                throw new FechaInvalidaExcepcion("No se realizaron ventas en la fecha buscada.");
            }
        }

        /// <summary>
        /// Agrego un método que me permita agregar una venta total según la fecha al contenedor de ventas por fecha
        /// </summary>
        /// <param name="ventaTotal">Venta total</param>
        /// <param name="fechaVenta">Fecha de la venta</param>

        public static void AgregarVentaPorFecha(VentaTotal ventaTotal, DateTime fechaVenta)
        {  
            /// <summary>
            /// En caso que todavía no exista ninguna venta en ésta fecha, que me cree la clave fechaVenta y me agregue la venta
            /// </summary>
            /// <returns></returns>

            if (ContenedorVentasPorFecha.VentasPorFecha.ContainsKey(fechaVenta))
            {
                ContenedorVentasPorFecha.VentasPorFecha[fechaVenta].Add(ventaTotal);
            }

            // En caso que ya exista una venta por fecha, que lo agregue a la lista
            else
            {
                ContenedorVentasPorFecha.VentasPorFecha[fechaVenta] = new List<VentaTotal>() {ventaTotal};
            }
        }

        /// <summary>
        /// Creo un metodo que me permita eliminar las ventas realizadas (para el test)
        /// </summary>
        public static void EliminarVentas()
        {
            ContenedorVentasPorFecha.VentasPorFecha.Clear();
        }
    }
}