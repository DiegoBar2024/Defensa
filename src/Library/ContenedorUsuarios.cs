using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Clase para la creación de objetos del tipo Usuario.
    /// Se utiliza el patrón Creator (leer archivo readme para más detalles)
    /// </summary>
    public class ContenedorUsuarios
    {
        // Lista que contiene los objetos del tipo Usuario
        private static List<Usuario> Usuarios = new List<Usuario>();

        /// <summary>
        /// Usando el patrón Creator agrego un usuario a la lista usando los parámetros dados de entrada
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="permiso">Si se tienen permisos de administrador</param>
        /// <returns></returns>
        public static Usuario AltaUsuario(string nombre, string permiso, List<string> nombresDepositos)
        {
            bool Permiso = Convert.ToBoolean(permiso);
            /// <summary>
            /// Creación de un usuario en base a parámetros de entrada
            /// </summary>
            /// <returns> El usuario (administrador o usuario)</returns>
            
            // En caso que el usuario tenga permisos de administrador, que me cree un objeto Administrador
            // y me lo agregue a la lista de usuarios
            // Como Administrador es un SUBTIPO de Usuario, no va a haber problema con la agregación
            if (Permiso)
            {
                // Instanciación de objeto Administrador
                Administrador administrador = new Administrador(nombre);

                // Lo agrego a la lista
                ContenedorUsuarios.Usuarios.Add(administrador);

                return administrador;
            }

            // En caso que el usuario no tenga permisos de administrador, que me cree un objeto Usuario
            // y me lo agregue a la lista de usuarios
            else
            {
                // Instanciación de un objeto Usuario
                Usuario usuario = new Usuario(nombre, nombresDepositos);

                // Lo agrego a la lista
                ContenedorUsuarios.Usuarios.Add(usuario);

                // Retorno usuario
                return usuario;
            }
        }

        /// <summary>
        /// Propiedad (getter) que retorna la lista de usuarios 
        /// </summary>
        /// <value>Lista de usuarios</value>
        public static List<Usuario> GetUsuarios
        {
            get
            {

                return ContenedorUsuarios.Usuarios;
            }
        }

        // Metodo para dar de baja a un usuario
        public static void BajaUsuario(string nombre)
        {
            foreach (Usuario usuario in ContenedorUsuarios.Usuarios.ToList())
            {
                // usuario.GetNombre == nombre
                if (usuario.GetNombre.Equals(nombre))
                {
                    ContenedorUsuarios.Usuarios.Remove(usuario);
                }
            }
        }
    }
}