namespace MASA.EShop.Web.Client;

public class SimulateAuthStateProvider : AuthenticationStateProvider
{
    //https://docs.microsoft.com/zh-cn/aspnet/core/blazor/security/?view=aspnetcore-5.0#expose-the-authentication-state-as-a-cascading-parameter
    //https://code-maze.com/authenticationstateprovider-blazor-webassembly/
    //if Use ProtectedLocalStorage throw System.Security.Cryptography.CryptographicException: The key was not found in the key ring. For more
    private readonly ProtectedSessionStorage _protectedSessionStore;

    public SimulateAuthStateProvider(ProtectedSessionStorage protectedSessionStore)
    {
        _protectedSessionStore = protectedSessionStore;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        var userResult = await _protectedSessionStore.GetAsync<string>("user");
        if (userResult.Success && userResult.Value?.Length > 0)
        {
            identity = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.Name, userResult.Value),
                    new Claim("email", "masa@lonsid.com"),
                    new Claim("address_city", "hang zhou"),
                    new Claim("address_street", "xia sha"),
                    new Claim("address_country", "China")
                }, "Fake authentication type");
        }
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));

        return authState;
    }
}

