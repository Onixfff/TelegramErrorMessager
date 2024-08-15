using DataBasePomelo;
using DataBasePomelo.Interface;
using ErrorBot.Options;
using DataBasePomelo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ErrorBot.Serices;
using ErrorBot.Interface;

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
                    var connectionString = configuration.GetConnectionString(nameof(DataBasePomelo));

                    services.AddDbContext<ErrorsDbContext>(options =>
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

                    var userIds = configuration.GetSection("Peoples").Get<List<long>>();

                    if (userIds == null)
                    {
                        throw new InvalidOperationException("Configuration section 'Peoples' is missing or invalid.");
                    }

                    services.AddSingleton(userIds);

                    services.AddTransient<IJsonRead, JsonRead>();
                    services.AddTransient<IMessageUpdate, Message>();
                    services.AddTransient<IMessageNotifier, MessageNotifierService>();

                    services.AddHostedService<MessagePollingBackgroundSerice>();

                    services.AddHostedService<TelegramBotBackgroundService>();

                    // Настраиваем TelegramOptions из конфигурации
                    services.Configure<TelegramOptions>(configuration.GetSection(TelegramOptions.Telegram));

                })
                .Build();

            host.Run();
        }
    }
}
