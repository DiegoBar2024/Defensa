using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Xml.Serialization;

namespace ProyectoFinal
{
    /// <summary>
    /// Clase que representa un Usuario.
    /// Se utiliza SRP para delegar responsabilidades a otras clases. 
    /// Se utiliza también el patrón Creator (leer archivo readme para más detalles).
    /// </summary>
    public class Usuario
    {
            /// <summary>
            /// Atributos de la clase
            /// </summary>
             protected string Nombre;

            /// <summary>
            /// Constructor de la clase
            /// </summary>
            /// <param name="nombre">Nombre del usuario</param>
            public Usuario(string nombre)
            {
                this.Nombre = nombre;
            }

            /// <summary>
            /// Método que agrega un producto a una sección de un depósito
            /// </summary>
            /// <param name="nombre">Nombre del producto</param>
            /// <param name="precio">Precio del producto</param>
            /// <param name="codigo">Código del producto</param>
            /// <param name="marca">Marca del producto</param>
            /// <param name="categorias">Categoría del producto</param>
            /// <param name="nombreSeccion">Nombre de la sección en la cual agrego el producto</param>
            /// <param name="nombreDeposito">Nombre del depósito que contiene la sección que agregué el producto</param>
            /// <param name="stock">Stock del  producto</param>
            public void AltaProducto(string nombre, double precio, int codigo, string marca, List<string> categorias, string nombreSeccion, string nombreDeposito, int stock)
            {
                // Instancio un buscador de secciones
                IBuscador<ISeccion> buscadorSecciones = new BuscadorSecciones(nombreDeposito, nombreSeccion);

                // Busco la sección dentro del depósito
                ISeccion seccion = buscadorSecciones.Buscar();

                // CREATOR --> Delego a la clase Seccion la responsabilidad de dar de alta el producto
                seccion.AltaProducto(nombre, precio, codigo, marca, categorias, stock);
            }

            /*public string DepositoMasCercano(int codigo)
            {
            }*/

            /// <summary>
            /// Método que realiza una venta de un producto
            /// </summary>
            public void RealizarVenta()
            {
                // Especifico fecha de la venta
                DateTime fechaVenta = DateTime.Now;

                // Instancio una venta
                VentaTotal ventaTotal = new VentaTotal(fechaVenta);

                // Inicializo la variable booleana que me diga si sigo pidiendo datos al usuario
                bool pidiendoDatos = true;

                while (pidiendoDatos)
                {
                    // FASE 1: PEDIR DATOS DE LA VENTA AL USUARIO

                    // Pido al usuario el código del producto que se está vendiendo
                    System.Console.WriteLine("Ingrese código del producto: ");
                    int codigoProducto = Convert.ToInt32(Console.ReadLine());

                    // Pido al usuario la cantidad de stock que estoy vendiendo
                    System.Console.WriteLine("Ingrese cantidad de stock vendido: ");
                    int stock = Convert.ToInt32(Console.ReadLine());

                    // Pido al usuario el nombre de la sección de donde estoy sacando el producto
                    System.Console.WriteLine("Ingrese el nombre de sección de donde está vendiendo: ");
                    string nombreSeccion = Console.ReadLine();

                    // Pido al usuario el nombre del depósito de donde se está sacando el producto
                    System.Console.WriteLine("Ingrese el nombre del depósito de donde se está vendiendo: ");
                    string nombreDeposito = Console.ReadLine();

                    // FASE 2: AGREGAR LA VENTA INDIVIDUAL A LA LISTA DE VENTAS INDIVIDUALES

                    // Agrego la venta individual a la lista de ventas individuales
                    ventaTotal.AgregarVenta(codigoProducto, stock, nombreSeccion, nombreDeposito);

                    // FASE 3: PREGUNTAR AL USUARIO SI QUIERE HACER OTRA VENTA

                    // Pregunto al usuario si quiere hacer otra venta
                    System.Console.WriteLine("¿Quiere agregar otro producto a su venta? (S/N)");
                    string terminarVenta = Console.ReadLine();

                    // En caso que el usuario quiera terminar de vender, que me setee la bandera a false
                    if (terminarVenta.Trim().Equals("N", StringComparison.OrdinalIgnoreCase))
                    {
                        pidiendoDatos = false;
                    }
                }

                // Delego la responsabilidad de disminuir el stock de una venta a VentaTotal
                ventaTotal.DisminuirStockTotal();       
                
                // Delego la responsabilidad de agregar una venta por fecha a la clase ContenedorVentasPorFecha
                ContenedorVentasPorFecha.AgregarVentaPorFecha(ventaTotal, fechaVenta);
            }

            /// <summary>
            /// Método getter para el nombre
            /// </summary>
            /// <value>Nombre del usuario</value>
            public string GetNombre
            {
                get
                {
                    return this.Nombre;
                }
            }
        }
}