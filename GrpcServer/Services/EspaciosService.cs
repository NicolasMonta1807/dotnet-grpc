using Grpc.Core;

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
        Id = _espacios.Count > 0 ? _espacios.Max(e => e.Id) + 1 : 1, // Genera un ID incremental
        Name = request.Name,
        Capacity = request.Capacity
      };

      _espacios.Add(espacio);

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
        throw new RpcException(new Status(StatusCode.NotFound, "Espacio no encontrado"));
      }

      return Task.FromResult(new EspacioResponse
      {
        Espacio = espacio
      });
    }

    public override Task<EspacioListResponse> GetAllEspacios(Empty request, ServerCallContext context)
    {
      var response = new EspacioListResponse();
      response.Espacios.AddRange(_espacios);

      return Task.FromResult(response);
    }

    public override Task<EspacioResponse> UpdateEspacio(EspacioUpdateRequest request, ServerCallContext context)
    {
      var espacio = _espacios.FirstOrDefault(e => e.Id == request.Id);

      if (espacio == null)
      {
        throw new RpcException(new Status(StatusCode.NotFound, "Espacio no encontrado"));
      }

      // Actualiza el espacio
      espacio.Name = request.Name;
      espacio.Capacity = request.Capacity;

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
        throw new RpcException(new Status(StatusCode.NotFound, "Espacio no encontrado"));
      }

      _espacios.Remove(espacio);

      return Task.FromResult(new Empty());
    }
  }
}
