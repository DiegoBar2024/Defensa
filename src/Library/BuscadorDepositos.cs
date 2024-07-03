using System;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Creo una clase BuscadorDepositos que se encargue de buscar depositos en base a un nombre
    /// Se utiliza el patrón Polimorfismo (leer archivo readme para más detalles).
    /// </summary>
    public class BuscadorDepositos : IBuscador<IDeposito>
    {
        /// <summary>
        /// Atributos de la clase
        /// </summary>
        private string NombreDeposito;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombreDeposito">Nombre del depósito a buscar</param>
        public BuscadorDepositos(string nombreDeposito)
        {
            this.NombreDeposito = nombreDeposito;
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
                if (string.Equals(deposito.GetNombre, this.NombreDeposito))
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

