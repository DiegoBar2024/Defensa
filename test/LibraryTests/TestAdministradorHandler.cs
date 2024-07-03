using NUnit.Framework;
using Ucu.Poo.TelegramBot;
using Telegram.Bot.Types;
using ProyectoFinal;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System;

namespace Tests
{
    /// <summary>
    /// Clase que contiene los casos de prueba para AdministradorHandler
    /// </summary>
    public class TestAdministradorHandler
    {
        AdministradorHandler handler;
        Message message;
        Administrador admin;

        [SetUp]
        public void Setup()
        {
            handler = AdministradorHandler.GetInstance();
            message = new Message();
            message.From = new User();
            message.From.Id = 0;

            admin = new Administrador("Administrador");
        }

        /// <summary>
        /// Le digo que antes de empezar elimine todos los depósitos
        /// </summary>
        public void Teardown()
        {
            ContenedorDepositos.EliminarDepositos();
        }

        [Test]
        public void TestHandleDeposito()
        {
            // Creo la cadena con la cual voy a comparar
            StringBuilder cadenaComparacion = new StringBuilder();

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string ADMINISTRADOR_MENSAJE = "Como administrador podes hacer:\n1) Crear deposito\n2) Dar de alta usuario\n3) Definir secciones\n4) Dar de alta proveedor\n5) Crear una nueva categoría\n6) Registrar una compra\n7) Visualizar las ventas en un día\n8) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            cadenaComparacion.AppendLine("Depósito creado correctamente. Las características del depósito son: ");

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            // Doy mensaje
            message.Text = "1";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame el nombre del deposito"));

            // Doy mensaje
            message.Text = "Deposito";

            // Actualizo cadena de comparacion
            cadenaComparacion.AppendLine($" Nombre: {message.Text}");

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame la ubicación del deposito"));

            // Doy mensaje
            message.Text = "Ubicacion";
            
            // Actualizo cadena de comparacion
            cadenaComparacion.AppendLine($" Ubicacion: {message.Text}");

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame la capacidad del deposito"));

            // Doy mensaje
            message.Text = "100";

            // Actualizo cadena de comparacion
            cadenaComparacion.AppendLine($" Capacidad: {message.Text}");

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo($"{cadenaComparacion}\n{ADMINISTRADOR_MENSAJE}"));
        }

        [Test]
        public void TestHandleAltaUsuario()
        {
            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string ADMINISTRADOR_MENSAJE = "Como administrador podes hacer:\n1) Crear deposito\n2) Dar de alta usuario\n3) Definir secciones\n4) Dar de alta proveedor\n5) Crear una nueva categoría\n6) Registrar una compra\n7) Visualizar las ventas en un día\n8) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            // Supongo que le doy la opción 2
            message.Text = "2";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el nombre del usuario. Ésta será la contraseña para su perfil."));

            message.Text = "Usuario";

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("¿El usuario tiene permisos de administrador? (true/false)"));

            message.Text = "True";

            handler.Handle(message, out response);

            // Lo que hago ahora es llamar al intermediario de administrador para que me devuelva una cadena con todos los depositos creados
            string depositosExistentes = IntermediarioAdministrador.NombresDepositos();

            Assert.That(response, Is.EqualTo($"{depositosExistentes}\nDame los depósitos a los que puede tener acceso el usuario: <<deposito1>>, <<deposito2>> ... <<depositoN>>"));

            message.Text = "Deposito";

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo($"Usuario dado de alta correctamente.\n{ADMINISTRADOR_MENSAJE}"));
        }

        [Test]
        public void TestHandleCrearSecciones()
        {
            // Digo al administrador que de de alta una seccion
            admin.CrearDeposito("Deposito", "ubicacion", 100);

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string ADMINISTRADOR_MENSAJE = "Como administrador podes hacer:\n1) Crear deposito\n2) Dar de alta usuario\n3) Definir secciones\n4) Dar de alta proveedor\n5) Crear una nueva categoría\n6) Registrar una compra\n7) Visualizar las ventas en un día\n8) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            // Supongo que le doy la opción 3
            message.Text = "3";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el nombre de la seccion"));

            message.Text = "Seccion";

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame la capacidad de la seccion"));

            // Doy mensaje
            message.Text = "100";

            // Le digo al handler que lo maneje
            handler.Handle(message, out response);
            
            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame el nombre del depósito al que pertenece la seccion"));

            // Doy mensaje
            message.Text = "Deposito";

            // Le digo al handler que lo maneje
            handler.Handle(message, out response);
            
            // Compruebo resultados
            Assert.That(response, Is.EqualTo($"Seccion creada correctamente.\n{ADMINISTRADOR_MENSAJE}"));
        }

        [Test]
        public void TestHandleAltaProveedores()
        {
            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string ADMINISTRADOR_MENSAJE = "Como administrador podes hacer:\n1) Crear deposito\n2) Dar de alta usuario\n3) Definir secciones\n4) Dar de alta proveedor\n5) Crear una nueva categoría\n6) Registrar una compra\n7) Visualizar las ventas en un día\n8) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            message.Text = "4";

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el nombre del proveedor. Ésta será la contraseña para su perfil."));
            
            message.Text = "Proveedor";

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo($"Proveedor dado de alta correctamente.\n{ADMINISTRADOR_MENSAJE}"));
        }

        [Test]
        public void TestHandleCrearCategorias()
        {
            // Digo al administrador que cree un deposito
            admin.CrearDeposito("deposito", "ubicacion", 100);

            // Digo al administrador que cree una seccion
            admin.CrearSeccion("seccion", 100, "deposito");

            // Creo un nuevo usuario con los permisos correspondientes
            Usuario usuario = new Usuario("usuario", new List<string>() {"deposito"});

            // Digo al usuario que de de alta un producto
            usuario.AltaProducto("nuevoProducto", 100, 700, "marca1", new List<string>() {"categoria1, categoria2"}, "seccion", "deposito", 50);

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string ADMINISTRADOR_MENSAJE = "Como administrador podes hacer:\n1) Crear deposito\n2) Dar de alta usuario\n3) Definir secciones\n4) Dar de alta proveedor\n5) Crear una nueva categoría\n6) Registrar una compra\n7) Visualizar las ventas en un día\n8) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            message.Text = "5";

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame la categoría que quieras crear"));

            // Doy mensaje
            message.Text = "Categoria";

            // Le digo al handler que lo maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame los codigos de los productos para aplicar la categorias: <<codigo1>>, <<codigo2>>, ..., <<codigoN>>"));

            // Doy mensaje
            message.Text = "700";

            // Le digo al handler que lo maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo($"Categoría creada correctamente.\n{ADMINISTRADOR_MENSAJE}"));

        }

        [Test]
        public void TestHandleRegistrarCompra()
        {
            // Digo al administrador que cree un deposito
            admin.CrearDeposito("depositoF", "ubicacion", 100);

            // Digo al administrador que cree una seccion
            admin.CrearSeccion("seccionF", 100, "depositoF");

             // Creo un nuevo usuario con los permisos correspondientes
            Usuario usuario = new Usuario("usuario", new List<string>() {"depositoF"});

            // Digo al usuario que de de alta un producto
            usuario.AltaProducto("nuevoProducto", 100, 150, "marca1", new List<string>() {"categoria1, categoria2"}, "seccionF", "depositoF", 10);

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string ADMINISTRADOR_MENSAJE = "Como administrador podes hacer:\n1) Crear deposito\n2) Dar de alta usuario\n3) Definir secciones\n4) Dar de alta proveedor\n5) Crear una nueva categoría\n6) Registrar una compra\n7) Visualizar las ventas en un día\n8) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            // Doy mensaje
            message.Text = "6";

            // Digo al handler que se maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame el deposito en donde vas a registrar la compra"));

            // Doy mensaje
            message.Text = "depositoF";

            // Digo al handler que se maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame la cantidad de stock comprado de dicho producto"));

            // Doy mensaje
            message.Text = "5";

            // Digo al handler que se maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame el codigo del producto a comprar"));

            // Doy mensaje
            message.Text = "150";

            // Digo al handler que se maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo($"Se ha registrado la compra correctamente.\n{ADMINISTRADOR_MENSAJE}"));
        }

        [Test]
        public void TestHandleVisualizarVentas()
        {
            /// <summary>
            /// Instancio la fecha de hoy
            /// </summary>
            DateTime fechaHoy = DateTime.Now;
            // Genero una linea de comparacion
            StringBuilder lineaComparacion = new StringBuilder($"Para la fecha {fechaHoy.Day}/{fechaHoy.Month}/{fechaHoy.Year} se tienen las siguentes ventas:\n");

            // Digo al administrador que cree un deposito
            admin.CrearDeposito("depositoP", "ubicacion", 100);

            // Digo al administrador que cree una seccion
            admin.CrearSeccion("seccion", 100, "depositoP");

            // Creo un nuevo usuario con los permisos correspondientes
            Usuario usuario = new Usuario("usuario", new List<string>() {"depositoP"});

            // Digo al usuario que de de alta un producto
            usuario.AltaProducto("nuevoProducto", 100, 150, "marca1", new List<string>() {"categoria1, categoria2"}, "seccion", "depositoP", 50);

            // Digo al usuario que de de alta un producto
            usuario.AltaProducto("nuevoProducto2", 100, 151, "marca2", new List<string>() {"categoria1, categoria2"}, "seccion", "depositoP", 50);

            // Creo las ventas individuales
            List<string> ventaIndividual1 = new List<string>() {"150", "5", "seccion", "depositoP"};
            List<string> ventaIndividual2 = new List<string>() {"151", "5", "seccion", "depositoP"};

            // Digo al usuario que venda algunas unidades de cada producto
            usuario.RealizarVenta(new List<List<string>> {ventaIndividual1, ventaIndividual2});

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string ADMINISTRADOR_MENSAJE = "Como administrador podes hacer:\n1) Crear deposito\n2) Dar de alta usuario\n3) Definir secciones\n4) Dar de alta proveedor\n5) Crear una nueva categoría\n6) Registrar una compra\n7) Visualizar las ventas en un día\n8) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            // Doy mensaje
            message.Text = "7";

            // Que el handler se maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame el dia en el cual querés visualizar las ventas"));

            // Especifico el dia (dia actual)
            message.Text = Convert.ToString(fechaHoy.Day);

            // Que el handler se maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame el mes en el cual querés visualizar las ventas"));

            // Especifico el dia (mes actual)
            message.Text = Convert.ToString(fechaHoy.Month);

            // Que el handler se maneje
            handler.Handle(message, out response);

            // Compruebo resultados
            Assert.That(response, Is.EqualTo("Dame el año en el cual querés visualizar las ventas"));

            // Especifico el dia (año actual)
            message.Text = Convert.ToString(fechaHoy.Year);

            // Que el handler se maneje
            handler.Handle(message, out response);

            // Actualizo la linea de comparacion con las ventas
            lineaComparacion.AppendLine($"Se vendieron 5 unidades del producto de codigo 150");
            lineaComparacion.AppendLine($"Se vendieron 5 unidades del producto de codigo 151");

            // Compruebo resultados
            Assert.That(response, Is.EqualTo($"Venta visualizada correctamente.\n{lineaComparacion}\n{ADMINISTRADOR_MENSAJE}"));
        }
    }
}