using System.Reflection;
using Doopass.Options;
using Doopass.Repositories;

namespace Doopass;

public static class Program
{
    private static WebApplicationBuilder? Builder { get; set; }
    private static WebApplication? App { get; set; }
    private static string[]? Args { get; set; }

    public static async Task Main(string[] args)
    {
        InitializeArguments(args);
        InitializeApplicationBuilder();
        ConfigureServices();
        ConfigureApplication();
        await RunApplication();
    }

    private static async Task RunApplication()
    {
        await App!.RunAsync();
    }

    private static void InitializeArguments(string[] args)
    {
        Args = args.ToArray();
    }

    private static void InitializeApplicationBuilder()
    {
        Builder = WebApplication.CreateBuilder(Args!);
    }

    private static void ConfigureServices()
    {
        Builder!.Services.Configure<DbOptions>(Builder.Configuration.GetSection(DbOptions.Position));
        Builder.Services.Configure<InfoOptions>(Builder.Configuration.GetSection(InfoOptions.Position));
        Builder.Services.AddScoped<UsersRepository, UsersRepository>();
        Builder.Services.AddControllers();
        Builder.Services.AddEndpointsApiExplorer();
        Builder.Services.AddSwaggerGen();
        Builder.Logging.ClearProviders();
        Builder.Logging.AddConsole();
        Builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(Program))!)
        );
    }

    private static void ConfigureApplication()
    {
        App = Builder!.Build();

        if (App.Environment.IsDevelopment())
        {
            App.UseSwagger();
            App.UseSwaggerUI();
        }

        App.UseHttpsRedirection();
        App.UseStaticFiles();
        App.UseRouting();
        App.UseAuthorization();
        App.MapControllers();
    }
}