// Services/AuthorizationMessageHandler.cs
using LucyShared.Services;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly IAuthService _auth;

    public AuthorizationMessageHandler(IAuthService auth) => _auth = auth;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        var token = await _auth.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, ct);
    }
}