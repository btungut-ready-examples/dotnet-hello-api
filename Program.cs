using Microsoft.OpenApi.Models;

namespace HelloAPI;

public class Program
{
    public static readonly Type TypeOfProgram = typeof(Program);
    public const string ApiVersion = "v1";
    public static readonly string ApiName = TypeOfProgram.Namespace.Split('.').FirstOrDefault() ?? throw new InvalidOperationException("Namespace is not defined");

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiName, Version = ApiVersion });
        });


        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ApiName} {ApiVersion}");
        });
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
