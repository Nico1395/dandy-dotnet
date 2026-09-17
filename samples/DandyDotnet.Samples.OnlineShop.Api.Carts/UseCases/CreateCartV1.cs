using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.Mediator.Commands;
using DandyDotnet.Patterns.Mediator.Commands.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Carts.UseCases;

internal static class CreateCartV1
{
    // TODO -> Request when placing order

    private sealed record Command(string UserId) : ICommand<Guid>;

    private sealed class CommandHandler(IEventStore eventStore) : ICommandHandler<Command, Guid>
    {
        public async Task<ICommandResponse<Guid>> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var created = new CartCreatedV1(request.UserId);
            var cart = new Cart(created, DateTime.UtcNow);

            await eventStore.AppendAsync<Cart>(cart.Id.ToString(), created, cancellationToken);
            return CommandResponse.OK_200(cart.Id).Build();
        }
    }
}