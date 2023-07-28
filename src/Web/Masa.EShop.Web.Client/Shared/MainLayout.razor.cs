using System.Globalization;

namespace Masa.EShop.Web.Client.Shared;

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
        if (I18n.Culture.Name == "zh-CN")
        {
            changeLanguage = "en-US";
        }
        else
        {
            changeLanguage = "zh-CN";
        }
        I18n.SetCulture(new CultureInfo(changeLanguage));
    }
}

