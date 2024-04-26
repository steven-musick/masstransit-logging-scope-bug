using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using Contracts;
using Microsoft.Extensions.Logging;

namespace MassTransitLogScope;

public class Worker : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger<Worker> _logger;

    public Worker(IBus bus, ILogger<Worker> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // First, throw an example exception where scope works as expected to verify we do in fact log scopes.
        try
        {
            using var scope = _logger.BeginScope("Working Scope");
            throw new Exception("Failure showing scope working as expected during log.");
        }
        catch(Exception ex) when(LogException(ex))
        {
            // Swallow to publish message next.
        }
        


        await _bus.Publish(new ReproMessage { Value = $"The time is {DateTimeOffset.Now}" }, stoppingToken);
    }

    private bool LogException(Exception ex)
    {
        _logger.LogError(ex, "An error with scope occurred.");
        return true;
    }
}
