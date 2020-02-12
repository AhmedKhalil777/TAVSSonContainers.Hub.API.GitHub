using Hub.API.Hubs;
using Hub.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.API.Installers
{
    public class RealTimeInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddSignalR();
            services.AddSignalRCore();
            services.AddSingleton<ChatHub>();
            
        }
    }
}
