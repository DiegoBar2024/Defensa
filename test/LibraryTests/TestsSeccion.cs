using NUnit.Framework;
using ProyectoFinal;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tests
{
    /// <summary>
    /// Clase para las pruebas de la clase Seccion, especialmente para comprobar a la hora de 
    /// modificar el stock de una sección.
    /// </summary>
    [TestFixture]
    public class TestsSeccion
    {
        private Administrador admin;

        private Usuario usuario;

        /// <summary>
        /// Se crean objetos admin y usuario
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
        /// Limpieza test
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            // Le digo que antes de empezar elimine todos los depósitos
            ContenedorDepositos.EliminarDepositos();
        }

        /// <summary>
        /// Test para comprobar la funcionalidad de modificar el stock de una determinada 
        /// sección.
        /// </summary>
        [Test]
        public void TestModificarStock()
        {
            // Creo un depósito
            admin.CrearDeposito("nuevoDeposito", "ubicacion", 100, 100);

            // Creo una seccion
            admin.CrearSeccion("nuevaSeccion", 100, "nuevoDeposito");

            // Creo otra seccion
            admin.CrearSeccion("nuevaSeccion1", 100, "nuevoDeposito");

            // Codigo del producto a dar de alta
            int codigo = 1011;

            // Creo la lista de categorias del producto
            List<string> categorias = new List<string>() {"Bebidas", "Vinos"};

            // Stock de producto que voy a dar de alta
            int stock = 10;

            // El usuario da de alta un producto
            usuario.AltaProducto("nuevoProducto", 100, codigo, "marca1", categorias, "nuevaSeccion", "nuevoDeposito", stock);

            // Doy de alta un producto en la segunda seccion
            usuario.AltaProducto("nuevoProducto", 100, codigo, "marca1", categorias, "nuevaSeccion1", "nuevoDeposito", 20);

            // Bandera booleana
            bool stockCorrecto = false;

            // Itero deposito por deposito
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                // En caso que el deposito sea el buscado
                if (string.Equals(deposito.GetNombre, "nuevoDeposito", StringComparison.OrdinalIgnoreCase))
                {
                    // Itero seccion por seccion
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que llegue a la sección 'nuevaSeccion1', que me modifique el stock de producto
                        if (string.Equals(seccion.GetNombre, "nuevaSeccion1", StringComparison.OrdinalIgnoreCase))
                        {
                            seccion.ModificarStock(codigo, -10);

                            // Si me quedan en stock 10 unidades de producto en este caso, seteo la bandera a true
                            if (seccion.CantidadStock(codigo).Equals(10))
                            {
                                stockCorrecto = true;
                            }
                        }
                    }
                }
            }

            // Compruebo que el test sea valido
            Assert.AreEqual(true, stockCorrecto);
        }
    }
}