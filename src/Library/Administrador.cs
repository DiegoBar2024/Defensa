using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml.Serialization;

namespace ProyectoFinal
{
        /// <summary>
        /// Clase que representa un Administrador y hereda de la clase Usuario.
        /// También en esta clase se utiliza el patrón Creator y Expert.
        /// Como principios se utilizó SRP.
        /// En el readme se puede encontrar la explicación más detallada del por qué se
        /// usaron estos patrones y principios
        /// </summary>
        public class Administrador : Usuario
        {
                /// <summary>
                /// Constructor de la clase
                /// </summary>
                /// <param name="nombre">Nombre del administrador</param>
                /// <returns></returns>
                public Administrador(string nombre) : base(nombre)
                {
                }

                /// <summary>
                /// Método para la creación de un depósito
                /// </summary>
                /// <param name="nombre">Nombre del depósito</param>
                /// <param name="ubicacion">Ubicación del depósito</param>
                /// <param name="capacidad">Capacidad del depósito</param>
                /// <param name="distancia">Distancia depósitos</param>
                public void CrearDeposito(string nombre, string ubicacion, int capacidad)
                {
                        // CREATOR --> Paso los parámetros del depósito a crear para que se encargue DepositContainer
                        ContenedorDepositos.AgregarDeposito(nombre, ubicacion, capacidad);
                }

                /// <summary>
                /// Método para crear la sección dentro del depósito
                /// </summary>
                /// <param name="nombre">Nombre de la sección</param>
                /// <param name="capacidad">Capacidad de la sección</param>
                /// <param name="nombreDeposito">Nombre del depósito en que se creará la sección</param>
                public void CrearSeccion(string nombre, int capacidad, string nombreDeposito)
                {
                        // Creo un buscador de depósitos pasando como parámetro el nombre del depósito
                        IBuscador<IDeposito> buscadorDepositos = BuscadorDepositos.GetBuscadorDepositos(nombreDeposito);

                        // Busco el depósito usando el método Buscar() del buscador
                        IDeposito deposito = buscadorDepositos.Buscar();

                        // En caso que el depósito ingresado no exista, que me levante una excepción del tipo DepositoNoEncontradoExcepcion
                        if (deposito == null)
                        {
                                throw new DepositoNoEncontradoExcepcion($"El depósito <<{nombreDeposito}>> no fue encontrado");
                        }

                        // CREATOR --> Paso los parámetros de la sección a cerar para que se encargue de crearlo el depósito mismo
                        deposito.CrearSeccion(nombre, capacidad);
                }

                /// <summary>
                /// Método para dar de alta a un Usuario
                /// </summary>
                /// <param name="nombre">Nombre del usuario</param>
                /// <param name="permiso">Permiso del usuario (administrador o usuario)</param>
                public void AltaUsuario(string nombre, string permiso, List<string> nombresDepositos)
                {
                        // CREATOR --> Delego la reponsabilidad de dar de alta un usuario a la clase UserContainer
                        ContenedorUsuarios.AltaUsuario(nombre, permiso, nombresDepositos);
                }

                public void BajaUsuario(string nombre)
                {
                        ContenedorUsuarios.BajaUsuario(nombre);
                }

                /// <summary>
                /// Método para dar de alta a un proveedor
                /// </summary>
                /// <param name="nombre">Nombre del proveedor</param>
                public void AltaProveedor(string nombre)
                {
                        // CREATOR --> Delego la responsabilidad de dar de alta un proveedor a la clase SupplierContainer
                        ContenedorProveedores.AltaProveedor(nombre);
                }

                /// <summary>
                /// Método para crear una categoría 
                /// </summary>
                /// <param name="categoria">Nombre de la categoría</param>
                /// <param name="codigoProductos">Lista de códigos de productos de una categoría</param>

                public void CrearCategoria(string categoria, List<int> codigoProductos)
                {
                        // Prueba la responsabilidad de crear la categoría de un producto a otra clase
                        CreadorCategorias.CrearCategoria(categoria, codigoProductos);
                }

                /// <summary>
                /// Método para aumentar stock de un producto en un determinado depósito 
                /// </summary>
                /// <param name="nombreDeposito">Nombre del depósito en el que quiero aumentar el stock del producto</param>
                /// <param name="stockComprado">Cantidad de stock comprado</param>
                /// <param name="codigoProducto">Código del producto</param>

                public void AumentarStock(string nombreDeposito, int stockComprado, int codigoProducto)
                {
                        // Instancio un buscador de depositos
                        IBuscador<IDeposito> buscadorDepositos = BuscadorDepositos.GetBuscadorDepositos(nombreDeposito);

                        // Busco el depósito con el nombre pasado como parámetro
                        IDeposito deposito = buscadorDepositos.Buscar();

                        // En caso que el depósito ingresado no exista, que me levante una excepción del tipo DepositoNoEncontradoExcepcion
                        if (deposito.Equals(null))
                        {
                                throw new DepositoNoEncontradoExcepcion($"El depósito {nombreDeposito} no fue encontrado");
                        }

                        // Itero en cada una de las secciones del depósito
                        foreach (ISeccion seccion in deposito.GetSecciones)
                        {
                                // En caso que ya no tenga más stock para asignar, termino el bucle
                                if (stockComprado == 0)
                                {
                                        break;
                                }

                                if (seccion.GetCapacidad > seccion.CantidadProductos)
                                {
                                        if (stockComprado > seccion.GetCapacidad - seccion.CantidadProductos)
                                        {
                                                seccion.ModificarStock(codigoProducto, seccion.GetCapacidad - seccion.CantidadProductos);
                                                stockComprado -= seccion.GetCapacidad - seccion.CantidadProductos;
                                        }

                                        else if (stockComprado <= seccion.GetCapacidad - seccion.CantidadProductos)
                                        {
                                                seccion.ModificarStock(codigoProducto, stockComprado);
                                                stockComprado = 0;
                                        }
                                }

                                else
                                {
                                        continue;
                                }
                        }

                        // En caso de que no tenga lugar en mi depósito para llenarlo con todo el stock comprado,
                        // que me levante una excepción
                        if (stockComprado > 0)
                        {
                                throw new CapacidadInsuficienteExcepcion($"No hay suficiente capacidad en el depósito '{nombreDeposito}' para aumentar en {stockComprado} unidades el producto con código {codigoProducto}.");
                        }
                }

                /// <summary>
                /// Método para adquirir las ventas realizadas en un día dado
                /// </summary>
                /// <param name="fecha">Fecha de las ventas realizadas</param>
                /// <returns></returns>

                public List<VentaTotal> VentasEnDia(DateTime fecha)
                {
                        // EXPERT --> Delego la responsabilidad de buscar las ventas por día a la clase ContenedorVentasPorFecha
                        return ContenedorVentasPorFecha.VentasEnDia(fecha);
                }
                /// <summary>
                /// Método que obtiene los nombres de los depósitos
                /// </summary>
                /// <returns>Cadena que contiene los nombres de los depósitos</returns>
                public string NombresDepositos()
                {
                        // EXPERT --> Delego la responsabilidad de obtener los nombres de todos los depositos a la clase ContenedorDepositos
                        return ContenedorDepositos.NombresDepositos();
                }

                public void EliminarCodigoProducto(int codigoProducto)
                {
                        // Itero deposito por deposito en la lista de depositos existentes
                        foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
                        {
                                // Itero sección por sección en la lista de secciones existentes
                                foreach (ISeccion seccion in deposito.GetSecciones)
                                {
                                        // Le digo a la sección que de de baja el producto
                                        seccion.EliminarCodigo(codigoProducto);
                                }
                        }
                }
        }
}