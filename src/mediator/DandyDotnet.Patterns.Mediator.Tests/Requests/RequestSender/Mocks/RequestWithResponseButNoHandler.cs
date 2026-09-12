using DandyMediator;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.RequestSender.Mocks;

internal sealed record RequestWithResponseButNoHandler : IRequest<bool>;