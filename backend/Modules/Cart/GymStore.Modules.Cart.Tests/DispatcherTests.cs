using GymStore.BuildingBlocks.Cqrs;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Cart.Tests;

public class DispatcherTests
{
    [Fact]
    public async Task Dispatcher_RoutesQueryToRegisteredHandler()
    {
        var services = new ServiceCollection();
        services.AddCqrs();
        services.AddHandlersFromAssembly(typeof(DispatcherTests).Assembly);
        await using var provider = services.BuildServiceProvider();

        var dispatcher = provider.GetRequiredService<IDispatcher>();
        var result = await dispatcher.Query(new PingQuery("hello"), CancellationToken.None);

        Assert.Equal("hello:pong", result);
    }

    [Fact]
    public async Task Dispatcher_RoutesCommandToRegisteredHandler()
    {
        var services = new ServiceCollection();
        services.AddCqrs();
        services.AddHandlersFromAssembly(typeof(DispatcherTests).Assembly);
        await using var provider = services.BuildServiceProvider();

        var dispatcher = provider.GetRequiredService<IDispatcher>();
        var result = await dispatcher.Send(new EchoCommand(21), CancellationToken.None);

        Assert.Equal(42, result);
    }

    internal sealed record PingQuery(string Value) : IQuery<string>;

    // Handler is internal on purpose: proves the dispatcher can invoke non-public handlers
    // (the scenario that broke with dynamic dispatch across assembly boundaries).
    internal sealed class PingQueryHandler : IQueryHandler<PingQuery, string>
    {
        public Task<string> Handle(PingQuery query, CancellationToken ct) =>
            Task.FromResult($"{query.Value}:pong");
    }

    internal sealed record EchoCommand(int Value) : ICommand<int>;

    internal sealed class EchoCommandHandler : ICommandHandler<EchoCommand, int>
    {
        public Task<int> Handle(EchoCommand command, CancellationToken ct) =>
            Task.FromResult(command.Value * 2);
    }
}
