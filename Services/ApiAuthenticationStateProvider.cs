using LucyShared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _auth;

    public ApiAuthenticationStateProvider(IAuthService auth) => _auth = auth;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _auth.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        // decode JWT (or just trust it and extract claims)
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var claims = jwt.Claims;

        //var identity = new ClaimsIdentity(claims, "jwtAuth");
        var identity = new ClaimsIdentity(
        jwt.Claims,
        authenticationType: "jwtAuth",
        nameType: JwtRegisteredClaimNames.Name,
        roleType: ClaimTypes.Role
    );
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public void NotifyAuthenticationStateChanged() =>
        base.NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}