using Microsoft.AspNetCore.Components;

namespace MASA.EShop.Web.Client.Shared
{
    public partial class MainLayout
    {

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public void Navigation(string path)
        {
            NavigationManager.NavigateTo(path, true);
        }

    }
}
