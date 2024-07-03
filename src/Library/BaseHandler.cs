using System;
using System.Linq;
using ProyectoFinal;
using Telegram.Bot.Types;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Clase base para implementar el patrón
    /// <a href="https://refactoring.guru/design-patterns/chain-of-responsibility">Chain of Responsibility</a>. En ese
    /// patrón se pasa un mensaje a través de una cadena de "handlers" que pueden procesar o no el mensaje. Cada
    /// "handler" decide si procesa el mensaje, o si se lo pasa al siguiente. Esta clase base implmementa la
    /// responsabilidad de recibir el mensaje y pasarlo al siguiente "handler" en caso que el mensaje no sea procesado.
    /// La responsabilidad de decidir si el mensaje se procesa o no, y de procesarlo, se delega a las clases sucesoras
    /// de esta clase base.
    /// <remarks>
    /// Esta clase se crea en base al <a href="https://github.com/ucudal/PII_Principios_Patrones/blob/master/OCP.md">
    /// principio abierto/cerrado</a> ya que la lógica de funcionamiento de la cadena de responsabilidad no puede ser
    /// modificada por las clases sucesoras -la clase es cerrada a la modificación-, por un lado, pero el procesamiento
    /// concreto de los mensajes se realiza en las clases sucesoras –la clase está abierta a la modificación–.
    /// </remarks>
    /// <remarks>
    /// La cadena de responsabilidad implementada con esta clase y sus sucesoras utiliza el patrón GRASP
    /// <a href="https://github.com/ucudal/PII_Principios_Patrones/blob/master/Polymorphism.md">polimorfismo</a> pues
    /// evita preguntar por la clase capaz de procesar un mensaje, asignando la reponsabilidad de procesar el mensaje
    /// en operaciones polimórficas –métodos virtuales– cuya implementación concreta se delega a los sucesores.
    /// </remarks>
    /// <remarks>
    /// Esta clase usa el patrón <a href="https://refactoring.guru/design-patterns/template-method">Template
    /// Method</a>: el método <see cref="BaseHandler.Handle(Message, out string)"/> tiene la lógica para procesar
    /// un comando, que incluye determinar si se puede procesar o no el comando, y en caso de que no se pueda procesar
    /// el comando, pasar el mensaje al siguiente "handler"; sin embargo, la lógica para procesar efectivamente el
    /// comando, se delega a los sucesores a través del método virtual
    /// <see cref="BaseHandler.InternalHandle(Message, out string)"/>; al mismo tiempo, aunque se provee la lógica
    /// predeterminada para determinar si se puede procesar un mensaje o no, en el método
    /// <see cref="BaseHandler.CanHandle(Message)"/>, los sucesores pueden sobrescribir este método para proveer otro
    /// comportamiento.
    /// </remarks>
    /// </summary>
    public abstract class BaseHandler : IHandler
    {
        /// <summary>
        /// Obtiene el próximo "handler".
        /// </summary>
        /// <value>El "handler" que será invocado si este "handler" no procesa el mensaje.</value>
        public IHandler Next { get; set; }

        /// <summary>
        /// Obtiene o asigna el conjunto de palabras clave que este "handler" puede procesar.
        /// </summary>
        /// <value>Un array de palabras clave.</value>
        public string[] Keywords { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BaseHandler"/>.
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public BaseHandler(IHandler next)
        {
            this.Next = next;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BaseHandler"/> con una lista de comandos.
        /// </summary>
        /// <param name="keywords">La lista de comandos.</param>
        /// <param name="next">El próximo "handler".</param>
        public BaseHandler(string[] keywords, BaseHandler next)
        {
            this.Keywords = keywords;
            this.Next = next;
        }


        // Creo un segundo constructor vacío de la clase
        // Ésto es para que los handlers de Usuario, Administrador y Proveedor puedan ejecutarlo
        public BaseHandler()
        {
        }

        /// <summary>
        /// Este método debe ser sobreescrito por las clases sucesores. La clase sucesora procesa el mensaje y asigna
        /// la respuesta al mensaje.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        protected abstract void InternalHandle(Message message, out string response);

        /// <summary>
        /// Este método puede ser sobreescrito en las clases sucesores que procesan varios mensajes cambiando de estado
        /// entre mensajes deben sobreescribir este método para volver al estado inicial. En la clase base no hace nada.
        /// </summary>
        protected virtual void InternalCancel(Message message)
        {
            // Intencionalmente en blanco.
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza el array
        /// <see cref="BaseHandler.Keywords"/> para buscar el texto en el mensaje ignorando mayúsculas y minúsculas. Las
        /// clases sucesores pueden sobreescribir este método para proveer otro mecanismo para determina si procesan o no
        /// un mensaje.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected virtual bool CanHandle(Message message)
        {
            // Cuando no hay palabras clave este método debe ser sobreescrito por las clases sucesoras y por lo tanto
            // este método no debería haberse invocado.
            if (this.Keywords == null || this.Keywords.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }

            return this.Keywords.Any(s => message.Text.Equals(s, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Procesa el mensaje o lo pasa al siguiente "handler" si existe.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>El "handler" que procesó el mensaje si el mensaje fue procesado; null en caso contrario.</returns>
        public IHandler Handle(Message message, out string response)
        {
            if (this.CanHandle(message))
            {
                this.InternalHandle(message, out response);
                return this;
            }
            else if (this.Next != null)
            {
                return this.Next.Handle(message, out response);
            }
            else
            {
                response = String.Empty;
                return null;
            }
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial. En los "handler" sin estado no hace nada. Los "handlers" que
        /// procesan varios mensajes cambiando de estado entre mensajes deben sobreescribir este método para volver al
        /// estado inicial.
        /// </summary>
        public virtual void Cancel(Message message)
        {
            this.InternalCancel(message);
            if (this.Next != null)
            {
                this.Next.Cancel(message);
            }
        }
    }
}