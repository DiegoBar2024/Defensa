//-----------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using ProyectoFinal;

namespace Ucu.Poo.Locations.Client
{
    /// <summary>
    /// Un programa que demuestra el uso del cliente de la API REST de localización.
    /// </summary>
    public class calculoDistancia

    {
        /// <summary>
        /// creamos una iteracion para poder usar distance para usarla en usuario.
        /// </summary>
        public static Distance distanciaFinal;
        /// <summary>
        /// Punto de entrada al programa.
        /// </summary>
        public static async Task Direcciones( string addressUsuario, string addressDeposito)
        {
           /* const string addressUsuario = "Av. 8 de Octubre 2738";
            const string addressDeposito = "Comandante Braga 2715";*/
            LocationApiClient client = new LocationApiClient();

            // Versión asincrónica

            Location locationUsuario = await client.GetLocationAsync(addressUsuario);
            Console.WriteLine($"Las coordenadas de '{addressUsuario}' son " +
                $"'{locationUsuario.Latitude}:{locationUsuario.Longitude}'");

            Location locationDeposito = await client.GetLocationAsync(addressDeposito);
            Console.WriteLine($"Las coordenadas de '{addressDeposito}' son " +
                $"'{locationDeposito.Latitude}:{locationDeposito.Longitude}'");

            Distance distance = await client.GetDistanceAsync(locationUsuario, locationDeposito);
            Console.WriteLine($"La distancia entre '{locationUsuario.Latitude},{locationUsuario.Longitude}' y "+
                $"'{locationDeposito.Latitude},{locationDeposito.Longitude}' es de {distance.TravelDistance} kilómetros.");

            distance = await client.GetDistanceAsync(addressUsuario, addressDeposito);
            Console.WriteLine($"La distancia entre '{addressUsuario}' y '{addressDeposito}' " +
                $"es de {distance.TravelDistance} kilómetros.");
            
            

            await client.DownloadMapAsync(locationUsuario.Latitude, locationUsuario.Longitude, @"map-a.png");
            Console.WriteLine($"Descargado asincrónicamente el mapa de '{addressUsuario}'");

            await client.DownloadRouteAsync(
                locationUsuario.Latitude,
                locationUsuario.Longitude,
                locationDeposito.Latitude,
                locationDeposito.Longitude,
                @"route-a.png");
            Console.WriteLine($"Descargado asincrónicamente el mapa de '{addressUsuario}' a '{addressDeposito}'");

            // Versión sincrónica

            locationUsuario = client.GetLocation(addressUsuario);
            Console.WriteLine($"Las coordenadas de '{addressUsuario}' son " +
                $"'{locationUsuario.Latitude}:{locationUsuario.Longitude}'");

            locationDeposito = client.GetLocation(addressDeposito);
            Console.WriteLine($"Las coordenadas de '{addressDeposito}' son " +
                $"'{locationDeposito.Latitude}:{locationDeposito.Longitude}'");

            distance = client.GetDistance(locationUsuario, locationDeposito);
            Console.WriteLine($"La distancia entre '{locationUsuario.Latitude},{locationUsuario.Longitude}' y "+
                $"'{locationDeposito.Latitude},{locationDeposito.Longitude}' es de {distance.TravelDistance} kilómetros.");

            distance = client.GetDistance(addressUsuario, addressDeposito);
            Console.WriteLine($"La distancia entre '{addressUsuario}' y '{addressDeposito}' " +
                $"es de {distance.TravelDistance} kilómetros.");
            
            calculoDistancia.distanciaFinal= distance;

            client.DownloadMap(locationUsuario.Latitude, locationUsuario.Longitude, @"map-s.png");
            Console.WriteLine($"Descargado sincrónicamente el mapa de '{addressUsuario}'");

            client.DownloadRoute(
                locationUsuario.Latitude,
                locationUsuario.Longitude,
                locationDeposito.Latitude,
                locationDeposito.Longitude,
                @"route-s.png");
            Console.WriteLine($"Descargado sincrónicamente el mapa de '{addressUsuario}' a '{addressDeposito}'");
        }
    }
}
