using System;
using System.Collections.Generic;

namespace ProyectoFinal
{

    /// <summary>
    /// Creo una clase que me implemente la comunicación intermedia entre
    /// el bot y el programa bajo el acceso de Proveedor
    /// </summary>
    public class IntermediarioProveedor : IIntermediario
    {
        // Creo un objeto privado proveedor al cual le voy a delegar los metodos
        private static Proveedor proveedor;

        /// <summary>
        /// Delego el metodo VisualizarStock al objeto proveedor
        /// </summary>
        /// <param name="codigo">código del producto</param>
        /// <returns></returns>
        public static string VisualizarStock(string nombreProducto)
        {
            return proveedor.VisualizarStock(nombreProducto);
        }

        /// <summary>
        /// Creo un metodo que me permita configurar el proveedor
        /// </summary>
        /// <param name="parametrosProveedor">lista de parámetros para configurar el proveedor</param>
        public static void ConfigurarProveedor(List<string> parametrosProveedor)
        {
            IntermediarioProveedor.proveedor = new Proveedor(parametrosProveedor[0]);
        }
    }
}