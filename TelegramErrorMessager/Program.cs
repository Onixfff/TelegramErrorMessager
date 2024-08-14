using DataBasePomelo;
using ErrorBot;
using ErrorBot.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TelegramErrorMessager
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory)
                          .AddJsonFile("appsettings.json")
                          .AddUserSecrets<Program>(optional: true); // Загружаем секреты пользователя
                })

                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    // Получаем строку подключения из конфигурации
                    var connectionString = configuration.GetConnectionString("DataBasePomelo");

                    services.AddDbContext<ErrorsDbContext>(options =>
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

                    services.AddHostedService<TelegramBotBackgroundService>();

                    // Настраиваем TelegramOptions из конфигурации
                    services.Configure<TelegramOptions>(configuration.GetSection(TelegramOptions.Telegram));

                })
                .Build();

            host.Run();
        }
    }
}
