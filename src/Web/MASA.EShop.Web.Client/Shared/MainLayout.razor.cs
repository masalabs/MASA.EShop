namespace MASA.EShop.Web.Client.Shared;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

public partial class MainLayout
{

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    protected ProtectedSessionStorage ProtectedSessionStore { get; set; }

    public void Navigation(string path)
    {
        NavigationManager.NavigateTo(path, true);
    }

    private async void Logout()
    {
        await ProtectedSessionStore.DeleteAsync("user");
        Navigation("/");
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

