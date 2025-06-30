using Lucy.Shared.Services;
using LucyShared.Services;
using Microsoft.JSInterop;

public class BrowserTokenStorage : ITokenStorage
{
    private readonly IJSRuntime _js;
    private const string TokenKey = "authToken";

    public BrowserTokenStorage(IJSRuntime js)
    {
        _js = js;
    }

    public ValueTask<string?> GetTokenAsync() =>
        _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);

    public Task SetTokenAsync(string token) =>
        _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token).AsTask();

    public Task RemoveTokenAsync() =>
        _js.InvokeVoidAsync("localStorage.removeItem", TokenKey).AsTask();
}