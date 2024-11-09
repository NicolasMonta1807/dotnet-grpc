using GrpcClient;
using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

class Program
{
  static async Task Main(string[] args)
  {
    // Crea un canal de comunicación al servidor gRPC
    using var channel = GrpcChannel.ForAddress("http://localhost:5126");

    // Crea un cliente para el servicio EspaciosService
    var client = new Espacios.EspaciosClient(channel);

    // Crear un nuevo espacio
    var createResponse = await client.CreateEspacioAsync(new EspacioCreateRequest
    {
      Name = "Sala de Conferencias",
      Capacity = 20
    });
    Console.WriteLine($"Espacio creado: ID = {createResponse.Espacio.Id}, Nombre = {createResponse.Espacio.Name}, Capacidad = {createResponse.Espacio.Capacity}");

    // Obtener un espacio
    Console.WriteLine("Consultando espacio con ID: " + createResponse.Espacio.Id);
    var getResponse = await client.GetEspacioAsync(new EspacioConsultRequest
    {
      Id = createResponse.Espacio.Id
    });
    Console.WriteLine($"Espacio obtenido: ID = {getResponse.Espacio.Id}, Nombre = {getResponse.Espacio.Name}, Capacidad = {getResponse.Espacio.Capacity}");

    // Listar todos los espacios
    var listResponse = await client.GetAllEspaciosAsync(new Empty());
    Console.WriteLine("Lista de espacios:");
    foreach (var espacio in listResponse.Espacios)
    {
      Console.WriteLine($"ID = {espacio.Id}, Nombre = {espacio.Name}, Capacidad = {espacio.Capacity}");
    }

    // Actualizar un espacio
    var updateResponse = await client.UpdateEspacioAsync(new EspacioUpdateRequest
    {
      Id = createResponse.Espacio.Id,
      Name = "Sala de Juntas",
      Capacity = 30
    });
    Console.WriteLine($"Espacio actualizado: ID = {updateResponse.Espacio.Id}, Nombre = {updateResponse.Espacio.Name}, Capacidad = {updateResponse.Espacio.Capacity}");

    // Eliminar un espacio
    await client.DeleteEspacioAsync(new EspacioDeleteRequest
    {
      Id = createResponse.Espacio.Id
    });
    Console.WriteLine("Espacio eliminado.");
  }
}
