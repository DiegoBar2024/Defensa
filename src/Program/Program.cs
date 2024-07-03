//--------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ProyectoFinal;
using System.Threading;
using System.Xml.Serialization;

namespace ConsoleApplication
{
    /// <summary>
    /// Programa de consola de demostración.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Punto de entrada al programa principal.
         /// </summary>
        public static void Main()
        {

            try
            {
                InicioSesion.GeneracionPIN();
                Console.WriteLine("Este mensaje será borrado después de 5 segundos.");
                Timer timer = new Timer(ClearConsole, null, 5000, Timeout.Infinite);
                InicioSesion.Verificar();
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void ClearConsole(object state)
        {
            Console.Clear(); // Limpiar la consola
            Console.WriteLine("Consola limpia.");
            System.Console.WriteLine("Ingrese nuevamente su Usuario: ");
        }
    }
}