using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
  public class EspaciosServiceImpl : Espacios.EspaciosBase
  {
    private readonly ILogger<EspaciosServiceImpl> _logger;
    private readonly List<Espacio> _espacios = new List<Espacio>();

    public EspaciosServiceImpl(ILogger<EspaciosServiceImpl> logger)
    {
      _logger = logger;
    }

    public override Task<EspacioResponse> CreateEspacio(EspacioCreateRequest request, ServerCallContext context)
    {
      var espacio = new Espacio
      {
        Id = _espacios.Count > 0 ? _espacios.Max(e => e.Id) + 1 : 1,
        Name = request.Name,
        Capacity = request.Capacity
      };

      _espacios.Add(espacio);

      Console.WriteLine($"Servidor: Espacio creado: ID = {espacio.Id}, Nombre = {espacio.Name}, Capacidad = {espacio.Capacity}");

      return Task.FromResult(new EspacioResponse
      {
        Espacio = espacio
      });
    }

    public override Task<EspacioResponse> GetEspacio(EspacioConsultRequest request, ServerCallContext context)
    {
      var espacio = _espacios.FirstOrDefault(e => e.Id == request.Id);

      if (espacio == null)
      {
        Console.WriteLine($"Servidor: Espacio con ID {request.Id} no encontrado.");
        throw new RpcException(new Status(StatusCode.NotFound, "Espacio no encontrado"));
      }

      Console.WriteLine($"Servidor: Espacio encontrado: ID = {espacio.Id}, Nombre = {espacio.Name}, Capacidad = {espacio.Capacity}");

      return Task.FromResult(new EspacioResponse
      {
        Espacio = espacio
      });
    }

    public override Task<EspacioListResponse> GetAllEspacios(Empty request, ServerCallContext context)
    {
      var response = new EspacioListResponse();
      response.Espacios.AddRange(_espacios);

      Console.WriteLine($"Servidor: Total de espacios disponibles: {response.Espacios.Count}");

      return Task.FromResult(response);
    }

    public override Task<EspacioResponse> UpdateEspacio(EspacioUpdateRequest request, ServerCallContext context)
    {
      var espacio = _espacios.FirstOrDefault(e => e.Id == request.Id);

      if (espacio == null)
      {
        Console.WriteLine($"Servidor: Espacio con ID {request.Id} no encontrado para actualización.");
        throw new RpcException(new Status(StatusCode.NotFound, "Espacio no encontrado"));
      }

      espacio.Name = request.Name;
      espacio.Capacity = request.Capacity;

      Console.WriteLine($"Servidor: Espacio actualizado: ID = {espacio.Id}, Nombre = {espacio.Name}, Capacidad = {espacio.Capacity}");

      return Task.FromResult(new EspacioResponse
      {
        Espacio = espacio
      });
    }

    public override Task<Empty> DeleteEspacio(EspacioDeleteRequest request, ServerCallContext context)
    {
      var espacio = _espacios.FirstOrDefault(e => e.Id == request.Id);

      if (espacio == null)
      {
        Console.WriteLine($"Servidor: Espacio con ID {request.Id} no encontrado para eliminación.");
        throw new RpcException(new Status(StatusCode.NotFound, "Espacio no encontrado"));
      }

      _espacios.Remove(espacio);

      Console.WriteLine($"Servidor: Espacio eliminado: ID = {espacio.Id}, Nombre = {espacio.Name}");

      return Task.FromResult(new Empty());
    }
  }
}
