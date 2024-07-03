using NUnit.Framework;
using Ucu.Poo.TelegramBot;
using Telegram.Bot.Types;
using ProyectoFinal;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    /// <summary>
    /// Casos de prueba para ProveedorHandler
    /// </summary>
    public class TestProveedorHandler
    {
        ProveedorHandler handler;
        Message message;

        Administrador admin;
        Usuario usuario;
        Proveedor proveedor;

        [SetUp]
        public void Setup()
        {
            handler = ProveedorHandler.GetInstance();
            message = new Message();
            message.From = new User();
            message.From.Id = 0;

            admin = new Administrador("admin");
            proveedor = new Proveedor("proveedor");
        }

        public void Teardown()
        {
            // Le digo que antes de empezar elimine todos los depósitos
            ContenedorDepositos.EliminarDepositos();
        }

        [Test]
        public void TestHandleProveedor()
        {
            // Elimino depositos
            ContenedorDepositos.EliminarDepositos();

            // Creo una cadena en donde voy a almacenar el resultado
            StringBuilder lineaComparacion = new StringBuilder();

            // Inicializo el mensaje del proveedor
            string PROVEEDOR_MENSAJE = "Como proveedor puedes hacer lo siguiente:\n1) Visualizar stock de un producto por código\n2) Salir";

            // Digo al administrador que de de alta un deposito
            admin.CrearDeposito("depositoH", "ubicacionH", 100);

            // Digo al administrador que de de alta una seccion
            admin.CrearSeccion("seccionH", 100, "depositoH");

            // Creo un usuario donde tenga como acceso el deposito H
            usuario = new Usuario("usuario", new List<string>() { "depositoH" });

            // Le digo al usuario que de de alta un producto con el codigo anterior
            usuario.AltaProducto("nuevoProducto", 100, 999, "marca1", new List<string>() { "categoria1, categoria2" }, "seccionH", "depositoH", 10);

            // Inicio la comunicación con el handler
            message.Text = handler.Keywords[0];

            // Inicializo la variable donde guardo la respuesta
            string response;

            // Le digo al handler que maneje el mensaje ingresado
            IHandler result = handler.Handle(message, out response);

            Assert.That(result, Is.Not.Null);
            Assert.That(response, Is.EqualTo(PROVEEDOR_MENSAJE));

            // Configuro el proveedor
            IntermediarioProveedor.ConfigurarProveedor(new List<string>() { "diego" });

            // Supongo que le doy la opción 1
            message.Text = "1";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            Assert.That(response, Is.EqualTo("Dame el código del producto cuyo stock querés ver"));

            // Especifico el codigo que quiero ver
            message.Text = "999";

            // Le digo al handler que maneje el mensaje ingresado
            handler.Handle(message, out response);

            lineaComparacion.AppendLine($"Para el depósito 'depositoH':");

            lineaComparacion.AppendLine($"   Cantidad en stock sección 'seccionH': 10");

            Assert.That(response, Is.EqualTo($"{lineaComparacion}\n{PROVEEDOR_MENSAJE}"));
        }
    }
}