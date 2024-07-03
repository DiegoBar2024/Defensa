using NUnit.Framework;
using ProyectoFinal;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace Tests
{
    /// <summary>
    /// Clase que prueba las funcionalidades del Usuario
    /// </summary>
    [TestFixture]
    public class TestsUsuario
    {
        private Administrador admin;

        private Usuario usuario;

        /// <summary>
        /// Se crean objetos Administrador y Usuario, sobre los cuales se harán los test.
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
        /// Limpieza de depósitos
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            // Le digo que antes de empezar elimine todos los depósitos
            ContenedorDepositos.EliminarDepositos();
        }

        
        /// <summary>
        /// Método de prueba para RealizarVenta de usuario
        /// La entrada va a ser una matriz de 5 columnas y n filas donde cada fila representa una venta individual
        /// Se hace ya que en una clase de Test no podemos interactuar directamente por consola con el usuario
        /// </summary>
        /// <param name="entradaUsuario"></param>
        public static void RealizarVenta(string[,] entradaUsuario)
        {
            // Especifico fecha de la venta
            DateTime fechaVenta = DateTime.Now;

            // Instancio una venta
            VentaTotal ventaTotal = new VentaTotal(fechaVenta);

            // Inicializo la variable booleana que me diga si sigo pidiendo datos al usuario
            bool pidiendoDatos = true;

            // Inicializo un valor que me identifique una venta individual
            int numeroVenta = 0;

            while (pidiendoDatos)
            {
                // FASE 1: PEDIR DATOS DE LA VENTA AL USUARIO

                // Pido al usuario el código del producto que se está vendiendo
                System.Console.WriteLine("Ingrese código del producto: ");
                int codigoProducto = Convert.ToInt32(entradaUsuario[numeroVenta, 0]);
                System.Console.WriteLine($"El código del producto ingresado es {codigoProducto}");

                // // Pido al usuario la cantidad de stock que estoy vendiendo
                System.Console.WriteLine("Ingrese cantidad de stock vendido: ");
                int stock = Convert.ToInt32(entradaUsuario[numeroVenta, 1]);
                System.Console.WriteLine($"La cantidad de stock a vender es {stock}");

                // // Pido al usuario el nombre de la sección de donde estoy sacando el producto
                System.Console.WriteLine("Ingrese el nombre de sección de donde está vendiendo: ");
                string nombreSeccion = entradaUsuario[numeroVenta, 2];
                System.Console.WriteLine($"El nombre de la sección de donde se vende es '{nombreSeccion}'");

                // // Pido al usuario el nombre del depósito de donde se está sacando el producto
                System.Console.WriteLine("Ingrese el nombre del depósito de donde se está vendiendo: ");
                string nombreDeposito = entradaUsuario[numeroVenta, 3];
                System.Console.WriteLine($"El nombre del deposito de donde se vende es '{nombreDeposito}'");

                // FASE 2: AGREGAR LA VENTA INDIVIDUAL A LA LISTA DE VENTAS INDIVIDUALES

                // Agrego la venta individual a la lista de ventas individuales
                ventaTotal.AgregarVenta(codigoProducto, stock, nombreSeccion, nombreDeposito);

                // FASE 3: PREGUNTAR AL USUARIO SI QUIERE HACER OTRA VENTA

                // Pregunto al usuario si quiere hacer otra venta
                System.Console.WriteLine("¿Quiere agregar otro producto a su venta? (S/N)");
                string terminarVenta = entradaUsuario[numeroVenta, 4];
                System.Console.WriteLine(terminarVenta);

                // En caso que el usuario quiera terminar de vender, que me setee la bandera a false
                if (terminarVenta.Trim().Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    pidiendoDatos = false;
                }

                // Avanzo a la siguiente venta ingresada
                numeroVenta ++;
            }

            // Delego la responsabilidad de disminuir el stock de una venta a VentaTotal
            ventaTotal.DisminuirStockTotal();

            // Delego la responsabilidad de agregar una venta por fecha a la clase ContenedorVentasPorFecha
            ContenedorVentasPorFecha.AgregarVentaPorFecha(ventaTotal, fechaVenta);
        }

        /// <summary>
        /// Test que comprueba la funcionalidad de dar de alta un producto
        /// </summary>

        [Test]
        public void DarAltaProducto()
        {
            // Creo un depósito
            admin.CrearDeposito("nuevoDeposito", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("nuevaSeccion", 100, "nuevoDeposito");

            // Codigo del producto a dar de alta
            int codigo = 1024;

            // Creo la lista de categorias del producto
            List<string> categorias = new List<string>() {"Bebidas", "Vinos"};

            // Stock de producto que voy a dar de alta
            int stock = 10;

            // El usuario da de alta un producto
            usuario.AltaProducto("nuevoProducto", 100, codigo, "marca1", categorias, "nuevaSeccion", "nuevoDeposito", stock);

            // Creación de bandera
            bool existeProducto = false;

            // Itero deposito por deposito
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // En caso que el deposito sea el buscado
                if (string.Equals(deposito.GetNombre, "nuevoDeposito", StringComparison.OrdinalIgnoreCase))
                {
                    // Itero seccion por seccion
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que la seccion sea la buscada
                        if (string.Equals(seccion.GetNombre, "nuevaSeccion", StringComparison.OrdinalIgnoreCase))
                        {
                            // Itero producto por producto
                            foreach (IProducto producto in seccion.Productos)
                            {
                                // En caso que se cumpla que:
                                // El codigo del producto haya sido el que el usuario dio de alta
                                // Las categorias del producto sean las que el usuario hayan sido dados de alta
                                // El stock que tenga del producto sea el mismo que especifiqué cuando lo di de alta
                                if (producto.GetCategorias.Equals(categorias)
                                && producto.GetCodigo.Equals(codigo)
                                && seccion.CantidadStock(producto.GetCodigo).Equals(stock))
                                {
                                    // Seteo la bandera a true
                                    existeProducto = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

        // Compruebo que el producto ingresado efectivamente exista
        Assert.AreEqual(existeProducto, true);
        }

        /// <summary>
        /// Test que prueba la función de realizar la venta de un producto
        /// </summary>
        [Test]
        public void RealizarVentaUnProducto()
        {
            // Creo un depósito
            admin.CrearDeposito("miDeposito", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("miSeccion", 100, "miDeposito");

            // Codigo del producto a dar de alta
            int codigo = 976;

            // Creo la lista de categorias del producto
            List<string> categorias = new List<string>() {"Bebidas", "Vinos"};

            // Stock de producto que voy a dar de alta
            int stock = 50;

            // El usuario da de alta un producto
            usuario.AltaProducto("miProducto", 100, codigo, "marca1", categorias, "miSeccion", "miDeposito", stock);

            // Le digo al usuario que realice una venta
            // Para que el test funcione bien, vamos a vender 20 unidades de miProducto de la seccion miSeccion en el depósito miDeposito
            // En este caso como mi stock inicial es 50, el valor en stock resultante va a ser 30
            TestsUsuario.RealizarVenta(new string[,] {{"976", "20", "miSeccion", "miDeposito", "N"}});

            // Asigno bandera booleana
            bool ventaRealizada = false;

            // Itero seccion por seccion en la lista de secciones
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // En caso que llegue al depósito que quiero, me paro
                if (string.Equals(deposito.GetNombre, "miDeposito", StringComparison.OrdinalIgnoreCase))
                {
                    // Itero seccion por seccion en la lista de secciones en el deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que llegue al depósito que quiero, me paro
                        if (string.Equals(seccion.GetNombre, "miSeccion", StringComparison.OrdinalIgnoreCase))
                        {
                            // Itero producto por producto en la lista de productos de la seccion
                            foreach (IProducto producto in seccion.Productos)
                            {
                                // En caso que el producto tenga mi código y tenga un stock de 30 (luego de la venta) seteo la bandera a true
                                if (producto.GetCodigo.Equals(codigo) && 
                                seccion.CantidadStock(codigo).Equals(30))
                                {
                                    ventaRealizada = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // Compruebo que el stock haya disminuido correctamente
            Assert.AreEqual(ventaRealizada, true);
        }

        /// <summary>
        /// Test que prueba la función de realizar la venta de dos productos
        /// </summary>

        [Test]
        public void RealizarVentaDosProductos()
        {
            // Creo un depósito
            admin.CrearDeposito("miDeposito1", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("miSeccion1", 100, "miDeposito1");

            // Creo el codigo del primer producto a dar de alta
            int codigo1 = 789;

            // Creo el codigo del segundo producto a dar de alta
            int codigo2 = 345;

            // Creo la lista de categorias del producto 1
            List<string> categorias1 = new List<string>() {"Bebidas", "Vinos"};

            // Creo la lista de categorias del producto 2
            List<string> categorias2 = new List<string> {"Alimentos"};

            // Stock de producto 1 que voy a dar de alta
            int stock1 = 50;

            // Stock de producto 2 que voy a dar de alta
            int stock2 = 10;

            // El usuario da de alta producto 1
            usuario.AltaProducto("producto1", 100, codigo1, "marca1", categorias1, "miSeccion1", "miDeposito1", stock1);
    
            // El usuario da de alta producto 2
            usuario.AltaProducto("producto2", 70, codigo2, "marca2", categorias2, "miSeccion1", "miDeposito1", stock2);

            // El usuario hace una venta
            RealizarVenta(new string[,]{{"789", "20", "miSeccion1", "miDeposito1", "S"},
                                        {"345", "5", "miSeccion1", "miDeposito1", "N"}});

            // Inicializo bandera
            bool ventaRealizada = false;

            // Itero seccion por seccion en la lista de secciones
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // En caso que llegue al depósito que quiero, me paro
                if (string.Equals(deposito.GetNombre, "miDeposito1", StringComparison.OrdinalIgnoreCase))
                {
                    // Itero seccion por seccion en la lista de secciones en el deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que llegue al depósito que quiero, me paro
                        if (string.Equals(seccion.GetNombre, "miSeccion1", StringComparison.OrdinalIgnoreCase))
                        {
                            // Compruebo que el stock resultante de ambos productos en la seccion sean los que vendí
                            if (seccion.CantidadStock(codigo1).Equals(30)
                            && seccion.CantidadStock(codigo2).Equals(5))
                            {
                                // Seteo la bandera a True
                                ventaRealizada = true;
                                break;
                            }
                        }
                    }
                }
            }

            // Compruebo que la venta se haya realizado correctamente
            Assert.AreEqual(ventaRealizada, true);
        }
    }
}