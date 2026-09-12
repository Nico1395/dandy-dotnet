using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Commands.Abstractions;

/// <summary>
/// Response returned by a command.
/// </summary>
public interface ICommandResponse : IRequestResponse
{
}

/// <summary>
/// Response returned by a command containing data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
public interface ICommandResponse<out TData> : ICommandResponse, IRequestResponse<TData>
{
}
