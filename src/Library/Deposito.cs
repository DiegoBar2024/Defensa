using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Xml.Serialization;

namespace ProyectoFinal
{

    /// <summary>
    /// Se usa el patrón Creator, esta clase es la ideal para la creación de objetos ISeccion.
    /// /// Se crea interfaz IDeposito con el objetivo de utilizar el principio OCP (para mas detalles leer archivo readme)
    /// </summary>
    public class Deposito : IDeposito
    {
       /// <summary>
        /// Atributos de la clase
        /// </summary>

        public string Nombre;
        private string Ubicacion;   
        private int Capacidad;
        private double Distancia;
        private IList ListaSecciones = new ArrayList();

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombre">Nombre del depósito</param>
        /// <param name="ubicacion">Ubicación del depósito</param>
        /// <param name="capacidad">Capacidad del depósito</param>
        /// <param name="distancia">Distancia depósitos</param>

        public Deposito(string nombre, string ubicacion, int capacidad, double distancia)
        {
            this.Nombre = nombre;
            this.Ubicacion = ubicacion;
            this.Capacidad = capacidad;
            this.Distancia = distancia;
        }

        /// <summary>
        /// Usando el patrón Creator le asigno al depósito la responsabilidad de crear una seccion
        /// </summary>
        /// <param name="nombre">Nombre de la sección</param>
        /// <param name="capacidad">Capacidad de la sección</param>
        /// <returns>La sección</returns>

        public ISeccion CrearSeccion(string nombre, int capacidad)
        {
            // En caso que el nombre de la sección ya exista, que me levante una excepcion
            if (this.GetNombresSecciones.Contains(nombre))
            {
                throw new SeccionExistenteExcepcion($"La sección con el nombre '{nombre}' ya ha sido creada dentro de éste depósito");
            }

            // En caso que el depósito tenga la capacidad llena, que me levante una excepción
            if (this.ListaSecciones.Count >= this.Capacidad)
            {
                throw new CapacidadInsuficienteExcepcion("No se pueden agregar más secciones a éste depósito ya que se ha alcanzado su capacidad máxima");
            }

            // Creo una sección usando los parámetros de entrada
            ISeccion seccion = new Seccion(nombre, capacidad);

            // Agrego la sección a la lista de secciones
            this.ListaSecciones.Add(seccion);

            // Retorno la sección creada
            return seccion;
        }

        /// <summary>
        /// Construyo un getter para las secciones dentro de un deposito
        /// </summary>
        /// <value>Las secciones</value>

        public IEnumerable GetSecciones
        {
            get
            {
                return this.ListaSecciones;
            }
        }

        /// <summary>
        /// Construyo método getter que me de los nombres de secciones dentro de un depósito
        /// </summary>
        /// <value>Lista de nombre de secciones dentro de un depósito</value>
        public List<string> GetNombresSecciones
        {
            get
            {
                // Creo una lista en donde guardo el resultado
                List<string> result = new List<string>();

                // Itero sección por sección en el depósito
                foreach (ISeccion seccion in this.ListaSecciones)
                {
                    // Agrego el nombre de cada seccion en la lista
                    result.Add(seccion.GetNombre);
                }

                // Retorno la lista con los nombres de secciones
                return result;
            }
        }

        /// <summary>
        /// Creo un método getter para el nombre del deposito
        /// </summary>
        /// <value>Nombre del depósito</value>

        public string GetNombre
        {
            get
            {
                return this.Nombre;
            }
        }
    }
}