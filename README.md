[![](https://img.shields.io/nuget/v/soenneker.extensions.loggerconfiguration.hangfire.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.loggerconfiguration.hangfire/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.loggerconfiguration.hangfire/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.loggerconfiguration.hangfire/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.loggerconfiguration.hangfire.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.loggerconfiguration.hangfire/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.loggerconfiguration.hangfire/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.loggerconfiguration.hangfire/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.LoggerConfiguration.Hangfire
Serilog `LoggerConfiguration` extensions for adding Hangfire job context and log sinks from application configuration.

## Installation

```bash
dotnet add package Soenneker.Extensions.LoggerConfiguration.Hangfire
```

## Usage

```csharp
using Soenneker.Extensions.LoggerConfiguration.Hangfire;

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration.AddHangfire(context.Configuration);
});
```

Enable the integration and choose its minimum level through configuration:

```json
{
  "Hangfire": {
    "Enabled": true
  },
  "Log": {
    "Levels": {
      "Default": "Information"
    }
  }
}
```

When `Hangfire:Enabled` is absent or `false`, `AddHangfire()` leaves the logger configuration unchanged. When enabled, it adds the Hangfire context enricher and an asynchronous Hangfire sink.

The sink's minimum level is read from `Log:Levels:Default`, then the legacy `Log:DefaultLogLevel`, then `Information`. The value is captured when the sink is added; later configuration changes do not update it automatically. Unsupported level names throw `InvalidOperationException` during setup.

This extension only connects Serilog to Hangfire console context. It does not configure Hangfire storage, start a worker server, expose or secure the dashboard, register jobs, or enable Hangfire.Console itself. Complete those steps in the host before expecting job-scoped output.

Call `AddHangfire()` once per `LoggerConfiguration`; repeated calls add duplicate enrichers/sinks and can duplicate output. The default Serilog async wrapper can drop events when its buffer is full rather than block job execution.

Job output can be visible to dashboard users. Avoid logging arguments, credentials, tokens, or personal data unless dashboard access and retention are appropriate for that information.
