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
    /// Clase que tiene los test para la verificación de las funciones que tiene un proveedor
    /// </summary>
    [TestFixture]
    public class TestsProveedor
    {
        private Administrador admin;

        private Usuario usuario;

        private Proveedor proveedor;
        
        /// <summary>
        /// Se crean instancias a Administrador, Proveedor y Usuario, sobre los cuales se harán los test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            admin = new Administrador("admin");
            usuario = new Usuario("usuario");
            proveedor = new Proveedor("proveedor");
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
        /// Test para comprobar la función de visualizar el stock de un producto en dos secciones distintas
        /// </summary>
        [Test]
        public void VisualizarStockUnProductoDosSecciones()
        {
            // Creo un depósito
            admin.CrearDeposito("miDeposito2", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("miSeccion2a", 100, "miDeposito2");

            // Creo otra seccion
            admin.CrearSeccion("miSeccion2b", 100, "miDeposito2");

            // Creo el codigo del producto a dar de alta
            int codigo1 = 891;

            // Creo la lista de categorias del producto 1
            List<string> categorias1 = new List<string>() {"Bebidas", "Vinos"};

            // Stocks de producto que voy a dar de alta
            int stock1 = 50;
            int stock2 = 30;

            // El usuario da de alta producto 1
            usuario.AltaProducto("producto1", 100, codigo1, "marca1", categorias1, "miSeccion2a", "miDeposito2", stock1);
    
            // El usuario da de alta producto 2
            usuario.AltaProducto("producto1", 100, codigo1, "marca1", categorias1, "miSeccion2b", "miDeposito2", stock2);

            // El proveedor visualiza el stock
            Dictionary<string ,Dictionary<string, int>> stockProducto = proveedor.VisualizarStock(codigo1);

            // Creo una bandera booleana para cada seccion
            bool seccionUnoCorrecta = false;
            bool seccionDosCorrecta = false;

            // Itero para cada uno de los depositos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // En caso que llegue al depósito que quiero, me paro
                if (string.Equals(deposito.GetNombre, "miDeposito2", StringComparison.OrdinalIgnoreCase))
                {
                    // Itero para cada una de las secciones dentro del deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que 'Seccion 2a' tenga 50 productos de los que di de alta en stock 
                        // y coincida con la información del proveedor, que me setee la primera bandera a true
                        if (string.Equals(seccion.GetNombre, "miSeccion2a", StringComparison.OrdinalIgnoreCase)
                        && seccion.CantidadStock(codigo1).Equals(stockProducto["miDeposito2"]["miSeccion2a"]))
                        {
                            // Seteo el flag correspondiente a true
                            seccionUnoCorrecta = true;
                        }

                        // En caso que 'Seccion 2b' tenga 30 productos de los que di de alta en stock 
                        // y coincida con la información del proveedor, que me setee la segunda bandera a true
                        if (string.Equals(seccion.GetNombre, "miSeccion2b", StringComparison.OrdinalIgnoreCase)
                        && seccion.CantidadStock(codigo1).Equals(stockProducto["miDeposito2"]["miSeccion2b"]))
                        {
                            // Seteo el flag correspondiente a true
                            seccionDosCorrecta = true;
                        }
                    }
                }
            }

            // Compruebo que ambas secciones que informé al proveedor verifiquen
            Assert.AreEqual(seccionUnoCorrecta && seccionDosCorrecta, true);
        }

        /// <summary>
        /// Test para comprobar la función de visualizar el stock de un producto en dos depósitos distintos
        /// </summary>

        [Test]
        public void VisualizarStockUnProductoDosDepositos()
        {
            // Creo un depósito
            admin.CrearDeposito("miDeposito2a", "ubicacion", 100, 100);

            // Creo otro depósito
            admin.CrearDeposito("miDeposito2b", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("miSeccion2a", 100, "miDeposito2a");

            // Creo otra seccion
            admin.CrearSeccion("miSeccion2b", 100, "miDeposito2b");

            // Creo el codigo del producto a dar de alta
            int codigo1 = 900;

            // Creo la lista de categorias del producto 1
            List<string> categorias1 = new List<string>() {"Bebidas", "Vinos"};

            // Stocks de producto que voy a dar de alta
            int stock1 = 50;
            int stock2 = 30;

            // El usuario da de alta producto 1
            usuario.AltaProducto("producto1", 100, codigo1, "marca1", categorias1, "miSeccion2a", "miDeposito2a", stock1);
    
            // El usuario da de alta producto 2
            usuario.AltaProducto("producto1", 100, codigo1, "marca1", categorias1, "miSeccion2b", "miDeposito2b", stock2);

            // El proveedor visualiza el stock
            Dictionary<string ,Dictionary<string, int>> stockProducto = proveedor.VisualizarStock(codigo1);

            // Creo una bandera booleana para cada seccion
            bool seccionUnoCorrecta = false;
            bool seccionDosCorrecta = false;

            // Itero para cada uno de los depositos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // En caso que llegue al depósito "miDeposito2a" que quiero, me paro
                if (string.Equals(deposito.GetNombre, "miDeposito2a", StringComparison.OrdinalIgnoreCase))
                {
                    // Itero para cada una de las secciones dentro del deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que 'Seccion 2b' tenga 30 productos de los que di de alta en stock 
                        // y coincida con la información del proveedor, que me setee la segunda bandera a true
                        if (string.Equals(seccion.GetNombre, "miSeccion2a", StringComparison.OrdinalIgnoreCase)
                        && seccion.CantidadStock(codigo1).Equals(stockProducto["miDeposito2a"]["miSeccion2a"]))
                        {
                            // Seteo el flag correspondiente a true
                            seccionUnoCorrecta = true;
                        }
                    }
                }

                // En caso que llegue al depósito "miDeposito2b" que quiero, me paro
                if (string.Equals(deposito.GetNombre, "miDeposito2b", StringComparison.OrdinalIgnoreCase))
                {
                    // Itero para cada una de las secciones dentro del deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que 'Seccion 2a' tenga 50 productos de los que di de alta en stock 
                        // y coincida con la información del proveedor, que me setee la primera bandera a true
                        if (string.Equals(seccion.GetNombre, "miSeccion2b", StringComparison.OrdinalIgnoreCase)
                        && seccion.CantidadStock(codigo1).Equals(stockProducto["miDeposito2b"]["miSeccion2b"]))
                        {
                            // Seteo el flag correspondiente a true
                            seccionDosCorrecta = true;
                        }
                    }
                }
            }

            // Compruebo que ambas secciones den bien
            Assert.AreEqual(seccionUnoCorrecta && seccionDosCorrecta, true);
        }

        /// <summary>
        /// Test para verificar la funcionalidad de visualizar el stock de dos productos distintos en dos secciones distintas
        /// </summary>

        [Test]
        public void VisualizarStockDosProductosDosSecciones()
        {
            // Creo un depósito
            admin.CrearDeposito("miDeposito", "ubicacion", 100, 100);

            // Creo una seccion dentro de éste deposito
            admin.CrearSeccion("miSeccion", 100, "miDeposito");

            // Creo otra seccion dentro de éste deposito
            admin.CrearSeccion("miSeccion2", 100, "miDeposito");

            // Codigo del producto 1 a ingresar
            int codigo1 = 432;

            // Codigo del producto 2 a ingresar
            int codigo2 = 410;

            // Categorías del producto 1
            List<string> categorias1 = new List<string>() {"Bebidas", "Vinos"};

            // Categorías del producto 2
            List<string> categorias2 = new List<string>() {"Alimentos"};

            // Cantidad en stock a dar de alta del producto 1
            int stock1 = 40;

            // Cantidad en stock a dar de alta del producto 2
            int stock2 = 20;

            // El usuario da de alta producto 1 en la seccion 'miSeccion' del depósito 'miDeposito'
            usuario.AltaProducto("producto1", 100, codigo1, "marca1", categorias1, "miSeccion", "miDeposito", stock1);
    
            // El usuario da de alta producto 2 en la seccion 'miSeccion2' del depósito 'miDeposito'
            usuario.AltaProducto("producto2", 100, codigo2, "marca2", categorias2, "miSeccion2", "miDeposito", stock2);

            // Creo una bandera booleana para cada sección que estoy probando
            bool seccionUnoCorrecta = false;
            bool seccionDosCorrecta = false;

            // El proveedor visualiza el stock del producto 1
            Dictionary<string, Dictionary<string, int>> stockProducto = proveedor.VisualizarStock(codigo1);

            // Itero deposito por deposito en la lista de depositos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // Si llego al depósito de nombre 'miDeposito', me paro ahi
                if (string.Equals(deposito.GetNombre, "miDeposito", StringComparison.OrdinalIgnoreCase))
                {
                    // Itero para cada una de las secciones dentro del depósito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que esté parado en la seccion 'miSeccion' y compruebo que el stock de producto 1 es el mismo que
                        // le estoy pasando en la informacion al proveedor (40) seteo la bandera a true
                        if (string.Equals(seccion.GetNombre, "miSeccion", StringComparison.OrdinalIgnoreCase)
                        && seccion.CantidadStock(codigo1).Equals(stockProducto["miDeposito"]["miSeccion"]))
                        {
                            seccionUnoCorrecta = true;
                        }

                        // En caso que esté parado en la seccion 'miSeccion2' y compruebo que el stock de producto 1 es el mismo que
                        // le estoy pasando en la informacion al proveedor (0) seteo la bandera a true
                        if (string.Equals(seccion.GetNombre, "miSeccion2", StringComparison.OrdinalIgnoreCase)
                        && seccion.CantidadStock(codigo1).Equals(stockProducto["miDeposito"]["miSeccion2"]))
                        {
                            seccionDosCorrecta = true;
                        }
                    }
                }
            }

            // Compruebo que ambas secciones tengan los datos correctos
            Assert.AreEqual(seccionUnoCorrecta && seccionDosCorrecta, true);
        }
    }
}