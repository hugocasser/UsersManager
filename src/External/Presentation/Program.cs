using Presentation.Extensions;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication
            .CreateBuilder(args)
            .ConfigureBuilder();
        
        var application = builder
            .Build()
            .ConfigureApplication();

        await application.RunApplicationAsync();
    }
}