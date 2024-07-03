using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Clase para la creación de objetos del tipo IDeposito.
    /// Se usa el patron Creator (leer archivo readme para más detalles).
    /// </summary>
    public class ContenedorDepositos
    {
        private static List<IDeposito> Depositos = new List<IDeposito>();

        /// <summary>
        /// Uso el patrón Creator para asignar la responsabilidad de la creación de un depósito a ésta clase
        /// que contiene múltiples objetos Deposito
        /// </summary>
        /// <param name="nombre">Nombre del depósito</param>
        /// <param name="ubicacion">Ubicación del depósito</param>
        /// <param name="capacidad">Capacidad del depósito</param>
        /// <param name="distancia">Distancia depósitos</param>
        /// <returns></returns>
        public static IDeposito AgregarDeposito(string nombre, string ubicacion, int capacidad)
        {
            /// <summary>
            /// Creo el depósito con los parámetros de entrada
            /// </summary>
            /// <returns>El depósito</returns>
            IDeposito deposito = new Deposito(nombre, ubicacion, capacidad);

            // Agrego el depósito a la lista de depositos
            ContenedorDepositos.Depositos.Add(deposito);

            /// <summary>
            /// Retorno el deposito creado
            /// </summary>
            return deposito;
        }


        /// <summary>
        /// Creo un método que me elimine todos los depósitos
        /// </summary>
        public static void EliminarDepositos()
        {
            ContenedorDepositos.Depositos.Clear();
        }

       /// <summary>
       /// Creo un método getter que me permita acceder a los depósitos externamente
       /// </summary>
       /// <value>Depósitos</value>
        public static IEnumerable<IDeposito> GetDepositos
        {
            get
            {
                return ContenedorDepositos.Depositos;
            }
        }

        /// <summary>
        /// Creo un metodo que me devuelva los nombres de todos los depositos usando Expert
        /// </summary>
        /// <returns></returns>
        public static string NombresDepositos()
        {
            // Creo un StringBuilder que me almacene la cadena a imprimir
            StringBuilder stringBuilder = new StringBuilder("Los depósitos existentes son los siguientes:");

            // Itero deposito por deposito en la lista de depositos
            foreach (IDeposito deposito in ContenedorDepositos.Depositos)
            {
                stringBuilder.AppendLine($"- {deposito.GetNombre}");
            }

            // Retorno la cadena correspondiente
            return stringBuilder.ToString();
        }
    }
}