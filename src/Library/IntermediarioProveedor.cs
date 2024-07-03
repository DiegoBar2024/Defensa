using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProyectoFinal;
using System.Collections;
using System.Linq;
using System.Text;

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
        public static string VisualizarStock(int codigo)
        {
            return proveedor.VisualizarStock(codigo);
        }

        /// <summary>
        /// Creo un metodo que me permita configurar el proveedor
        /// </summary>
        /// <param name="parametrosProveedor">lista de parámetros para configurar el proveedor</param>
        public static void ConfigurarProveedor(List<string> parametrosProveedor)
        {
            IntermediarioProveedor.proveedor = new Proveedor(parametrosProveedor[0], parametrosProveedor[2].Split(", ").ToList());
        }
    }
}