using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Commands.Abstractions;

/// <summary>
/// Handles commands of type <typeparamref name="TCommand"/>.
/// </summary>
/// <typeparam name="TCommand">Type of command being handled.</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ICommandResponse>
    where TCommand : ICommand
{
}

/// <summary>
/// Handles commands of type <typeparamref name="TCommand"/> that return data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TCommand">Type of command being handled.</typeparam>
/// <typeparam name="TData">Type of response data.</typeparam>
public interface ICommandHandler<in TCommand, TData> : IRequestHandler<TCommand, ICommandResponse<TData>>
    where TCommand : ICommand<TData>
{
}
