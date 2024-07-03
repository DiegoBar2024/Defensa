using System;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Creo una clase BuscadorDepositos que se encargue de buscar depositos en base a un nombre
    /// Se utiliza el patrón Polimorfismo (leer archivo readme para más detalles).
    /// Aplicamos el patrón SINGLETON para ésta clase
    /// </summary>
    public class BuscadorDepositos : IBuscador<IDeposito>
    {
        /// <summary>
        /// Atributos de la clase
        /// </summary>
        private static string NombreDeposito;

        private static BuscadorDepositos buscadorDepositos;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombreDeposito">Nombre del depósito a buscar</param>
        private BuscadorDepositos()
        {
        }

        // Creo un método estático público el cual haga la función de obtener la instancia de Singleton
        public static IBuscador<IDeposito> GetBuscadorDepositos(string nombreDeposito)
        {
            // En caso que no tenga ningún buscador de depósitos creo uno
            if (buscadorDepositos == null)
            {
                // Llamo al constructor privado para crear el nuevo buscador de depósitos
                BuscadorDepositos.buscadorDepositos = new BuscadorDepositos();
            }

            // Asigno el nombre del depósito
            BuscadorDepositos.NombreDeposito = nombreDeposito;

            // En otro caso devuelvo el mismo buscador de depósitos
            return buscadorDepositos;
        }

        /// <summary>
        /// Creo un método que me traduzca un nombre de deposito en un deposito
        /// </summary>
        /// <returns> Depósito correspondiente al nombre o en caso que no se encuentre retorna null</returns>
        public IDeposito Buscar()
        {
            // Itero depósito por depósito en el contenedor de depósitos            
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                /// <summary>
                /// En caso que el nombre del depósito sea el que estoy buscando, que me retorne el depósito
                /// </summary>
                /// <returns>El depósito buscado por nombre</returns>
                if (string.Equals(deposito.GetNombre, BuscadorDepositos.NombreDeposito))
                {
                    return deposito;
                }
            }

        /// <summary>
        /// Retorno el valor null en otro caso
        /// </summary>
        return null;

        }
    }
}
