using BlazorComponent;
using MASA.Blazor;
using MASA.EShop.Web.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MASA.EShop.Web.Client.Pages
{
    public class EShopBasePage : PageBase
    {
        protected bool _isAuthenticated;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProtectedSessionStorage ProtectedSessionStore { get; set; } = default!;

        [CascadingParameter]
        public MainLayout App { get; set; } = default!;

        public void Navigation(string path)
        {
            NavigationManager.NavigateTo(path, true);
        }
        #region Message

        public void Message(string content, AlertTypes type = default, int timeout = 3000)
            {
            App.Message(content, type, timeout);
        }

        #endregion
    }
}
