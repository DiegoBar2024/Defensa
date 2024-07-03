using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Telegram.Bot.Types;
using ProyectoFinal;
using System.Collections;
using System.Linq;
using System.Text;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "dirección".
    /// </summary>
    public class UsuarioHandler : BaseHandler
    {
        // Especifico posibles mensajes
        public const string USUARIO_MENSAJE = "Como usuario puedes hacer lo siguiente:\n1) Dar de alta un producto\n2) Visualizar el deposito mas cercano que tenga stock de un producto\n3) Realizar una venta\n4) Salir";
        public const string OPCION_NO_VALIDA = "La opción ingresada no es válida";

        // Diccionario donde voy guardando los estados
        private Dictionary<long, EstadoUsuario> stateForUser = new Dictionary<long, EstadoUsuario>();

        // Creo un diccionario para almacenar elementos de memoria dentro de Alta Producto
        Dictionary<long, EstadoAltaProducto> altaProducto = new Dictionary<long, EstadoAltaProducto>();

        Dictionary<long, EstadoRealizarVenta> realizarVenta = new Dictionary<long, EstadoRealizarVenta>();

        Dictionary<long, EstadoDepositoMasCercano> depositoMasCercano = new Dictionary<long, EstadoDepositoMasCercano>();

        // Inicializo localmente las variables donde guardo los parametros del productos
        List<string> parametros = new List<string>();

        List<List<string>> ventaTotal = new List<List<string>>();

        // Atributo global de la cadena a imprimir
        StringBuilder stringBuilder = new StringBuilder();

        // Creo la instancia UsuarioHandler privada estática para cumplir con Singleton
        private static UsuarioHandler usuarioHandler;

        /// <summary>
        /// El estado del comando para un usuario que envía un mensaje. Cuando se comienza a procesar el comando para un
        /// nuevo usuario se agrega a este diccionario y cuando se termina de procesar el comando se remueve.
        /// </summary>
        public IReadOnlyDictionary<long, EstadoUsuario> StateForUser
        {
            get
            {
                return this.stateForUser;
            }
        }

        // Creo el constructor privado para UsuarioHandler por Singleton
        private UsuarioHandler()
        {
            this.Keywords = new string[] {"usuario"};
        }

        // Creo un método que me permita obtener la misma instancia usando Singleton
        public static UsuarioHandler GetInstance()
        {
            // En caso que no exista ningún objeto, le digo que lo cree
            if (UsuarioHandler.usuarioHandler == null)
            {
                // Instancio un nuevo objeto llamando al constructor privado
                usuarioHandler = new UsuarioHandler();
            }

            // Retorno la instancia
            return usuarioHandler;
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
                this.stateForUser.Add(message.From.Id, EstadoUsuario.Start);
                response = USUARIO_MENSAJE;
            }

            EstadoUsuario state = this.StateForUser[message.From.Id];

            // Estado START: Estado de comienzo por defecto
            // Le imprimo el mensaje al usuario con las opciones disponibles
            if (state == EstadoUsuario.Start)
            {
                // En el estado Start le pide la dirección y pasa al estado UsuarioMensaje
                this.stateForUser[message.From.Id] = EstadoUsuario.UsuarioMensaje;
                response = USUARIO_MENSAJE;
            }

            // Estado PROVEEDOR_MENSAJE: Le pido al proveedor que quiere hacer
            else if (state == EstadoUsuario.UsuarioMensaje)
            {
                // En caso que se ingresa la opción 1 (Dar de alta un producto) que me lleve al siguiente estado
                if (message.Text.Equals("1"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoUsuario.AltaProducto;

                    // Inicio el diccionario con el primer estado que es el de pedir nombre
                    altaProducto.Add(message.From.Id, EstadoAltaProducto.Nombre);

                    // Pido nombre
                    response = "Dame el nombre del producto";

                    // Inicio el constructor de cadenas
                    stringBuilder.AppendLine("Producto dado de alta correctamente. Características del producto:\n");
                }

                // En caso que se ingresa opcion 2 (Deposito mas cercano) que me busque el deposito mas cercano
                else if (message.Text.Equals("2"))
                {
                    // Seteo el estado del Usuario en el Deposito Mas Cercano
                    this.stateForUser[message.From.Id] = EstadoUsuario.DepositoMasCercano;

                    // Agrego el estado interno inicial
                    depositoMasCercano.Add(message.From.Id, EstadoDepositoMasCercano.Codigo);

                    // Aviso al usuario que me de el codigo mas cercano
                    response = "Dame el codigo del producto";
                }

                // En caso que se ingresa la opcion 2 (Realizar una venta) que me lleve al siguiente estado
                else if (message.Text.Equals("3"))
                {
                    // Seteo el estado del Usuario en el Realizar Venta
                    this.stateForUser[message.From.Id] = EstadoUsuario.RealizarVenta;

                    // Agrego el estado interno inicial
                    realizarVenta.Add(message.From.Id, EstadoRealizarVenta.Codigo);

                    // Aviso al usuario que eligió realizar una venta
                    response = "Dame el codigo del producto";
                }

                // En caso que se ingresa la opción de salir que lleve al sigue
                else if (message.Text.Equals("4"))
                {
                    // Llamo al internal cancel
                    this.InternalCancel(message);

                    // Cambio el estado del handler autenticador
                    AutenticadorHandler.YaSeEjecuto = false;

                    // Envío respuesta al usuario
                    response = "Saliendo. Ingrese <<hola>> para volver a iniciar sesión.";
                }

                // En otro caso, que le avise al usuario que no sabe hacer eso
                else
                {
                    response = OPCION_NO_VALIDA;
                }
            }

            // Estado ALTA PRODUCTO: Hago que el programa de de alta un producto delegando al Intermediario
            // Le pido al usuario que ingrese los diferentes datos de un producto
            else if (state == EstadoUsuario.AltaProducto)
            {
                // Armo el estado interior en AltaProducto
                EstadoAltaProducto estadoAltaProducto = altaProducto[message.From.Id];

                // Ingreso de nombre
                if (estadoAltaProducto == EstadoAltaProducto.Nombre)
                {
                    // Pido precio
                    response = "Dame el precio del producto";

                    // Registro el nombre del producto
                    parametros.Add(message.Text);

                    stringBuilder.AppendLine($"Nombre: {message.Text}");

                    // Paso al siguiente estado
                    altaProducto[message.From.Id] = EstadoAltaProducto.Precio;
                }

                // Ingreso de precio
                else if (estadoAltaProducto == EstadoAltaProducto.Precio)
                {
                    // Pido el codigo
                    response = "Dame el codigo del producto";

                    // Registro el precio
                    parametros.Add(message.Text);

                    stringBuilder.AppendLine($"Precio: {message.Text}");

                    // Paso al siguiente estado
                    altaProducto[message.From.Id] = EstadoAltaProducto.Codigo;
                }

                // Ingreso de codigo
                else if (estadoAltaProducto == EstadoAltaProducto.Codigo)
                {
                    // Pido la marca
                    response = "Dame la marca del producto";

                    // Registro el codigo
                    parametros.Add(message.Text);
                    
                    stringBuilder.AppendLine($"Codigo: {message.Text}");

                    // Paso al siguiente estado
                    altaProducto[message.From.Id] = EstadoAltaProducto.Marca;
                }

                // Ingreso la marca
                else if (estadoAltaProducto == EstadoAltaProducto.Marca)
                {
                    // Pido las categorias
                    // El usuario me las va a dar como palabras separadas por coma
                    response = "Dame las categorias del producto (<<categoria1>>, <<categoria2>>, ...)";

                    // Registro el nombre de la marca
                    parametros.Add(message.Text);

                    stringBuilder.AppendLine($"Marca: {message.Text}");

                    // Paso al siguiente estado
                    altaProducto[message.From.Id] = EstadoAltaProducto.Categorias;
                }

                // Ingreso las categorias
                else if (estadoAltaProducto == EstadoAltaProducto.Categorias)
                {
                    // Pido la seccion
                    response = "Dame el nombre de la seccion donde vas a dar de alta";

                    // Registro el nombre del producto
                    parametros.Add(message.Text);

                    stringBuilder.AppendLine($"Categorias: {message.Text}");

                    // Paso al siguiente estado
                    altaProducto[message.From.Id] = EstadoAltaProducto.Seccion;
                }

                // Ingreso el nombre de la seccion
                else if (estadoAltaProducto == EstadoAltaProducto.Seccion)
                {
                    // Pido el deposito
                    response = "Dame el nombre del deposito donde vas a dar de alta";

                    // Registro el nombre del producto
                    parametros.Add(message.Text);
                    
                    stringBuilder.AppendLine($"Seccion: {message.Text}");

                    // Paso al siguiente estado
                    altaProducto[message.From.Id] = EstadoAltaProducto.Deposito;
                }

                // Ingreso el nombre del deposito
                else if (estadoAltaProducto == EstadoAltaProducto.Deposito)
                {
                    // Pido el stock
                    response = "Dame el stock de producto que vas a dar de alta";

                    // Registro el nombre del deposito
                    parametros.Add(message.Text);
                    
                    stringBuilder.AppendLine($"Deposito: {message.Text}");

                    // Paso al siguiente estado
                    altaProducto[message.From.Id] = EstadoAltaProducto.Stock;
                }

                // Ingreso el valor del stock
                else if (estadoAltaProducto == EstadoAltaProducto.Stock)
                {
                    // Registro el valor de stock a ingresar
                    parametros.Add(message.Text);

                    try
                    {
                        // Delego la funcion de dar de alta al intermediario
                        IntermediarioUsuario.AltaProducto(parametros[0], Convert.ToDouble(parametros[1]), 
                        Convert.ToInt32(parametros[2]), parametros[3], parametros[4].Split(", ").ToList(), parametros[5], parametros[6], 
                        Convert.ToInt32(parametros[7]));

                        // Retorno una respuesta que va a ser el ticket de producto
                        response = $"{stringBuilder}\n{USUARIO_MENSAJE}";

                        // Limpio la cadena StringBuilder
                        stringBuilder.Clear();
                    }

                    catch (Exception e)
                    {
                        response = $"{e.Message}\n{USUARIO_MENSAJE}";
                    }

                    // Borro el diccionario que ya tengo
                    altaProducto.Clear();

                    // Borro todos los parámetros ingresados
                    parametros.Clear();

                    // Limpio el stringBuilder
                    stringBuilder.Clear();

                    // Vuelvo al estado inicial
                    this.stateForUser[message.From.Id] = EstadoUsuario.UsuarioMensaje;
                }

                else
                {
                    response = "No se hacer eso :)";
                }
            }

            // Estado de Deposito Mas Cercano
            else if (state == EstadoUsuario.DepositoMasCercano)
            {
                // Agarro el estado de deposito más cercano
                EstadoDepositoMasCercano estadoDepositoMasCercano = depositoMasCercano[message.From.Id];

                // Pido el codigo de producto al usuario
                if (estadoDepositoMasCercano == EstadoDepositoMasCercano.Codigo)
                {
                    parametros.Add(message.Text);

                    // Pido la ubicación al usuario
                    response = "Dame tu ubicacion";

                    // Siguiente estado
                    depositoMasCercano[message.From.Id] = EstadoDepositoMasCercano.Ubicacion;
                }

                // Pido la ubicacion al usuario
                else if (estadoDepositoMasCercano == EstadoDepositoMasCercano.Ubicacion)
                {
                    parametros.Add(message.Text);

                    try
                    {
                        // Delego la funcion de hallar el deposito más cercano a IntermediarioUsuario
                        string nombreDeposito = IntermediarioUsuario.DepositoMasCercano(Convert.ToInt32(parametros[0]),
                        parametros[1]);

                        // Retorno una respuesta que va a ser el ticket de producto
                        response = $"El depósito más cercano con stock disponible es {nombreDeposito}\n{USUARIO_MENSAJE}";
                    }

                    catch (Exception e)
                    {
                        response = $"{e.Message}\n{USUARIO_MENSAJE}";
                    }

                    // Borro el diccionario que ya tengo
                    depositoMasCercano.Clear();

                    // Borro la lista de parametros
                    parametros.Clear();

                    // Vuelvo al estado inicial
                    this.stateForUser[message.From.Id] = EstadoUsuario.UsuarioMensaje;
                }

                else
                {
                    response = string.Empty;
                }
            }

            else if (state == EstadoUsuario.RealizarVenta)
            {
                EstadoRealizarVenta estadoRealizarVenta = realizarVenta[message.From.Id];

                if (estadoRealizarVenta == EstadoRealizarVenta.Codigo)
                {
                    parametros.Add(message.Text);

                    response = "Dame el stock del producto a vender";

                    realizarVenta[message.From.Id] = EstadoRealizarVenta.Stock;
                }

                else if (estadoRealizarVenta == EstadoRealizarVenta.Stock)
                {
                    parametros.Add(message.Text);

                    response = "Dame la sección de donde vas a extraer el producto a vender";

                    realizarVenta[message.From.Id] = EstadoRealizarVenta.Seccion;
                }

                else if (estadoRealizarVenta == EstadoRealizarVenta.Seccion)
                {
                    parametros.Add(message.Text);

                    response = "Dame el deposito de donde vas a extraer el producto a vender";

                    realizarVenta[message.From.Id] = EstadoRealizarVenta.Deposito;
                }

                else if (estadoRealizarVenta == EstadoRealizarVenta.Deposito)
                {
                    parametros.Add(message.Text);

                    ventaTotal.Add(new List<string>(){parametros[0], parametros[1], parametros[2], parametros[3]});

                    parametros.Clear();

                    realizarVenta[message.From.Id] = EstadoRealizarVenta.Preguntando;

                    response = "¿Quiere hacer alguna otra venta?\n1) Sí\n2) No";
                }

                else if (estadoRealizarVenta == EstadoRealizarVenta.Preguntando)
                {
                    if (message.Text == "1")
                    {
                        realizarVenta[message.From.Id] = EstadoRealizarVenta.Codigo;

                        response = "Dame el codigo del producto";
                    }

                    else if (message.Text == "2")
                    {
                        try
                        {
                            string respuesta = IntermediarioUsuario.RealizarVenta(ventaTotal);

                            if (string.IsNullOrEmpty(respuesta))
                            {
                                response = $"Venta realizada correctamente\n{USUARIO_MENSAJE}";
                            }

                            else
                            {
                                response = $"{respuesta}{USUARIO_MENSAJE}";
                            }
                        }

                        catch (Exception e)
                        {
                            response = $"{e.Message}\n{USUARIO_MENSAJE}";
                        }

                        ventaTotal.Clear();

                        // Borro el diccionario que ya tengo
                        realizarVenta.Clear();

                        // Vuelvo al estado inicial
                        this.stateForUser[message.From.Id] = EstadoUsuario.UsuarioMensaje;
                    }

                    else
                    {
                        response = "No se hacer eso :)";
                    }
                }

                else
                {
                    response = string.Empty;
                }
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
        /// Enum que me guarde los estados de Usuarios
        /// </summary>
        public enum EstadoUsuario
        {
            Start,
            UsuarioMensaje,
            DepositoMasCercano,
            AltaProducto,
            RealizarVenta
        }

        /// <summary>
        /// Hago un enum que me guarde los estados de Deposito Mas Cercano
        /// </summary>
        public enum EstadoDepositoMasCercano
        {
            Codigo,
            Ubicacion
        }

        /// <summary>
        ///  Hago un enum que me guarde los estados de Alta Producto
        /// </summary>
        public enum EstadoAltaProducto
        {
            Nombre,
            Precio,
            Codigo,
            Marca,
            Categorias,
            Seccion,
            Deposito,
            Stock,
        }

        /// <summary>
        /// Enum que me guarde los estados de realizar ventas
        /// </summary>
        public enum EstadoRealizarVenta
        {
            Codigo,
            Stock,
            Seccion,
            Deposito,
            Preguntando
        }
    }
}