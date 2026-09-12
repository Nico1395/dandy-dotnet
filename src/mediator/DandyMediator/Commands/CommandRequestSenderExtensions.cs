namespace DandyMediator.Commands;

/// <summary>
/// Contains extension methods for sending commands.
/// </summary>
public static class CommandRequestSenderExtensions
{
    /// <summary>
    /// Sends a command without response data.
    /// </summary>
    /// <param name="sender">Request sender.</param>
    /// <param name="command">Command being sent.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The command response.</returns>
    public static Task<ICommandResponse> SendAsync(this IRequestSender sender, ICommand command, CancellationToken cancellationToken = default)
    {
        return sender.SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Sends a command that returns data.
    /// </summary>
    /// <typeparam name="TData">Type of response data.</typeparam>
    /// <param name="sender">Request sender.</param>
    /// <param name="command">Command being sent.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The command response.</returns>
    public static Task<ICommandResponse<TData>> SendAsync<TData>(this IRequestSender sender, ICommand<TData> command, CancellationToken cancellationToken = default)
    {
        return sender.SendAsync(command, cancellationToken);
    }
}
