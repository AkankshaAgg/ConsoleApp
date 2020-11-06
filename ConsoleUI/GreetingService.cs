using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

//DI, Serilog, Settings

namespace ConsoleUI
{
    public class GreetingService : IGreetingService
    {
        private readonly ILogger<GreetingService> _log;
        private readonly IConfiguration _Config;
        public GreetingService(ILogger<GreetingService> log, IConfiguration config)
        {
            _log = log;
            _Config = config;
        }
        public void Run()
        {
            for (int i = 0; i < _Config.GetValue<int>("LoopTimes"); i++)
            {
                _log.LogInformation("Run number {runNumber}", i);
            }
        }
    }
}
