using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Commands.Abstractions;

namespace DandyDotnet.Patterns.Mediator.Commands;

internal sealed class CommandResponseBuilder(RequestResponseStatus status) : ICommandResponseBuilder
{
    private readonly Dictionary<string, object> _metadata = [];

    public ICommandResponseBuilder WithMetadata(string key, object value)
    {
        _metadata[key] = value;
        return this;
    }

    public ICommandResponse Build()
    {
        return new CommandResponse(status)
        {
            Metadata = _metadata
        };
    }
}

internal sealed class CommandResponseBuilder<TData>(RequestResponseStatus status, TData? data) : ICommandResponseBuilder<TData>
{
    private readonly Dictionary<string, object> _metadata = [];

    public ICommandResponseBuilder<TData> WithMetadata(string key, object value)
    {
        _metadata[key] = value;
        return this;
    }

    public ICommandResponse<TData> Build()
    {
        return new CommandResponse<TData>(status)
        {
            Data = data,
            Metadata = _metadata
        };
    }
}
