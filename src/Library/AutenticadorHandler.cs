using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Telegram.Bot.Types;
using ProyectoFinal;
using System.Collections;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un "handler" del patrón Chain of Responsibility que implementa el comando "dirección".
    /// </summary>
    public class AutenticadorHandler : BaseHandler
    {
        // Diccionario donde voy guardando los estados
        private Dictionary<long, EstadoAutenticador> stateForUser = new Dictionary<long, EstadoAutenticador>();

        // Especifico los perfiles posibles con sus contraseñas
        private static Dictionary<string, List<string>> Perfiles = new Dictionary<string, List<string>>()
        {{"administrador", new List<string>(){"administrador"}}};

        // Creo un diccionario para vincular las contraseñas con cada uno de los perfiles
        public static Dictionary<string, List<string>> Accesos = new Dictionary<string, List<string>>();

        // Variables estáticas del perfil y la contraseña
        private static string Perfil;
        private static string Contraseña;

        // Variable estática donde guardo el tipo de acceso
        public static bool YaSeEjecuto = false;

        /// <summary>
        /// Metodo para agregar perfiles al diccionario
        /// </summary>
        /// <param name="clave"></param>
        public static void AgregarPerfil(string perfil, string contraseña)
        {
            // En caso que el usuario no exista en el diccionario lo agrego por primera vez
            if (!Perfiles.ContainsKey(perfil))
            {
                // Agrego al diccionario de perfiles un par clave valor con el usuario y la contraseña del perfil
                Perfiles.Add(perfil, new List<string>() { contraseña });
            }

            // En caso que el usuario ya exista lo agrego a la lista
            else
            {
                // Agrego el elemento a la lista
                Perfiles[perfil].Add(contraseña);
            }
        }

        /// <summary>
        /// El estado del comando para un usuario que envía un mensaje. Cuando se comienza a procesar el comando para un
        /// nuevo usuario se agrega a este diccionario y cuando se termina de procesar el comando se remueve.
        /// </summary>
        public IReadOnlyDictionary<long, EstadoAutenticador> StateForUser
        {
            get
            {
                return this.stateForUser;
            }
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ProveedorHandler"/>.
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public AutenticadorHandler(BaseHandler next)
            : base(next)
        {
            this.Keywords = new string[] { "hola" };
        }

        /// <summary>
        /// <>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool CanHandle(Message message)
        {
            // En caso que ya se haya ejecutado el handler, que me diga que no lo puede manejar
            if (YaSeEjecuto)
            {
                return false;
            }

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
                this.stateForUser.Add(message.From.Id, EstadoAutenticador.Start);
            }

            // Defino el estado actual del autenticador
            EstadoAutenticador state = this.StateForUser[message.From.Id];

            if (state == EstadoAutenticador.Start)
            {
                // Doy un mensaje de bienvenida
                response = "Bienvenido! Ingrese el perfil.";

                // Lo llevo al siguiente estado que es pedir el perfil
                this.stateForUser[message.From.Id] = EstadoAutenticador.PedirPerfil;
            }

            // Estado 1: El autenticador pide el perfil
            else if (state == EstadoAutenticador.PedirPerfil)
            {
                // Asigno el valor del primer mensaje a usuario
                Perfil = message.Text;

                // Compruebo que el perfil ingresado sea existente
                if (!Perfiles.ContainsKey(Perfil.ToLower()))
                {
                    response = "El perfil ingresado no es válido.";
                }

                // En caso que el perfil exista, paso al estado de pedir la contraseña
                else
                {
                    // En el estado Start le pide la dirección y pasa al estado UsuarioMensaje
                    this.stateForUser[message.From.Id] = EstadoAutenticador.PedirContraseña;

                    // Pido la contraseña
                    response = "Ingrese su contraseña.";
                }
            }

            // Estado 2: El autenticador pide la contraseña
            else if (state == EstadoAutenticador.PedirContraseña)
            {
                // Asigno el valor del segundo mensaje a la contraseña
                Contraseña = message.Text;

                // Compruebo que la contraseña sea válida
                if (!Perfiles[Perfil].Contains(Contraseña))
                {
                    response = "La contraseña ingresada es incorrecta. Vuelva a ingresar su contraseña.";
                }

                // En caso que la contraseña sea válida, termino la autenticación
                else
                {
                    // Digo que ya se ejecutó el handler de autenticacion
                    YaSeEjecuto = true;

                    // Vuelvo al estado inicial
                    this.stateForUser.Remove(message.From.Id);

                    // Caso I: Se valida el ingreso de Administrador
                    if (Perfil.Equals("administrador"))
                    {
                        // Asigno como siguiente handler el handler de Administrador
                        this.Next = AdministradorHandler.GetInstance();

                        // Le digo al usuario que todo está bien
                        response = "¡Se ha autenticado correctamente! Ingrese la palabra clave <<administrador>> para comenzar la conversación";
                    }

                    // Caso II: Se valida el ingreso de Usuario
                    else if (Perfil.Equals("usuario"))
                    {
                        // Asigno como siguiente handler el handler de Usuario
                        this.Next = UsuarioHandler.GetInstance();

                        // Le digo al usuario que todo está bien
                        response = "¡Se ha autenticado correctamente! Ingrese la palabra clave <<usuario>> para comenzar la conversación";

                        // Configuro variable de Intermediario de Usuario
                        IntermediarioUsuario.ConfigurarUsuario(Accesos[Contraseña]);
                    }

                    // Caso III: Se valida el ingreso de Proveedor
                    else
                    {
                        // Asigno como siguiente handler el handler de Proveedor
                        this.Next = ProveedorHandler.GetInstance();

                        // Le digo al usuario que todo está bien
                        response = "¡Se ha autenticado correctamente! Ingrese la palabra clave <<proveedor>> para comenzar la conversación";

                        // Configuro variable de Intermediario de Usuario
                        IntermediarioProveedor.ConfigurarProveedor(Accesos[Contraseña]);
                    }
                }
            }

            // En caso que no esté en ningún otro estado, que avise
            else
            {
                response = "No sé hacer eso :)";
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
        /// Enum que me guarde los estados de cuando me voy a autenticar
        /// </summary>
        public enum EstadoAutenticador
        {
            Start,
            PedirPerfil,
            PedirContraseña
        }
    }
}