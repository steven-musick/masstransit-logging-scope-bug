using MassTransit;
using MassTransitLogScope;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureLogging((HostBuilderContext context, ILoggingBuilder logging) =>
{
    logging.ClearProviders();
    logging.AddSimpleConsole(console =>
    {
        console.IncludeScopes = true;
        console.UseUtcTimestamp = true;
        console.IncludeScopes = true;
    });
});

builder.ConfigureServices((context, services) =>
{
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        // By default, sagas are in-memory, but should be changed to a durable
        // saga repository.
        x.SetInMemorySagaRepositoryProvider();

        var entryAssembly = Assembly.GetEntryAssembly();

        x.AddConsumers(entryAssembly);
        x.AddSagaStateMachines(entryAssembly);
        x.AddSagas(entryAssembly);
        x.AddActivities(entryAssembly);

        x.UsingInMemory((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
        });
    });

    services.AddHostedService<Worker>();
});

var host = builder.Build();
host.Run();