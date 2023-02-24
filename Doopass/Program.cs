using Doopass.Options;

namespace Doopass;

public static class Program
{
    private static WebApplicationBuilder? Builder { get; set; }
    private static WebApplication? App { get; set; }
    private static string[]? Args { get; set; }
    
    public static void Main(string[] args)
    {
        InitializeArguments(args);
        InitializeApplicationBuilder();
        ConfigureServices();
        ConfigureApplication();
        RunApplication();        
    }

    private static void RunApplication()
    {
        App!.Run();
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
        Builder.Services.AddControllers();
        Builder.Services.AddEndpointsApiExplorer();
        Builder.Services.AddSwaggerGen();
        Builder.Logging.ClearProviders();
        Builder.Logging.AddConsole();
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
        App.UseAuthorization();
        App.UseRouting();
        App.MapControllerRoute(name: "default", pattern: "{controller=Info}/{action=GetUsageInfo}/{id?}");
    }
}
