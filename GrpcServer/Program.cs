using GrpcServer.Services;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddGrpc();
    builder.Services.AddSingleton<EspaciosServiceImpl>();

    var app = builder.Build();

    app.MapGrpcService<EspaciosServiceImpl>();

    app.Run();
  }
}


