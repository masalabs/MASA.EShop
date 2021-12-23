namespace MASA.EShop.Web.Client.Shared;

public partial class MainLayout
{

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected ProtectedSessionStorage ProtectedSessionStore { get; set; } = default!;

    [Inject]
    protected I18n I18n { get; set; } = default!;

    public void Navigation(string path)
    {
        NavigationManager.NavigateTo(path, true);
    }

    private async void Logout()
    {
        await ProtectedSessionStore.DeleteAsync("user");
        Navigation("/login");
    }

    private void ChangeLanguage()
    {
        string? changeLanguage;
        if (I18n.CurrentLanguage == "zh-CN")
        {
            changeLanguage = "en-US";
        }
        else
        {
            changeLanguage = "zh-CN";
        }
        I18n.SetLang(changeLanguage);
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

