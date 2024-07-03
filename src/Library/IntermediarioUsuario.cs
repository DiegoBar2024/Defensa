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
    /// el bot y el programa bajo el acceso de Usuario
    /// </summary>
    public class IntermediarioUsuario : IIntermediario
    {
        // Creo un objeto privado usuario al que voy a delegar los métodos
        private static Usuario usuario;
        /// <summary>
        /// Delego el método AltaProducto a Usuario
        /// </summary>
        /// <param name="nombre">nombre del producto</param>
        /// <param name="precio">precio del producto</param>
        /// <param name="codigo">código del producto</param>
        /// <param name="marca">marca del producto</param>
        /// <param name="categorias">categorías a la cual pertenece el producto</param>
        /// <param name="nombreSeccion">nombre de la sección en la cual se encuentra el producto</param>
        /// <param name="nombreDeposito">nombre del depósito en el cual se encuentra el producto</param>
        /// <param name="stock">cantidad de stock del producto</param>
        public static void AltaProducto(string nombre, double precio, int codigo, string marca, List<string> categorias, string nombreSeccion, string nombreDeposito, int stock)
        {
            usuario.AltaProducto(nombre, precio, codigo, marca, categorias, nombreSeccion, nombreDeposito, stock);
        }
        /// <summary>
        /// Delego el metodo DepositoMasCercano a Usuario
        /// </summary>
        /// <param name="codigoProducto">código del producto</param>
        /// <param name="ubicacionUsuario">ubicación del usuario</param>
        /// <returns></returns>
        public static string DepositoMasCercano(int codigoProducto, string ubicacionUsuario)
        {
            return usuario.DepositoMasCercano(codigoProducto, ubicacionUsuario);
        }

        /// <summary>
        /// Delego el método RealizarVenta a Usuario
        /// </summary>
        /// <param name="ventasIndividuales">representa el resultado de una venta</param>
        /// <returns></returns>
        public static string RealizarVenta(List<List<string>> ventasIndividuales)
        {
            return usuario.RealizarVenta(ventasIndividuales);
        }
        /// <summary>
        /// Configuración del usuario
        /// </summary>
        /// <param name="parametrosUsuario">parámetros necesarios para configurar al usuario</param>
        public static void ConfigurarUsuario(List<string> parametrosUsuario)
        {
            IntermediarioUsuario.usuario = new Usuario(parametrosUsuario[0], parametrosUsuario[2].Split(", ").ToList());
        }
    }
}