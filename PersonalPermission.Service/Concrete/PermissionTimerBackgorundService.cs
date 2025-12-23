using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalPermission.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Service.Concrete
{
    public class PermissionTimerBackgorundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionTimerBackgorundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var calculatingPermision = scope.ServiceProvider.GetRequiredService<ICalculatingPermision>();
                    calculatingPermision.UpdateYearlyAndAdministrivePermision();
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Günde 1 kez kontrol et
            }
        }
    }
}
