using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Serialization;
using Ucu.Poo.Locations.Client;

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
            /// Atributos de la clase: Nombre del usuario y la lista de depóstios asociados
            /// </summary>
            protected string Nombre;
            private List<IDeposito> DepositosUsuario = new List<IDeposito>();

            /// <summary>
            /// Constructor de la clase
            /// </summary>
            /// <param name="nombre">Nombre del usuario</param>
            public Usuario(string nombre, List<string> NombreDepositos)
            {
                this.Nombre = nombre;

                // Itero nombre deposito en la lista de nombres de depositos
                foreach (string nombreDeposito in NombreDepositos)
                {
                    // Obtengo un buscador
                    IBuscador<IDeposito> buscadorDepositos = BuscadorDepositos.GetBuscadorDepositos(nombreDeposito);

                    // Le digo al buscador que me busque el deposito con el nombre
                    IDeposito deposito = buscadorDepositos.Buscar();

                    // En caso que el resultado de la busqueda sea nulo (deposito no existe) que salte una excepcion
                    if (deposito == null)
                    {
                        throw new Exception($"El depósito '{nombreDeposito}' no existe");
                    }

                    // En otro caso, agrego el deposito a la lista de depositos del usuario
                    this.DepositosUsuario.Add(deposito);
                }
            }

            /// <summary>
            ///  Constructor vacío (para que se pueda instanciar en uno de los intermediarios)
            /// </summary>
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
                // Instancio un buscador de depositos
                IBuscador<IDeposito> buscadorDepositos = BuscadorDepositos.GetBuscadorDepositos(nombreDeposito);

                // Hago la búsqueda del depósito
                IDeposito deposito = buscadorDepositos.Buscar();
    
                // Instancio un buscador de secciones
                IBuscador<ISeccion> buscadorSecciones = BuscadorSecciones.GetBuscadorSecciones(nombreDeposito, nombreSeccion);

                // Busco la sección dentro del depósito
                ISeccion seccion = buscadorSecciones.Buscar();

                // En caso que el deposito no esté permitido para el usuario, que levante una excepcion
                if (!this.DepositosUsuario.Contains(deposito))
                {
                    throw new Exception($"El depósito '{nombreDeposito}' no está permitido para el usuario '{this.GetNombre}'");
                }

                // En caso que el deposito no se encuentre, que me levante una excepción avisando que no existe el deposito
                if (deposito == null)
                {
                    throw new Exception($"El depósito '{nombreDeposito}' no ha sido creado");
                }

                // En caso que la sección no se encuentre, que me levante una excepción avisando que no existe la seccion
                if (seccion == null)
                {
                    throw new Exception($"La sección '{nombreSeccion}' no ha sido creada");
                }

            // CREATOR --> Delego a la clase Seccion la responsabilidad de dar de alta el producto
            seccion.AltaProducto(nombre, precio, codigo, marca, categorias, stock);
        }

            public string DepositoMasCercano(int codigo, string ubicacionUsuario)
            {
                // Instancio un cliente de ubicación de la Api
                LocationApiClient cliente = new LocationApiClient();

                // Calculo la distancia entre el deposito correspondiente y el usuario
                DistanceCalculator calculadorDistancia = new DistanceCalculator(cliente);

                // Variable donde guardo el depóstio que tengo mas cerca
                IDeposito depositoMasCercano = null;

                // Generar una variable en donde guardo el valor de la distancia más cercana al usuario
                double distanciaMasCercana = 0;

                // Itero deposito por deposito para cada uno de los depositos del usuario
                foreach (IDeposito deposito in this.DepositosUsuario)
                {
                    // Calculo la distancia entre el deposito actual y la ubicacion del usuario
                    IDistanceResult distanciaResultado = calculadorDistancia.CalculateDistance(ubicacionUsuario, deposito.GetUbicacion);

                    // Expreso la distancia anterior en kilómetros
                    double distancia = distanciaResultado.Distance;

                    // Seteo una variable que me diga si el deposito tiene stock del producto
                    bool existeProducto = false;

                    // Compruebo que en el deposito haya stock del producto que estoy buscando
                    // Itero primero seccion por sección del deposito
                    foreach (ISeccion seccion in deposito.GetSecciones)
                    {
                        // En caso que el stock de producto que estoy buscando en la sección sea mayor a 0
                        if (seccion.CantidadStock(codigo) > 0)
                        {
                            // Ya sé que hay stock de producto en éste depósito y termino el bucle
                            existeProducto = true;
                            break;
                        }
                    }

                    // En caso que la distancia sea menor a la referencia y haya stock, me quedo con el deposito
                    // Abarco también el primer caso en donde la distancia más cercana es nula
                    if (distanciaMasCercana == 0 || (distancia < distanciaMasCercana && existeProducto))
                    {
                        // Me guardo el deposito
                        depositoMasCercano = deposito;

                        // Actualizo la variable de referencia
                        distanciaMasCercana = distancia;
                    }
                }

                // Devuelvo el deposito
                return depositoMasCercano.GetNombre;
            }

            /// <summary>
            /// Método que realiza una venta de un producto
            /// </summary>
            public string RealizarVenta(List<List<string>> datosVentas)
            {
                // Inicializo la cadena donde voy a guardar la advertencia al usuario si hay un producto que falta
                StringBuilder cadenaAdvertencia = new StringBuilder();

                // Especifico fecha de la venta
                DateTime fechaVenta = DateTime.Today;

            // Instancio una venta
            VentaTotal ventaTotal = new VentaTotal(fechaVenta);

                // Itero para cada una de las ventas individuales
                foreach (List<string> ventasIndividuales in datosVentas)
                {
                    // Instancio un buscador de depositos
                    IBuscador<IDeposito> buscadorDepositos = BuscadorDepositos.GetBuscadorDepositos(ventasIndividuales[3]);

                    // Hago la búsqueda del depósito
                    IDeposito deposito = buscadorDepositos.Buscar();

                    // En caso que el deposito no esté permitido para el usuario, que levante una excepcion
                    if (!this.DepositosUsuario.Contains(deposito))
                    {
                        throw new Exception($"El depósito '{ventasIndividuales[3]}' no está permitido para el usuario '{this.GetNombre}'");
                    }

                    // En caso que el deposito no se encuentre, que me levante una excepción avisando que no existe el deposito
                    if (deposito == null)
                    {
                        throw new Exception($"El depósito '{ventasIndividuales[3]}' no ha sido creado");
                    }

                    // Agrego la venta individual a la lista de ventas individuales
                    ventaTotal.AgregarVenta(Convert.ToInt32(ventasIndividuales[0]), Convert.ToInt32(ventasIndividuales[1]),
                    ventasIndividuales[2], ventasIndividuales[3]);
                }

                // Delego la responsabilidad de disminuir el stock de una venta a VentaTotal
                ventaTotal.DisminuirStockTotal(cadenaAdvertencia);

                // Delego la responsabilidad de agregar una venta por fecha a la clase ContenedorVentasPorFecha
                ContenedorVentasPorFecha.AgregarVentaPorFecha(ventaTotal, fechaVenta);

                // Retorno la cadena donde tengo la advertencia
                return cadenaAdvertencia.ToString();
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
