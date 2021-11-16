using PageBase = MASA.Blazor.PageBase;

namespace MASA.EShop.Web.Client.Pages;

public delegate string Localizer(string text, string? page = null);

public class EShopPageBase : PageBase
{
    private Localizer _localizer = default!;

    protected virtual string PageName { get; set; } = "Base";

    [Inject]
    protected I18n I18n { get; set; } = default!;

    protected Localizer T
    {
        get
        {
            if (_localizer == null)
            {
                _localizer = (key, page) =>
                {
                    I18n.GetLang(I18n.CurrentLanguage)[page ?? PageName].TryGetValue(key, out string? value);
                    return value ?? "";
                };
            }
            return _localizer;
        }
    }

    protected bool IsAuthenticated;

    protected ClaimsPrincipal User { get; private set; } = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected ProtectedSessionStorage ProtectedSessionStore { get; set; } = default!;

    [Inject]
    protected IOptions<Settings> Settings { get; set; } = default!;

    [CascadingParameter]
    protected MainLayout App { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationStateTask.ContinueWith(task =>
        {
            User = task.Result.User;
            IsAuthenticated = User.Identity.IsAuthenticated;
        });
    }

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

