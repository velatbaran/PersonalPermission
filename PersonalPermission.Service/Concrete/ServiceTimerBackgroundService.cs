using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PersonalPermission.Core.Entities;
using PersonalPermission.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Service.Concrete
{
    public class ServiceTimerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceTimerBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var userServiceTime = scope.ServiceProvider.GetRequiredService<ICalculatingServiceTime>();
                    userServiceTime.UpdateServiceTime();

                    //var serviceTimer = new UserServiceTime();

                    //foreach (var user in _serviceUser.GetAll())
                    //{
                    //    var _serviveTime = serviceTimer.CalculatingServiceTime(user.StartingWorkDate);
                    //    user.ServiceTimeDay = _serviveTime.Day;
                    //    user.ServiceTimeMonth = _serviveTime.Month;
                    //    user.ServiceTimeYear = _serviveTime.Year;
                    //    _serviceUser.Update(user);
                    //    await _serviceUser.SaveChangesAsync();
                    //    _logger.LogInformation("Hizmet süresi güncellemesi tamamlandı.");
                    //}
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Günde 1 kez çalışır
            }
        }
    }
}
