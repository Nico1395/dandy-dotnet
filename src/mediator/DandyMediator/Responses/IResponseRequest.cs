namespace DandyMediator.Responses;

/// <summary>
/// <see cref="IRequest{TResponse}"/> that returns an <see cref="IRequestResponse"/>.
/// </summary>
/// <typeparam name="TResponse">Type of response, implementing the <see cref="IRequestResponse"/> interface.</typeparam>
public interface IResponseRequest<TResponse> : IRequest<TResponse>
    where TResponse : IRequestResponse
{
}
