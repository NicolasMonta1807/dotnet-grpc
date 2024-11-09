using GrpcClient;
using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

class Program
{
  static async Task Main(string[] args)
  {
    using var channel = GrpcChannel.ForAddress("http://localhost:5126");

    var client = new Espacios.EspaciosClient(channel);

    var createResponse = await client.CreateEspacioAsync(new EspacioCreateRequest
    {
      Name = "Sala de Conferencias",
      Capacity = 20
    });
    Console.WriteLine($"Cliente: Espacio creado: ID = {createResponse.Espacio.Id}, Nombre = {createResponse.Espacio.Name}, Capacidad = {createResponse.Espacio.Capacity}");

    // Obtener un espacio
    Console.WriteLine($"Cliente: Consultando espacio con ID: {createResponse.Espacio.Id}");
    var getResponse = await client.GetEspacioAsync(new EspacioConsultRequest
    {
      Id = createResponse.Espacio.Id
    });
    Console.WriteLine($"Cliente: Espacio obtenido: ID = {getResponse.Espacio.Id}, Nombre = {getResponse.Espacio.Name}, Capacidad = {getResponse.Espacio.Capacity}");

    // Listar todos los espacios
    var listResponse = await client.GetAllEspaciosAsync(new Empty());
    Console.WriteLine("Cliente: Lista de espacios:");
    foreach (var espacio in listResponse.Espacios)
    {
      Console.WriteLine($"Cliente: ID = {espacio.Id}, Nombre = {espacio.Name}, Capacidad = {espacio.Capacity}");
    }

    // Actualizar un espacio
    var updateResponse = await client.UpdateEspacioAsync(new EspacioUpdateRequest
    {
      Id = createResponse.Espacio.Id,
      Name = "Sala de Juntas",
      Capacity = 30
    });
    Console.WriteLine($"Cliente: Espacio actualizado: ID = {updateResponse.Espacio.Id}, Nombre = {updateResponse.Espacio.Name}, Capacidad = {updateResponse.Espacio.Capacity}");

    // Eliminar un espacio
    await client.DeleteEspacioAsync(new EspacioDeleteRequest
    {
      Id = createResponse.Espacio.Id
    });
    Console.WriteLine("Cliente: Espacio eliminado.");
  }
}
