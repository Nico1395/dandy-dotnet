using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DandyDotnet.Samples.OnlineShop.App.Pages.Templates;

public abstract class PageComponentBase : ComponentBase, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    protected bool IsBusy { get; private set; }

    [Inject]
    public required IJSRuntime JsRuntime { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    public virtual void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    protected async Task DoBusyAsync(Func<CancellationToken, Task> func)
    {
        IsBusy = true;

        try
        {
            await func.Invoke(_cancellationTokenSource.Token);
        }
        catch (Exception exception)
        {
            await JsRuntime.InvokeVoidAsync("console.error", exception.ToString());
        }

        IsBusy = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        await DoBusyAsync(InitializePageAsync);
    }

    protected virtual Task InitializePageAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected void NavigateTo(string uri)
    {
        NavigationManager.NavigateTo(uri);
    }
}