using System;
using System.Collections.Generic;

namespace ProyectoFinal
{
   

        /// <summary>
        /// Creo una clase que me implemente la comunicación intermedia entre
        /// el bot y el programa bajo el acceso de Administrador
        /// </summary>
        public class IntermediarioAdministrador : IIntermediario
        {

                /// <summary>
                /// Creo un objeto privado administrador al que voy a delegar los métodos
                /// </summary>
                /// <returns></returns>
                private static Administrador administrador = new Administrador("Administrador");


                /// <summary>
                ///  Delego el método CrearDeposito al objeto administrador
                /// </summary>
                /// <param name="nombre">nombre del depósito</param>
                /// <param name="ubicacion">ubicación del depósito</param>
                /// <param name="capacidad">capacidad que tiene el depósito</param>
                public static void CrearDeposito(string nombre, string ubicacion, int capacidad)
                {
                        administrador.CrearDeposito(nombre, ubicacion, capacidad);
                }


                /// <summary>
                /// Delego el método CrearSeccion al objeto administrador
                /// </summary>
                /// <param name="nombre">nombre de la sección</param>
                /// <param name="capacidad">capacidad de la sección</param>
                /// <param name="nombreDeposito">nombre del depósito que pertenece la sección</param>
                public static void CrearSeccion(string nombre, int capacidad, string nombreDeposito)
                {
                        administrador.CrearSeccion(nombre, capacidad, nombreDeposito);
                }


                /// <summary>
                /// Delego el metodo AltaUsuario al objeto administrador
                /// </summary>
                /// <param name="nombre">nombre del usuario</param>
                /// <param name="permiso">permiso (adminitrador o usuario solo)</param>
                /// <param name="nombresDepositos">nombre de los depósitos en los cuales el usuario puede hacer modificaciones</param>
                public static void AltaUsuario(string nombre, string permiso, List<string> nombresDepositos)
                {
                        administrador.AltaUsuario(nombre, permiso, nombresDepositos);
                }

                /// <summary>
                /// Delego el metodo AltaProveedor al objeto administrador
                /// </summary>
                /// <param name="nombre">nombre del proveedor</param>
                public static void AltaProveedor(string nombre)
                {
                        administrador.AltaProveedor(nombre);
                }


                /// <summary>
                /// Delego el metodo CrearCategoria al objeto administrador
                /// </summary>
                /// <param name="categoria">nombre de la catogoría</param>
                /// <param name="codigoProductos">códigos de los productos asociados a esa categoría</param>
                public static void CrearCategoria(string categoria, List<int> codigoProductos)
                {
                        administrador.CrearCategoria(categoria, codigoProductos);
                }

                /// <summary>
                /// Delego el metodo AumentarStock al objeto administrador
                /// </summary>
                /// <param name="nombreDeposito">nombre del depósito en el cual aumentaré el stock</param>
                /// <param name="stockComprado">cantidad de stock comprado</param>
                /// <param name="codigoProducto">código del producto el cual compré</param>
                public static void AumentarStock(string nombreDeposito, int stockComprado, int codigoProducto)
                {
                        administrador.AumentarStock(nombreDeposito, stockComprado, codigoProducto);
                }

                /// <summary>
                /// Delego el metodo VentasEnDia al objeto administrador
                /// </summary>
                /// <param name="fecha">fecha de la venta </param>
                /// <returns>cadena correspondiente a la fecha</returns>
                public static List<VentaTotal> VentasEnDia(DateTime fecha)
                {
                        return administrador.VentasEnDia(fecha);
                }


                /// <summary>
                /// Delego el metodo NombresDeposito al objeto administrador
                /// </summary>
                /// <returns>Cadena que contiene los nombres de los depósitos</returns>
                public static string NombresDepositos()
                {
                        return administrador.NombresDepositos();
                }

                // Delego el metodo DarBajaUsuario al objeto administrador
                public static void BajaUsuario(string nombre)
                {
                        administrador.BajaUsuario(nombre);
                }
        }
}