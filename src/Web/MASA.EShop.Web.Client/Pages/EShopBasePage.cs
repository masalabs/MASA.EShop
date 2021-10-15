using BlazorComponent;
using MASA.Blazor;
using MASA.Blazor.Presets;
using Microsoft.AspNetCore.Components;

namespace MASA.EShop.Web.Client.Pages
{
    public class EShopBasePage : PageBase
    {
        protected bool _isAuthenticated;

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        public void Navigation(string path)
        {
            NavigationManager.NavigateTo(path, true);
        }
        #region Message

        private Message.Model _message = new();

        public void Message(string content, AlertTypes type = AlertTypes.None, int timeout = 3000)
        {
            _message = new Message.Model
            {
                Visible = true,
                Content = content,
                Timeout = timeout,
                Type = type
            };

            StateHasChanged();
        }

        #endregion
    }
}
