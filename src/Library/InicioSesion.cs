using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Se define clase para el inicio de sesión de un usuario 
    /// </summary>
    public class InicioSesion
    {
        /// <summary>
        /// Atributo estático PIN_Usuario en donde se almacenará el PIN del usuario generado
        /// </summary>

        private static string PIN_Usuario;

        // Atributo estático userTypes en donde se me guardan todos los posibles tipos de usuario
        private static List<string> userTypes = new List<string>(){"Usuario", "Administrador","Proveedor"};

        /// <summary>
        /// Método para la verificación del PIN de acceso 
        /// </summary>
        /// <returns>True or false</returns>

        public static bool Verificar()
        {     
            string userType = Console.ReadLine();

            if (userTypes.Contains(userType))
            {
                // Pido código al usuario
                System.Console.WriteLine("Ingrese código"); 

                string passCode = Console.ReadLine();

                // En caso que el código ingresado sea el asignado al usuario
                if (passCode == PIN_Usuario)
                {
                    // Lo envia a la clase correspondiente para seguir con el programa
                    System.Console.WriteLine($"Haz sido verificado");
                    return true;
                }  

                // En caso que el código sea erróneo, que levante una excepcion avisando que el PIN es invalido
                else
                {
                    throw new PINInvalidoExcepcion("El PIN ingresado es incorrecto");
                }
            }

            else
            {
                System.Console.WriteLine("Error, inténtelo nuevamente");
                return false;
            }
        }

        /// <summary>
        /// Método que me genera el PIN de acceso del usuario en base al valor generado
        /// </summary>

        public static void GeneracionPIN()
        {
            // Pido al usuario que me de su tipo de acceso
            Console.WriteLine("Dame tu usuario:");

            string userType = Console.ReadLine();

            // Si el tipo de acceso ingresado no es "Administrador", "Usuario" o "Proveedor" que levante una excepción
            // Se usa KeyNotFoundException
            if (!InicioSesion.userTypes.Contains(userType))
            {
                throw new TipoFuncionarioNoEncontradoExcepcion("El tipo de acceso ingresado debe ser Administrador, Usuario o Proveedor");
            }

            // Genero pines aleatorios para Administrador, Usuario y Proveedor
            Autenticacion.Autenticar();

            // Seteo PIN_Usuario como el PIN de acceso generado al usuario
            PIN_Usuario = Autenticacion.codigos[userType];

            Console.WriteLine($"Su PIN de acceso es: {PIN_Usuario}");
        }
        
    }
}