using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Telegram.Bot.Types;
using ProyectoFinal;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "dirección".
    /// </summary>
    public class ProveedorHandler : BaseHandler
    {
        // Especifico las cadenas de mensaje que se pueden imprimir en el rol de Proveedor
        public const string PROVEEDOR_MENSAJE = "Como proveedor puedes hacer lo siguiente:\n1) Visualizar stock de un producto por código\n2) Salir";
        public const string VISUALIZAR_STOCK = "Dame el código del producto cuyo stock querés ver";
        public const string CODIGO_NO_ENCONTRADO = "No se encuentra el codigo del producto ingresado. Dame un nuevo codigo";
        public const string OPCION_NO_VALIDA = "La opción ingresada no es válida";

        // Creo la instancia ProveedorHandler privada estática para cumplir con Singleton
        private static ProveedorHandler proveedorHandler;

        private Dictionary<long, EstadoProveedor> stateForUser = new Dictionary<long, EstadoProveedor>();

        /// <summary>
        /// El estado del comando para un usuario que envía un mensaje. Cuando se comienza a procesar el comando para un
        /// nuevo usuario se agrega a este diccionario y cuando se termina de procesar el comando se remueve.
        /// </summary>
        public IReadOnlyDictionary<long, EstadoProveedor> StateForUser
        {
            get
            {
                return this.stateForUser;
            }
        }

        // Creo el constructor privado para ProveedorHandler por Singleton
        private ProveedorHandler()
        {
            this.Keywords = new string[] {"proveedor"};
        }

        // Creo un método que me permita obtener la misma instancia usando Singleton
        public static ProveedorHandler GetInstance()
        {
            // En caso que no exista ningún objeto, le digo que lo cree
            if (ProveedorHandler.proveedorHandler == null)
            {
                // Instancio un nuevo objeto llamando al constructor privado
                proveedorHandler = new ProveedorHandler();
            }

            // Retorno la instancia
            return proveedorHandler;
        }

        /// <summary>
        /// <>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool CanHandle(Message message)
        {
            if (message == null || message.From == null)
            {
                throw new ArgumentException("No hay mensaje o no hay quién mande el mensaje");
            }

            if (this.StateForUser.ContainsKey(message.From.Id))
            {
                return true;
            }

            else
            {
                return base.CanHandle(message);
            }
        }

        /// <summary>
        /// Procesa todos los mensajes y retorna true siempre.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado indicando que el mensaje no pudo se procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override void InternalHandle(Message message, out string response)
        {
            if (message == null || message.From == null)
            {
                throw new ArgumentException("No hay mensaje o no hay quién mande el mensaje");
            }

            // Si no se recibió un mensaje antes de este usuario, entonces este es el primer mensaje y el estado del
            // comando es el estado inicial.
            if (!this.stateForUser.ContainsKey(message.From.Id))
            {
                this.stateForUser.Add(message.From.Id, EstadoProveedor.Start);
            }

            EstadoProveedor state = this.StateForUser[message.From.Id];

            // Estado START: Estado de comienzo por defecto
            // Le imprimo el mensaje al proveedor con las opciones disponibles
            if (state == EstadoProveedor.Start)
            {
                // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                this.stateForUser[message.From.Id] = EstadoProveedor.ProveedorMensaje;
                response = PROVEEDOR_MENSAJE;
            }

            // Estado PROVEEDOR_MENSAJE: Le pido al proveedor que quiere hacer
            else if (state == EstadoProveedor.ProveedorMensaje)
            {
                // En caso que se ingresa la opción 1 (Visualizar stock) que me lleve al siguiente estado
                if (message.Text.Equals("1"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoProveedor.VisualizarStock;
                    response = VISUALIZAR_STOCK;
                }

                // En caso que se ingresa la opción de salir que lleve al sigue
                else if (message.Text.Equals("2"))
                {
                    // Llamo al internal cancel
                    this.InternalCancel(message);

                    // Cambio el estado del handler autenticador
                    AutenticadorHandler.YaSeEjecuto = false;

                    // Envío respuesta al usuario
                    response = "Saliendo. Ingrese <<hola>> para volver a iniciar sesión.";
                }

                // En otro caso, que le avise al proveedor que no sabe hacer eso
                else
                {
                    response = OPCION_NO_VALIDA;
                }
            }

            // Estado VISUALIZAR_STOCK: Hago que el programa visualice el stock usando el Intermediario
            // Le pido al proveedor que ingrese un código de producto
            else if (state == EstadoProveedor.VisualizarStock)
            {
                // Tomo el codigo que me ingresa el proveedor y lo casteo a int
                int codigoProducto = Convert.ToInt32(message.Text);

                try
                {
                    // Delego la búsqueda del producto al Intermediario
                    string result = IntermediarioProveedor.VisualizarStock(codigoProducto);

                    // Hago que el bot retorne el mensaje adecuado
                    response = $"{result}\n{PROVEEDOR_MENSAJE}";
                }

                catch (Exception e)
                {
                    response = $"{e.Message}\n{PROVEEDOR_MENSAJE}";
                }

                // Vuelvo al estado inicial
                this.stateForUser[message.From.Id] = EstadoProveedor.ProveedorMensaje;
            }

            // En caso que no se cumplan ninguno de los casos de arriba, que el handler no haga nada
            else
            {
                response = string.Empty;
            }
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial.
        /// </summary>
        protected override void InternalCancel(Message message)
        {
            if (message != null && message.From != null && this.StateForUser.ContainsKey(message.From.Id))
            {
                this.stateForUser.Remove(message.From.Id);
            }
        }

        /// <summary>
        /// Indica los diferentes estados que puede tener el comando AddressHandler.
        /// - Start: El estado inicial del comando. En este estado el comando pide una dirección de origen y pasa al
        /// siguiente estado.
        /// - AddressPrompt: Luego de pedir la dirección. En este estado el comando obtiene las coordenadas de la
        /// dirección y vuelve al estado Start.
        /// </summary>
        public enum EstadoProveedor
        {
            Start,
            ProveedorMensaje,
            VisualizarStock
        }
    }
}