using System;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Creo una clase BuscadorSecciones que se encargue de buscar secciones en base a datos de entrada
    /// Se utiliza el patrón Polimorfismo (leer archivo readme para más detalles).
    /// </summary>
    public class BuscadorSecciones : IBuscador<ISeccion>
    {
        /// <summary>
        /// Atributos nombreDeposito y nombreSeccion
        /// </summary>
        private string NombreDeposito;
        private string NombreSeccion;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombreDeposito">Nombre del depósito donde se encuentra la sección</param>
        /// <param name="nombreSeccion">Nombre de la sección que se quiere buscar</param>
        public BuscadorSecciones(string nombreDeposito, string nombreSeccion)
        {
            this.NombreDeposito = nombreDeposito;
            this.NombreSeccion = nombreSeccion;
        }

        /// <summary>
        /// Creo un método que busque una sección por nombre dentro de un depósito
        /// </summary>
        /// <returns>La sección que estoy buscando o en caso contrario retorna null </returns>
        public ISeccion Buscar()
        {
            // Itero depósito por depósito en la lista de depósitos
            foreach (IDeposito deposito in ContenedorDepositos.GetDepositos)
            {
                /// <summary>
                /// En caso que encuentre aquel depósito cuyo nombre sea igual al que le pasé, me quedo en ese
                /// </summary>
                /// <returns>La seccion</returns>
                if (deposito.GetNombre.Equals(this.NombreDeposito, StringComparison.OrdinalIgnoreCase))
                {
                    // Itero seccion por seccion en la lista de secciones
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        /// <summary>
                        /// En caso que la sección tenga el nombre buscado, retorno dicho objeto
                        /// </summary>
                        /// <returns>La sección</returns>
                        if (seccion.GetNombre.Equals(this.NombreSeccion, StringComparison.OrdinalIgnoreCase))
                        {
                            return seccion;
                        }
                    }
                }
            }

            /// <summary>
            /// En otro caso, retorno valor nulo
            /// </summary>
            return null;
        }
    }
}