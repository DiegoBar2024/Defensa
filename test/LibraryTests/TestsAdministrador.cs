//-----------------------------------------------------------------------------
// <copyright file="TrainTests.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using NUnit.Framework;
using ProyectoFinal;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tests
{
    /// <summary>
    /// Clase que contiene los casos de prueba (test) para las funcionalidades de Administrador.
    /// </summary>
    [TestFixture]
    public class TestsAdministrador
    {
        private Administrador admin;

        private Usuario usuario;

        /// <summary>
        /// Creo objetos Administrador y Usuario, sobre los cuales se harán los test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            /// <summary>
            /// Creo el objeto admin de la clase Administrador sobre el cual voy a hacer los tests
            /// </summary>
            /// <returns></returns>
            admin = new Administrador("admin");


            /// <summary>
            /// Creo un usuario
            /// </summary>
            /// <returns></returns>
            usuario = new Usuario("usuario");
        }

        /// <summary>
        /// Limpieza de test
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            // Le digo que antes de empezar elimine todos los depósitos
            ContenedorDepositos.EliminarDepositos();
        }

        /// <summary>
        /// Busco un producto en la lista de depósitos por su código
        /// </summary>
        /// <param name="codigoProducto">Código del producto buscado</param>
        /// <returns>El producto encontrado o en caso que no lo encuentre retorna null</returns>
        public static IProducto Buscar(int codigoProducto)
        {
            // Itero depósito por depósito en la lista de depósitos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // Itero seccion por seccion en la lista de secciones
                foreach (ISeccion seccion in deposito.GetSecciones)
                {
                    // Itero producto por productos en la lista de productos
                    foreach (IProducto producto in seccion.Productos)
                    {
                        /// <summary>
                        /// En caso que el codigo del producto coincida con el que pasé, me retorne el producto
                        /// </summary>
                        /// <returns></returns>
                        if (producto.GetCodigo.Equals(codigoProducto))
                        {
                            return producto;
                        }
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Test para comprobar la funcionalidad de dar de alta a un usuario sin permisos de administrador
        /// </summary>

        [Test]
        public void DarAltaUsuarioSinPermisos()
        {
            /// <summary>
            /// Nombre del usuario a crear
            /// </summary>
            string nombreUsuario = "usuario";
            
            // Le digo al administrador 
            admin.AltaUsuario(nombreUsuario, "false");

            /// <summary>
            /// Creo una bandera
            /// </summary>
            bool existeUsuario = false;

            // Itero usuario por usuario en la lista de usuarios
            foreach (Usuario usuario in ContenedorUsuarios.GetUsuarios)
            {
                // Si existe un usuario que tenga el nombre colocado, que termine de ejecutar
                if (string.Equals(nombreUsuario, usuario.GetNombre, System.StringComparison.OrdinalIgnoreCase)
                    && !usuario.GetType().Equals(typeof(Administrador)))
                {
                    /// <summary>
                    /// Seteo la bandera a true y termino ejecución
                    /// </summary>
                    existeUsuario = true;
                    break;
                }
            }

            // Compruebo que el usuario efectivamente haya sido creado
            Assert.AreEqual(existeUsuario, true);
        }

        /// <summary>
        /// Test para comprobar la funcionalidad de dar de alta un usuario con permisos
        /// </summary>

        [Test]
        public void DarAltaUsuarioConPermisos()
        {
            /// <summary>
            /// Nombre del usuario a crear
            /// </summary>
            string nombreUsuario = "usuario";
            
            // Le digo al administrador 
            admin.AltaUsuario(nombreUsuario, "true");

            /// <summary>
            /// Creo una bandera
            /// </summary>
            bool existeUsuario = false;

            // Itero usuario por usuario en la lista de usuarios
            foreach (Usuario usuario in ContenedorUsuarios.GetUsuarios)
            {
                // Si existe un usuario que tenga el nombre colocado, que termine de ejecutar
                if (string.Equals(nombreUsuario, usuario.GetNombre, System.StringComparison.OrdinalIgnoreCase)
                    && usuario.GetType().Equals(typeof(Administrador)))
                {
                    /// <summary>
                    /// Seteo la bandera a true y termino ejecución
                    /// </summary>
                    existeUsuario = true;
                    break;
                }
            }

            // Compruebo que el usuario efectivamente haya sido creado
            Assert.AreEqual(existeUsuario, true);
        }

        /// <summary>
        /// Test para verificar la función dar de alta a un proveedor
        /// </summary>

        [Test]
        public void DarAltaProveedor()
        {
            /// <summary>
            /// Nombre del proveedor a crear
            /// </summary>
            string nombreProveedor = "proveedor";

            // El administrador da de alta a un usuario
            admin.AltaProveedor(nombreProveedor);

            /// <summary>
            /// Creo una bandera
            /// </summary>
            bool existeProveedor = false;

            // Itero usuario por usuario en la lista de usuarios
            foreach (Proveedor proveedor in ContenedorProveedores.GetProveedores)
            {
                // Si existe un usuario que tenga el nombre colocado, que termine de ejecutar
                if (string.Equals(nombreProveedor, proveedor.GetNombre, System.StringComparison.OrdinalIgnoreCase))
                {
                    /// <summary>
                    /// Seteo la bandera a true y termino ejecución
                    /// </summary>
                    existeProveedor = true;
                    break;
                }
            }

            // Compruebo que el usuario efectivamente haya sido creado
            Assert.AreEqual(existeProveedor, true);
        }

        /// <summary>
        /// Test para verificar la creación de depósitos 
        /// </summary>

        [Test]
        public void CrearDepositos()
        {
            /// <summary>
            /// Nombre del deposito
            /// </summary>
            string nombreDeposito = "deposito";

            // Creación del deposito
            admin.CrearDeposito(nombreDeposito, "ubicacion", 100, 100);

            /// <summary>
            /// Creo una bandera
            /// </summary>
            bool existeDeposito = false;

            // Itero deposito por deposito en la lista de depositos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // En caso que el nombre del depósito esté en el contenedor de depósitos, seteo a true
                if (string.Equals(deposito.GetNombre, nombreDeposito, System.StringComparison.OrdinalIgnoreCase))
                {
                    /// <summary>
                    /// Termino ejecución del bucle
                    /// </summary>
                    existeDeposito = true;
                    break;
                }
            }

            // Compruebo que esté creado
            Assert.AreEqual(existeDeposito, true);
        }

        /// <summary>
        /// Test para verificar la creación de secciones
        /// </summary>
        [Test]
        public void CrearSecciones()
        {
            // Nombre de la sección
            string nombreSeccion = "seccion";
            
            // Nombre del depósito
            string nombreDeposito = "deposito";

            // El administrador crea un depósito con dicho nombre
            admin.CrearDeposito(nombreDeposito, "ubicacion", 100, 100);

            // El administrador crea una seccion dentro de dicho depósito
            admin.CrearSeccion(nombreSeccion, 100, nombreDeposito);

            // Seteo bandera
            bool existeSeccion = false;

            // Itero deposito por deposito en la lista de depositos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // Si el depósito tiene el nombre que cree, que se pare ahí
                if (string.Equals(deposito.GetNombre, nombreDeposito, System.StringComparison.OrdinalIgnoreCase))
                {
                    // Itero seccion por sección. En caso que haya una seccion con ese nombre, que pare
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        existeSeccion = true;
                        break;
                    }
                }
            }

            // Compruebo que la sección exista
            Assert.AreEqual(true, existeSeccion);

        }

        /// <summary>
        /// Test para verificar la función de crear categoría de un solo producto
        /// </summary>

        [Test]
        public void CrearCategoriasUnProducto()
        {
            // Codigo de producto 1
            int codigoUno = 1201;

            // Codigo de producto 2
            int codigoDos = 3821;

            // Creo un depósito
            admin.CrearDeposito("deposito", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("seccion", 100, "deposito");

            // El usuario da de alta un producto
            usuario.AltaProducto("producto1", 100, codigoUno, "marca1", new List<string>(){"categoria1"}, "seccion", "deposito", 100);
            
            // El usuario da de alta otro producto
            usuario.AltaProducto("producto2", 100, codigoDos, "marca2", new List<string>(){"categoria2"}, "seccion", "deposito", 100);

            // Lista de los codigos de los productos a los que quiero cambiar la categoría
            List<int> listaCodigos = new List<int>{codigoUno, codigoDos};

            // El administrador crea una nueva categoria para los dos productos
            admin.CrearCategoria("nuevaCategoria", listaCodigos);

            // Inicializo la bandera
            bool existenCategorias = true;

            // Itero para cada uno de los codigos en la lista de codigos
            foreach (int codigo in listaCodigos)
            {
                // Encuentro el producto correspondiente para dicho codigo
                IProducto producto = Buscar(codigo);

                // En caso que el producto no contenga la categoría creada, seteo la bandera a false y termino ejecucion
                if (!producto.ExisteCategoria("nuevaCategoria"))
                {
                    existenCategorias = false;
                    break;
                }
            }

            // Compruebo que las categorias se hayan creado
            Assert.AreEqual(existenCategorias, true);
        }

        /// <summary>
        /// Test para verificar la función de aumentar stock de un producto
        /// </summary>

        [Test]
        public void CrearCategoriasDosProductos()
        {
            // Codigo de producto 1
            int codigoUno = 1400;

            // Creo un depósito
            admin.CrearDeposito("deposito", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("seccion", 100, "deposito");

            // Creo otra seccion
            admin.CrearSeccion("seccion1", 100, "deposito");

            // El usuario da de alta un producto
            usuario.AltaProducto("producto1", 100, codigoUno, "marca1", new List<string>(){"categoria1"}, "seccion", "deposito", 50);
            
            // El usuario da de alta otro producto
            usuario.AltaProducto("producto1", 100, codigoUno, "marca1", new List<string>(){"categoria1"}, "seccion1", "deposito", 40);

            // Lista de los codigos de los productos a los que quiero cambiar la categoría
            List<int> listaCodigos = new List<int>{codigoUno};

            // El administrador crea una nueva categoria para los dos productos
            admin.CrearCategoria("nuevaCategoria", listaCodigos);

            // Inicializo la bandera
            bool existenCategorias = true;

            // Compruebo que agregué ésta cantidad de producto
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // Me paro en el depósito que creé
                if (string.Equals(deposito.GetNombre, "deposito", System.StringComparison.OrdinalIgnoreCase))
                {
                    // Recorro las secciones del deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // Itero producto por producto en la lista de productos de la seccion
                        foreach (IProducto producto in seccion.Productos)
                        {
                            // Si hay al menos un producto con el codigo ingresado que no incluya la nueva categoría
                            // entonces el test falla y termino la ejecución del bucle
                            if (producto.GetCodigo.Equals(codigoUno)
                            && !producto.ExisteCategoria("nuevaCategoria"))
                            {
                                existenCategorias = false;
                                break;
                            }
                        }
                    }
                }
            }

            // Compruebo que las categorias se hayan asignado correctamente
            Assert.AreEqual(existenCategorias, true);
        }

        [Test]
        public void AumentarStock()
        {
            // Cantidad inicial de productos
            int cantidadInicial = 40;

            // Cantidad de productos que agrego
            int cantidadAgregados = 20;
            
            // Creo un depósito
            admin.CrearDeposito("deposito", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("seccion", 100, "deposito");

            // Le digo al usuario que de de alta un producto
            usuario.AltaProducto("producto1", 100, 1023, "marca1", new List<string>(){"categoria1"}, "seccion", "deposito", cantidadInicial);

            // El administrador aumenta el stock haciendo una compra
            admin.AumentarStock("deposito", cantidadAgregados, 1023);

            // Variable local en donde cuento la cantidad de productos que hay almacenados
            int cantidadProductos = 0;

            // Compruebo que agregué ésta cantidad de producto
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // Me paro en el depósito que creé
                if (string.Equals(deposito.GetNombre, "deposito", System.StringComparison.OrdinalIgnoreCase))
                {
                    // Voy seccion por seccion contando la cantidad de productos que hay con el código agregado
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        cantidadProductos += seccion.CantidadStock(1023);
                    }
                }
            }

            // Compruebo que la cantidad del producto que tengo es la que agregué
            Assert.AreEqual(cantidadInicial + cantidadAgregados, cantidadProductos);
        }

        /// <summary>
        /// Test para verificar la función de visualizar las ventas de un día dado
        /// </summary>

        [Test]
        public void VisualizarVentasEnDia()
        {
            // Creo un depósito
            admin.CrearDeposito("deposito", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("seccion", 100, "deposito");

            // El usuario da de alta un producto
            usuario.AltaProducto("producto1", 100, 101, "marca1", new List<string>(){"categoria1"}, "seccion", "deposito", 20);
            
            // El usuario da de alta otro producto
            usuario.AltaProducto("producto2", 100, 200, "marca2", new List<string>(){"categoria2"}, "seccion", "deposito", 20);

            // Dia de la venta
            int diaVenta = 20;

            // Mes de la venta
            int mesVenta = 2;

            // Año de venta
            int añoVenta = 2023;

            // Fecha de la venta
            DateTime fechaVenta = new DateTime(añoVenta, mesVenta, diaVenta);

            // Creo una venta total
            VentaTotal ventaHoy1 = new VentaTotal(fechaVenta);

            // Creo otra venta total
            VentaTotal ventaHoy2 = new VentaTotal(fechaVenta);

            // Agrego venta del producto 1 a la venta 1
            ventaHoy1.AgregarVenta(101, 10, "seccion", "deposito");

            // Agrego venta del producto 2 a la venta 1
            ventaHoy1.AgregarVenta(200, 5, "seccion", "deposito");

            // Agrego venta del producto 1 a la venta 2
            ventaHoy2.AgregarVenta(101, 1, "seccion", "deposito");

            // Agrego venta del producto 2 a la venta 2
            ventaHoy2.AgregarVenta(200, 2, "seccion", "deposito");

            // Agrego la venta total al contenedor de ventas por fecha
            ContenedorVentasPorFecha.AgregarVentaPorFecha(ventaHoy1, fechaVenta);

            // Agrego la segunda venta total al contenedor de ventas por fecha
            ContenedorVentasPorFecha.AgregarVentaPorFecha(ventaHoy2, fechaVenta);

            // Creo una lista que contenga las dos ventas que hice en ésta fecha
            List<VentaTotal> ventasHoy = new List<VentaTotal>() {ventaHoy1, ventaHoy2};

            // Le pido al Administrador que me devuelva las ventas de hoy
            List<VentaTotal> ventaResultado = admin.VentasEnDia(fechaVenta);

            // Si las dos listas son iguales, el test da correcto
            Assert.AreEqual(ventasHoy, ventaResultado);
        }
    }
}
