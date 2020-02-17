using Hub.API.Data;
using Hub.API.Hubs;
using Hub.API.TimerFeatures;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.API.Services
{
    public class ChartsServices : IChartsServices
    {
        private readonly IHubContext<ChartHub> _hub;
        public ChartsServices( IHubContext<ChartHub> hub)
        {
            _hub = hub;
        }
        
        public void GetChart()
        {
            var Timer = new TimerManager(()
                => _hub.Clients.All.SendCoreAsync("TransferCharts", new[] { DataChartSimulationManager.Get() }));
        }
    }
}
