using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Telegram.Bot.Types;
using ProyectoFinal;
using System.Collections;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Telegram.Bots.Requests;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "dirección".
    /// </summary>
    public class AdministradorHandler : BaseHandler
    {
        // Especifico posibles mensajes
        public const string ADMINISTRADOR_MENSAJE = "Como administrador podes hacer:\n1) Crear deposito\n2) Dar de alta usuario\n3) Definir secciones\n4) Dar de alta proveedor\n5) Crear una nueva categoría\n6) Registrar una compra\n7) Visualizar las ventas en un día\n8) Salir";

        // Creo la instancia AdministradorHandler privada estática para cumplir con Singleton
        private static AdministradorHandler administradorHandler;

        // Diccionario donde voy guardando los estados
        public Dictionary<long, EstadoAdministrador> stateForUser = new Dictionary<long, EstadoAdministrador>();

        Dictionary<long, EstadoCrearDeposito> crearDeposito = new Dictionary<long, EstadoCrearDeposito>();

        Dictionary<long, EstadoAltaUsuario> altaUsuario = new Dictionary<long, EstadoAltaUsuario>();

        Dictionary<long, EstadoCrearSeccion> crearSeccion = new Dictionary<long, EstadoCrearSeccion>();

        Dictionary<long, EstadoCrearCategorias> crearCategorias = new Dictionary<long, EstadoCrearCategorias>();

        Dictionary<long, EstadoRegistrarCompra> registrarCompra = new Dictionary<long, EstadoRegistrarCompra>();

        Dictionary<long, EstadoVisualizarVentas> visualizarVentas = new Dictionary<long, EstadoVisualizarVentas>();

        StringBuilder stringBuilder = new StringBuilder();

        // Inicializo localmente las variables donde guardo los parametros del productos
        List<string> parametros = new List<string>();

        // Creo un diccionario para almacenar elementos de memoria dentro de Realizar Venta
        // Dictionary<long, EstadoRealizarVenta> realizarVenta = new Dictionary<long, EstadoRealizarVenta>();

        /// <summary>
        /// El estado del comando para un usuario que envía un mensaje. Cuando se comienza a procesar el comando para un
        /// nuevo usuario se agrega a este diccionario y cuando se termina de procesar el comando se remueve.
        /// </summary>
        public IReadOnlyDictionary<long, EstadoAdministrador> StateForUser
        {
            get
            {
                return this.stateForUser;
            }
        }

        /// <summary>
        /// Creo el constructor privado para AdministradorHandler por Singleton
        /// </summary>
        private AdministradorHandler()
        {
            this.Keywords = new string[] {"administrador"};
        }
        /// <summary>
        /// Creo un método que me permita obtener la misma instancia usando Singleton
        /// </summary>
        /// <returns>la instancia</returns>
        public static AdministradorHandler GetInstance()
        {
            // En caso que no exista ningún objeto, le digo que lo cree
            if (AdministradorHandler.administradorHandler == null)
            {
                // Instancio un nuevo objeto llamando al constructor privado
                administradorHandler = new AdministradorHandler();
            }

            // Retorno la instancia
            return administradorHandler;
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
                this.stateForUser.Add(message.From.Id, EstadoAdministrador.Start);
            }

            EstadoAdministrador state = this.StateForUser[message.From.Id];

            // Estado START: Estado de comienzo por defecto
            // Le imprimo el mensaje al usuario con las opciones disponibles
            if (state == EstadoAdministrador.Start)
            {
                // En el estado Start le pide la dirección y pasa al estado UsuarioMensaje
                this.stateForUser[message.From.Id] = EstadoAdministrador.AdministradorMensaje;
                response = ADMINISTRADOR_MENSAJE;
            }

            // Estado Administrador_Mensaje: Le pido al proveedor que quiere hacer
            else if (state == EstadoAdministrador.AdministradorMensaje)
            {
                // En caso que se ingresa la opción 1 () que me lleve al siguiente estado
                if (message.Text.Equals("1"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoAdministrador.CrearDeposito;

                    crearDeposito[message.From.Id] = EstadoCrearDeposito.Nombre;

                    stringBuilder.AppendLine("Depósito creado correctamente. Las características del depósito son: ");

                    // Pido nombre
                    response = "Dame el nombre del deposito";
                }

                // En caso que se ingresa la opcion 2 (Dar de alta usuario) que me lleve al siguiente estado
                else if (message.Text.Equals("2"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoAdministrador.DarAltaUsuario;

                    altaUsuario[message.From.Id] = EstadoAltaUsuario.Nombre;

                    // Aviso al usuario que eligió realizar una venta
                    response = "Dame el nombre del usuario. Ésta será la contraseña para su perfil.";
                }
                
                // En caso que se ingresa la opcion 3 (Definir secciones dentro de un deposito) que me lleve al siguiente estado
                else if (message.Text.Equals("3"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoAdministrador.CrearSeccion;

                    crearSeccion[message.From.Id] = EstadoCrearSeccion.Nombre;

                    // Aviso al usuario que eligió realizar una venta
                    response = "Dame el nombre de la seccion";
                }

                // En caso que se ingresa la opcion 4 (Dar de alta un proveedor) que me lleve al siguiente estado
                else if (message.Text.Equals("4"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoAdministrador.DarAltaProveedor;

                    // Aviso al usuario que eligió realizar una venta
                    response = "Dame el nombre del proveedor. Ésta será la contraseña para su perfil.";
                }

                // En caso que se ingresa la opcion 5 (Registrar una compra) que me lleve al siguiente estado
                else if (message.Text.Equals("5"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoAdministrador.CrearCategorias;

                    crearCategorias[message.From.Id] = EstadoCrearCategorias.Categoria;

                    // Aviso al usuario que eligió realizar una venta
                    response = "Dame la categoría que quieras crear";
                }

                // En caso que se ingresa la opcion 5 (Registrar una compra) que me lleve al siguiente estado
                else if (message.Text.Equals("6"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoAdministrador.RegistrarCompra;

                    registrarCompra[message.From.Id] = EstadoRegistrarCompra.NombreDeposito;

                    // Aviso al usuario que eligió realizar una venta
                    response = "Dame el deposito en donde vas a registrar la compra";
                }

                // En caso que se ingresa la opcion 6 (Visualizar ventas en un dia) que me lleve al siguiente estado
                else if (message.Text.Equals("7"))
                {
                    // En el estado Start le pide la dirección y pasa al estado ProveedorMensaje
                    this.stateForUser[message.From.Id] = EstadoAdministrador.VisualizarVentas;

                    visualizarVentas[message.From.Id] = EstadoVisualizarVentas.Dia;

                    // Aviso al usuario que eligió realizar una venta
                    response = "Dame el dia en el cual querés visualizar las ventas";
                }

                // En caso que se ingresa la opción de salir que lleve al sigue
                else if (message.Text.Equals("8"))
                {
                    // Llamo al internal cancel
                    this.InternalCancel(message);

                    // Cambio el estado del handler autenticador
                    AutenticadorHandler.YaSeEjecuto = false;

                    // Envío respuesta al usuario
                    response = "Saliendo. Ingrese <<hola>> para volver a iniciar sesión.";
                }

                // En otro caso, que le avise al administrador que no sabe hacer eso
                else
                {
                    response = "La opcion no es valida" + "\n" + ADMINISTRADOR_MENSAJE;
                }
            }

            else if (state == EstadoAdministrador.CrearDeposito)
            {
                EstadoCrearDeposito estadoCrearDeposito = crearDeposito[message.From.Id];

                if (estadoCrearDeposito == EstadoCrearDeposito.Nombre)
                {
                    parametros.Add(message.Text);

                    response = "Dame la ubicación del deposito";

                    stringBuilder.AppendLine($" Nombre: {message.Text}");

                    crearDeposito[message.From.Id] = EstadoCrearDeposito.Ubicacion;
                }

                else if (estadoCrearDeposito == EstadoCrearDeposito.Ubicacion)
                {
                    parametros.Add(message.Text);

                    response = "Dame la capacidad del deposito";

                    stringBuilder.AppendLine($" Ubicacion: {message.Text}");

                    crearDeposito[message.From.Id] = EstadoCrearDeposito.Capacidad;
                }

                else if (estadoCrearDeposito == EstadoCrearDeposito.Capacidad)
                {
                    parametros.Add(message.Text);

                    stringBuilder.AppendLine($" Capacidad: {message.Text}");

                    try
                    {
                        IntermediarioAdministrador.CrearDeposito(parametros[0], parametros[1], Convert.ToInt32(parametros[2]));

                        // Retorno una respuesta que va a ser el ticket de producto
                        response = $"{stringBuilder}\n{ADMINISTRADOR_MENSAJE}";
                    }

                    catch (Exception e)
                    {
                        response = $"{e.Message}\n{ADMINISTRADOR_MENSAJE}";
                    }

                    // Limpio la cadena StringBuilder
                    stringBuilder.Clear();

                    // Borro el diccionario que ya tengo
                    crearDeposito.Clear();

                    // Borro todos los parámetros ingresados
                    parametros.Clear();

                    // Vuelvo al estado inicial
                    this.stateForUser[message.From.Id] = EstadoAdministrador.AdministradorMensaje;
                }

                else
                {
                    response = "No se hacer eso :)";
                }
            }

            else if (state == EstadoAdministrador.DarAltaUsuario)
            {
                EstadoAltaUsuario estadoAltaUsuario = altaUsuario[message.From.Id];

                if (estadoAltaUsuario == EstadoAltaUsuario.Nombre)
                {
                    parametros.Add(message.Text);

                    response = "¿El usuario tiene permisos de administrador? (true/false)";

                    altaUsuario[message.From.Id] = EstadoAltaUsuario.Permiso;
                }

                else if (estadoAltaUsuario == EstadoAltaUsuario.Permiso)
                {
                    parametros.Add(message.Text);

                    // Lo que hago ahora es llamar al intermediario de administrador para que me devuelva una cadena con todos los depositos creados
                    string depositosExistentes = IntermediarioAdministrador.NombresDepositos();

                    // Pido los depositos a los que el usuario va a poder tener acceso
                    response = $"{depositosExistentes}\nDame los depósitos a los que puede tener acceso el usuario: <<deposito1>>, <<deposito2>> ... <<depositoN>>";

                    // Siguiente estado
                    altaUsuario[message.From.Id] = EstadoAltaUsuario.DepositosUsuario;
                }

                else if (estadoAltaUsuario == EstadoAltaUsuario.DepositosUsuario)
                {
                    // Agrego el texto de mensajes a la lista de parámetros
                    parametros.Add(message.Text);

                    try
                    {
                        IntermediarioAdministrador.AltaUsuario(parametros[0], parametros[1], parametros[2].Split(", ").ToList());

                        response = $"Usuario dado de alta correctamente.\n{ADMINISTRADOR_MENSAJE}";

                        // Le digo al handler autenticador que cree el acceso para el nuevo usuario
                        AutenticadorHandler.AgregarPerfil("usuario", parametros[0]);

                        // Asocio los datos de la creación del perfil con la contraseña
                        AutenticadorHandler.Accesos.Add(parametros[0], new List<string>() {parametros[0], parametros[1], parametros[2]});
                    }

                    catch (Exception e)
                    {
                        response = $"{e.Message}\n{ADMINISTRADOR_MENSAJE}";
                    }

                    // Borro el diccionario que ya tengo
                    crearDeposito.Clear();

                    // Borro todos los parámetros ingresados
                    parametros.Clear();

                    // Vuelvo al estado inicial
                    this.stateForUser[message.From.Id] = EstadoAdministrador.AdministradorMensaje;
                }

                else
                {
                    response = "No se hacer eso :)";
                }
            }

            else if (state == EstadoAdministrador.CrearSeccion)
            {
                EstadoCrearSeccion estadoCrearSeccion = crearSeccion[message.From.Id];

                if (estadoCrearSeccion == EstadoCrearSeccion.Nombre)
                {
                    parametros.Add(message.Text);

                    response = "Dame la capacidad de la seccion";

                    crearSeccion[message.From.Id] = EstadoCrearSeccion.Capacidad;
                }

                else if (estadoCrearSeccion == EstadoCrearSeccion.Capacidad)
                {
                    parametros.Add(message.Text);

                    response = "Dame el nombre del depósito al que pertenece la seccion";

                    crearSeccion[message.From.Id] = EstadoCrearSeccion.NombreDeposito;
                }

                else if (estadoCrearSeccion == EstadoCrearSeccion.NombreDeposito)
                {
                    parametros.Add(message.Text);

                    try
                    {
                        IntermediarioAdministrador.CrearSeccion(parametros[0], Convert.ToInt32(parametros[1]), parametros[2]);

                        response = $"Seccion creada correctamente.\n{ADMINISTRADOR_MENSAJE}";
                    }

                    catch (Exception e)
                    {
                        response = $"{e.Message}\n{ADMINISTRADOR_MENSAJE}";
                    }

                    // Borro el diccionario que ya tengo
                    crearDeposito.Clear();

                    // Borro todos los parámetros ingresados
                    parametros.Clear();

                    // Vuelvo al estado inicial
                    this.stateForUser[message.From.Id] = EstadoAdministrador.AdministradorMensaje;
                }

                else
                {
                    response = "No se hacer eso :)";
                }
            }

            else if (state == EstadoAdministrador.DarAltaProveedor)
            {
                try
                {
                    IntermediarioAdministrador.AltaProveedor(message.Text);

                    response = $"Proveedor dado de alta correctamente.\n{ADMINISTRADOR_MENSAJE}";

                    // Le digo al handler autenticador que cree el acceso para el nuevo usuario
                    AutenticadorHandler.AgregarPerfil("proveedor", message.Text);

                    // Asocio los datos de la creación del perfil con la contraseña
                    AutenticadorHandler.Accesos.Add(message.Text, new List<string>() {message.Text});
                }

                catch (Exception e)
                {
                    response = $"{e.Message}\n{ADMINISTRADOR_MENSAJE}";
                }

                // Borro el diccionario que ya tengo
                crearDeposito.Clear();

                // Borro todos los parámetros ingresados
                parametros.Clear();

                // Vuelvo al estado inicial
                this.stateForUser[message.From.Id] = EstadoAdministrador.AdministradorMensaje;
            }

            else if (state == EstadoAdministrador.CrearCategorias)
            {
                EstadoCrearCategorias estadoCrearCategorias = crearCategorias[message.From.Id];

                if (estadoCrearCategorias == EstadoCrearCategorias.Categoria)
                {
                    parametros.Add(message.Text);

                    response = "Dame los codigos de los productos para aplicar la categorias: <<codigo1>>, <<codigo2>>, ..., <<codigoN>>";

                    crearCategorias[message.From.Id] = EstadoCrearCategorias.PidiendoCodigos;
                }

                else if (estadoCrearCategorias == EstadoCrearCategorias.PidiendoCodigos)
                {
                    parametros.Add(message.Text);

                    try
                    {
                        IntermediarioAdministrador.CrearCategoria(parametros[0], parametros[1].Split(", ").Select(int.Parse).ToList());

                        response = $"Categoría creada correctamente.\n{ADMINISTRADOR_MENSAJE}";
                    }

                    catch (Exception e)
                    {
                        response = $"{e.Message}{ADMINISTRADOR_MENSAJE}";
                    }

                    // Borro el diccionario que ya tengo
                    crearDeposito.Clear();

                    // Borro todos los parámetros ingresados
                    parametros.Clear();

                    // Vuelvo al estado inicial
                    this.stateForUser[message.From.Id] = EstadoAdministrador.AdministradorMensaje;
                }

                else
                {
                    response = "No se hacer eso :)";
                }
            }

            else if (state == EstadoAdministrador.RegistrarCompra)
            {
                EstadoRegistrarCompra estadoRegistrarCompra = registrarCompra[message.From.Id];

                if (estadoRegistrarCompra == EstadoRegistrarCompra.NombreDeposito)
                {
                    parametros.Add(message.Text);

                    response = "Dame la cantidad de stock comprado de dicho producto";

                    registrarCompra[message.From.Id] = EstadoRegistrarCompra.StockComprado;
                }

                else if (estadoRegistrarCompra == EstadoRegistrarCompra.StockComprado)
                {
                    parametros.Add(message.Text);

                    response = "Dame el codigo del producto a comprar";

                    registrarCompra[message.From.Id] = EstadoRegistrarCompra.CodigoProducto;
                }

                else if (estadoRegistrarCompra == EstadoRegistrarCompra.CodigoProducto)
                {
                    parametros.Add(message.Text);

                    try
                    {
                        IntermediarioAdministrador.AumentarStock(parametros[0], Convert.ToInt32(parametros[1]), Convert.ToInt32(parametros[2]));

                        response = $"Se ha registrado la compra correctamente.\n{ADMINISTRADOR_MENSAJE}";
                    }

                    catch (Exception e)
                    {
                        response = $"{e.Message}\n{ADMINISTRADOR_MENSAJE}";
                    }

                    // Borro el diccionario que ya tengo
                    crearDeposito.Clear();

                    // Borro todos los parámetros ingresados
                    parametros.Clear();

                    // Vuelvo al estado inicial
                    this.stateForUser[message.From.Id] = EstadoAdministrador.AdministradorMensaje;
                }

                else
                {
                    response = "No se hacer eso :)";
                }
            }

            else if (state == EstadoAdministrador.VisualizarVentas)
            {
                EstadoVisualizarVentas estadoVisualizarVentas = visualizarVentas[message.From.Id];

                if (estadoVisualizarVentas == EstadoVisualizarVentas.Dia)
                {
                    parametros.Add(message.Text);

                    response = "Dame el mes en el cual querés visualizar las ventas";

                    visualizarVentas[message.From.Id] = EstadoVisualizarVentas.Mes;
                }

                else if (estadoVisualizarVentas == EstadoVisualizarVentas.Mes)
                {
                    parametros.Add(message.Text);

                    response = "Dame el año en el cual querés visualizar las ventas";

                    visualizarVentas[message.From.Id] = EstadoVisualizarVentas.Año;
                }

                else if (estadoVisualizarVentas == EstadoVisualizarVentas.Año)
                {
                    parametros.Add(message.Text);

                    try
                    {
                        List<VentaTotal> result = IntermediarioAdministrador.VentasEnDia(new DateTime(Convert.ToInt32(parametros[2]), 
                        Convert.ToInt32(parametros[1]), Convert.ToInt32(parametros[0])));

                        StringBuilder cadena = new StringBuilder();

                        // Creo el diccionario que asocie productos con cantidades vendidas
                        Dictionary<int, int> ventasPorCodigo = new Dictionary<int, int>();

                        // Creo la variable donde guardo la cadena a imprimir
                        StringBuilder stringBuilder = new StringBuilder($"Para la fecha {parametros[0]}/{parametros[1]}/{parametros[2]} se tienen las siguentes ventas:\n");
                        
                        // Itero venta por venta en la lista de ventas
                        foreach (VentaTotal ventaTotal in result)
                        {
                            // Itero venta por venta individual dentro de la venta total
                            foreach (VentaIndividual ventaIndividual in ventaTotal.GetVentasIndividuales)
                            {
                                // En caso que el codigo del producto vendido ya exista en el diccionario, que me actualice la suma
                                if (ventasPorCodigo.ContainsKey(ventaIndividual.GetCodigoProducto))
                                {
                                    ventasPorCodigo[ventaIndividual.GetCodigoProducto] += ventaIndividual.GetCantidad;
                                }

                                // En otro caso, que me cree un nuevo par clave valor
                                else
                                {
                                    ventasPorCodigo.Add(ventaIndividual.GetCodigoProducto, ventaIndividual.GetCantidad);
                                }
                            }
                        }

                        // Itero codigo por codigo para cada uno de los productos vendidos
                        foreach (int codigo in ventasPorCodigo.Keys)
                        {
                            // Armo la línea por codigo
                            stringBuilder.AppendLine($"Se vendieron {ventasPorCodigo[codigo]} unidades del producto de codigo {codigo}");
                        }

                        response = $"Venta visualizada correctamente.\n{stringBuilder}\n{ADMINISTRADOR_MENSAJE}";

                        // Limpio el stringBuilder para poder volverlo a usar
                        stringBuilder.Clear();
                    }

                    catch (Exception e)
                    {
                        response = $"{e.Message}\n{ADMINISTRADOR_MENSAJE}";
                    }

                    // Borro el diccionario que ya tengo
                    crearDeposito.Clear();

                    // Borro todos los parámetros ingresados
                    parametros.Clear();

                    // Vuelvo al estado inicial
                    this.stateForUser[message.From.Id] = EstadoAdministrador.AdministradorMensaje;
                }

                else
                {
                    response = "No se hacer eso :)";
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
        /// Enum que me guarde los estados del administrador
        /// </summary>
        public enum EstadoAdministrador
        {
            Start,
            AdministradorMensaje,
            CrearDeposito,
            DarAltaUsuario,
            DarAltaProveedor,
            CrearSeccion,
            CrearCategorias,
            RegistrarCompra,
            VisualizarVentas,
        }

        /// <summary>
        /// Hago un enum que me guarde los estados de Alta Producto
        /// </summary>
        public enum EstadoCrearDeposito
        {
            Nombre,
            Ubicacion,
            Capacidad
        }
        /// <summary>
        /// Enum que me guarde los estados del Usuario
        /// </summary>
        public enum EstadoAltaUsuario
        {
            Nombre,
            Permiso,
            DepositosUsuario
        }

        /// <summary>
        /// Enum que me guarde los estados de crear una sección
        /// </summary>
        public enum EstadoCrearSeccion
        {
            Nombre,
            Capacidad,
            NombreDeposito
        }

        /// <summary>
        /// Enum que me guarde los estados de crear categorías
        /// </summary>

        public enum EstadoCrearCategorias
        {
            Categoria,
            PidiendoCodigos
        }

        /// <summary>
        /// Enum que me guarde los estados de cuando registro una compra
        /// </summary>
        public enum EstadoRegistrarCompra
        {
            NombreDeposito,
            StockComprado,
            CodigoProducto
        }

        /// <summary>
        /// Enum que me guarde los estados para cuando quiero visualizar la venta en un día dado
        /// </summary>
        public enum EstadoVisualizarVentas
        {
            Dia,
            Mes,
            Año
        }
    }
}