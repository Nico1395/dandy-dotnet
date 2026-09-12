using DandyMediator.Responses;

namespace DandyMediator.Commands;

/// <summary>
/// A command that returns a response without data.
/// </summary>
public interface ICommand : IResponseRequest<ICommandResponse>
{
}

/// <summary>
/// A command that returns a response containing data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
public interface ICommand<TData> : IResponseRequest<ICommandResponse<TData>>
{
}
