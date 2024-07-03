using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProyectoFinal
{


    /// <summary>
    /// Clase responsable de la autenticación de diferentes tipos de funcionarios: administrador, usuario y proveedor.
    /// Genera códigos aleatorios para cada tipo de funcionario y los almacena en un diccionario.
    /// </summary>
    public class Autenticacion
    {
        /// <summary>
        /// Diccionario que almacena los códigos de autenticación con el tipo de funcionario como clave.
        /// </summary>
        /// <typeparam name="string">Tipo de datos de la clave - string</typeparam>
        /// <typeparam name="string">">Tipo de datos de los calores - string</typeparam>
        /// <returns>Diccionario de códigos</returns>
        public static Dictionary<string, string> codigos = new Dictionary<string, string>();

        /// <summary>
        /// Genera códigos de autenticación aleatorios para los distintos tipos de funcionarios y los guarda en el diccionario.
        /// </summary>
        public static void Autenticar()
        {
            // Generar códigos aleatorios para los distintos tipos de funcionarios
            // string codigoAdmin = GenerarCodigoAdmin();
            //string codigoUsuario = GenerarCodigo();
            //string codigoProveedor = GenerarCodigo();

            /// <summary>
            ///  Guardar los códigos en el diccionario
            /// </summary>
            /// <returns></returns>
        
            codigos.Add("Administrador", GenerarPIN());
            codigos.Add("Usuario", GenerarPIN());
            codigos.Add("Proveedor", GenerarPIN());
        }

        /// <summary>
        /// Método para generar un código aleatorio de 4 dígitos
        /// </summary>
        /// <returns></returns>
        private static string GenerarPIN()
        {

        /// <summary>
        /// Genera un código aleatorio de 4 dígitos
        /// </summary>
        /// <returns>Código aleatorio de 4 dígitos</returns>
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }
    }
}