using DandyMediator.Responses;

namespace DandyMediator.Commands;

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
public interface ICommandResponse<TData> : ICommandResponse, IRequestResponse<TData>
{
}
