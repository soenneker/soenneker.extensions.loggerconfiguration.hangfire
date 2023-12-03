using Hangfire.Console.Extensions.Serilog;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Soenneker.Utils.Logger;

namespace Soenneker.Extensions.LoggerConfiguration.Hangfire;

/// <summary>
/// Serilog LoggerConfiguration extension methods related to Hangfire
/// </summary>
public static class HangfireLoggerConfigExtension
{
    /// <summary>
    /// Adds the Hangfire sink unless the config says that we shouldn't
    /// </summary>
    public static void AddHangfire(this Serilog.LoggerConfiguration loggerConfig, IConfigurationRoot configRoot)
    {
        var enabled = configRoot.GetValue<bool>("Hangfire:Enabled");

        if (!enabled)
            return;

        LogEventLevel logEventLevel = LoggerUtil.GetLogEventLevelFromConfigRoot(configRoot);

        loggerConfig.Enrich.WithHangfireContext();
        loggerConfig.WriteTo.Async(a => a.Hangfire(restrictedToMinimumLevel: logEventLevel));
    }
}