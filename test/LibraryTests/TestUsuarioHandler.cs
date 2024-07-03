using NUnit.Framework;
using Ucu.Poo.TelegramBot;
using Telegram.Bot.Types;
using ProyectoFinal;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    /// <summary>
    /// Casos de prueba para UsuarioHandler
    /// </summary>
    public class TestUsuarioHandler
    {
        UsuarioHandler handler;
        Message message;
        Usuario usuario;
        Administrador admin;

        [SetUp]
        public void Setup()
        {
            handler = UsuarioHandler.GetInstance();
            message = new Message();
            message.From = new User();
            message.From.Id = 0;

            admin = new Administrador("admin");
        }

        public void Teardown()
        {
            // Le digo que antes de empezar elimine todos los depósitos
            ContenedorDepositos.EliminarDepositos();
        }

        [Test]
        public void TestHandleAltaProducto()
        {
            // Le digo al administrador que cree un deposito
            admin.CrearDeposito("Deposito", "ubicacion", 100);

            // Le digo al administrador que cree una seccion
            admin.CrearSeccion("Seccion", 100, "Deposito");

            // Configuro el usuario
            IntermediarioUsuario.ConfigurarUsuario(new List<string>() { "diego", "false", "Deposito" });

            // Creo una linea que vamos comparando
            StringBuilder lineaComparacion = new StringBuilder();

            // Agrego la primera linea
            lineaComparacion.AppendLine("Producto dado de alta correctamente. Características del producto:\n");

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string USUARIO_MENSAJE = "Como usuario puedes hacer lo siguiente:\n1) Dar de alta un producto\n2) Visualizar el deposito mas cercano que tenga stock de un producto\n3) Realizar una venta\n4) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            // Supongo que le doy la opción 1
            message.Text = "1";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el nombre del producto"));

            message.Text = "Producto";

            lineaComparacion.AppendLine($"Nombre: {message.Text}");

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el precio del producto"));

            message.Text = "100";

            lineaComparacion.AppendLine($"Precio: {message.Text}");

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el codigo del producto"));

            message.Text = "100";

            lineaComparacion.AppendLine($"Codigo: {message.Text}");

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame la marca del producto"));

            message.Text = "Marca";

            lineaComparacion.AppendLine($"Marca: {message.Text}");

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame las categorias del producto (<<categoria1>>, <<categoria2>>, ...)"));

            message.Text = "Categoria1, Categoria2";

            lineaComparacion.AppendLine($"Categorias: {message.Text}");

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el nombre de la seccion donde vas a dar de alta"));

            message.Text = "Seccion";

            lineaComparacion.AppendLine($"Seccion: {message.Text}");

            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el nombre del deposito donde vas a dar de alta"));

            // Doy mensaje
            message.Text = "Deposito";

            lineaComparacion.AppendLine($"Deposito: {message.Text}");

            // Le digo al handler que se maneje
            handler.Handle(message, out response);

            // Comparo resultados
            Assert.That(response, Is.EqualTo("Dame el stock de producto que vas a dar de alta"));

            // Doy mensaje
            message.Text = "100";

            // Le digo al handler que se maneje
            handler.Handle(message, out response);

            // Comparo resultados
            Assert.That(response, Is.EqualTo($"{lineaComparacion}\n{USUARIO_MENSAJE}"));
        }

        [Test]
        public void TestHandleDepositoMasCercano()
        {
            // Digo al administrador que de de alta un deposito
            admin.CrearDeposito("depositoX", "8 de Octubre y Garibaldi", 100);

            // Digo al administrador que de de alta un deposito
            admin.CrearDeposito("depositoY", "18 de Julio y Ejido", 100);

            // Digo al administrador que de de alta una seccion
            admin.CrearSeccion("seccion", 100, "depositoX");

            // Digo al administrador que de de alta una seccion
            admin.CrearSeccion("seccion", 100, "depositoY");

            // Creo un usuario donde tenga como acceso el deposito H
            usuario = new Usuario("usuario", new List<string>() { "depositoX", "depositoY" });

            // Configuro el usuario
            IntermediarioUsuario.ConfigurarUsuario(new List<string>() { "diego", "false", "depositoX, depositoY" });

            // Le digo al usuario que de de alta un producto
            usuario.AltaProducto("nuevoProducto", 100, 700, "marca1", new List<string>() { "categoria1, categoria2" }, "seccion", "depositoX", 50);

            // Le digo al usuario que de de alta un producto
            usuario.AltaProducto("nuevoProducto", 100, 700, "marca1", new List<string>() { "categoria1, categoria2" }, "seccion", "depositoY", 50);

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string USUARIO_MENSAJE = "Como usuario puedes hacer lo siguiente:\n1) Dar de alta un producto\n2) Visualizar el deposito mas cercano que tenga stock de un producto\n3) Realizar una venta\n4) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            // Supongo que le doy la opción 2
            message.Text = "2";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo el resultado
            Assert.That(response, Is.EqualTo("Dame el codigo del producto"));

            // Codigo producto
            message.Text = "700";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo el resultado
            Assert.That(response, Is.EqualTo("Dame tu ubicacion"));

            // Doy ubicacion
            message.Text = "Colonia y Ejido";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo el resultado
            Assert.That(response, Is.EqualTo($"El depósito más cercano con stock disponible es depositoY\n{USUARIO_MENSAJE}"));

            // Supongo que le doy la opción 2
            message.Text = "2";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo el resultado
            Assert.That(response, Is.EqualTo("Dame el codigo del producto"));

            // Codigo producto
            message.Text = "700";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo el resultado
            Assert.That(response, Is.EqualTo("Dame tu ubicacion"));

            // Doy ubicacion
            message.Text = "8 de Octubre y Comandante Braga";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo el resultado
            Assert.That(response, Is.EqualTo($"El depósito más cercano con stock disponible es depositoX\n{USUARIO_MENSAJE}"));
        }

        [Test]
        public void TestHandleRealizarVenta()
        {
            // Digo al administrador que de de alta un deposito
            admin.CrearDeposito("depositoJ", "ubicacionJ", 100);

            // Digo al administrador que de de alta una seccion
            admin.CrearSeccion("seccionJ", 100, "depositoJ");

            // Creo un usuario donde tenga como acceso el deposito H
            usuario = new Usuario("usuario", new List<string>() { "depositoJ" });

            // Configuro el proveedor
            IntermediarioUsuario.ConfigurarUsuario(new List<string>() { "diego", "false", "depositoJ" });

            // Le digo al usuario que de de alta un producto con el codigo anterior
            usuario.AltaProducto("nuevoProducto", 100, 998, "marca1", new List<string>() { "categoria1, categoria2" }, "seccionJ", "depositoJ", 50);

            // Le digo al usuario que de de alta un producto con el codigo anterior
            usuario.AltaProducto("nuevoProducto2", 100, 879, "marca2", new List<string>() { "categoria1, categoria2" }, "seccionJ", "depositoJ", 50);

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            string USUARIO_MENSAJE = "Como usuario puedes hacer lo siguiente:\n1) Dar de alta un producto\n2) Visualizar el deposito mas cercano que tenga stock de un producto\n3) Realizar una venta\n4) Salir";

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            // Supongo que le doy la opción 3
            message.Text = "3";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el codigo del producto"));

            // Mensaje de entrada
            message.Text = "998";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("Dame el stock del producto a vender"));

            // Mensaje de entrada
            message.Text = "20";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("Dame la sección de donde vas a extraer el producto a vender"));

            // Mensaje de entrada
            message.Text = "seccionJ";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("Dame el deposito de donde vas a extraer el producto a vender"));

            // Mensaje de entrada
            message.Text = "depositoJ";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("¿Quiere hacer alguna otra venta?\n1) Sí\n2) No"));

            // Mensaje de entrada
            message.Text = "1";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("Dame el codigo del producto"));

            // Mensaje de entrada
            message.Text = "879";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("Dame el stock del producto a vender"));

            // Mensaje de entrada
            message.Text = "10";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("Dame la sección de donde vas a extraer el producto a vender"));

            // Mensaje de entrada
            message.Text = "seccionJ";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("Dame el deposito de donde vas a extraer el producto a vender"));

            // Mensaje de entrada
            message.Text = "depositoJ";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo("¿Quiere hacer alguna otra venta?\n1) Sí\n2) No"));

            // Mensaje de entrada
            message.Text = "2";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            // Compruebo resultado
            Assert.That(response, Is.EqualTo($"Venta realizada correctamente\n{USUARIO_MENSAJE}"));
        }
    }
}