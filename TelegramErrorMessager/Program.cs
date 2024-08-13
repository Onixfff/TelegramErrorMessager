using DataBasePomelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram;

namespace TelegramErrorMessager
{
    public class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Исправлено на AppContext.BaseDirectory для правильного пути
                .AddJsonFile("appsettings.json")
                .Build();

            // Создаем объект ServiceCollection
            var services = new ServiceCollection();

            // Получаем строку подключения из конфигурации
            var connectionString = configuration.GetConnectionString("DataBasePomelo");
            services.AddHostedService<Worker>();
            services.AddDbContext<ErrorsDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.BuildServiceProvider();

            var serviceProvider = services.BuildServiceProvider();

            // Получаем экземпляр DbContext из ServiceProvider
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ErrorsDbContext>();

                // Ваша основная логика работы с DbContext
                // Например, можно проверить, что контекст работает
                Console.WriteLine("DbContext successfully created and ready to use.");
            }
        }
    }
}
