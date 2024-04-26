namespace Company.Consumers;

using System.Threading.Tasks;
using MassTransit;
using Contracts;
using Microsoft.Extensions.Logging;
using System;

public class ReproMessageConsumer :
    IConsumer<ReproMessage>
{
    public ReproMessageConsumer(ILogger<ReproMessageConsumer> logger)
    {
        _logger = logger;
    }

    private readonly ILogger<ReproMessageConsumer> _logger;

    public Task Consume(ConsumeContext<ReproMessage> context)
    {
        using var scope = _logger.BeginScope("Broken Scope");
        throw new Exception("Failure showing scope being lost when logging.");
    }
}