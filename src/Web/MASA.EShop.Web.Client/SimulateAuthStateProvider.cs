using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace MASA.EShop.Web.Client
{
    public class SimulateAuthStateProvider : AuthenticationStateProvider
    {
        //https://docs.microsoft.com/zh-cn/aspnet/core/blazor/security/?view=aspnetcore-5.0#expose-the-authentication-state-as-a-cascading-parameter
        //https://code-maze.com/authenticationstateprovider-blazor-webassembly/
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
                }, "Fake authentication type");
            }
            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));

            return authState;
        }
    }
}
