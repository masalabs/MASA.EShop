using PageBase = MASA.Blazor.PageBase;

namespace MASA.EShop.Web.Client.Pages;

public delegate string Localizer(string text);

public class EShopPageBase : PageBase
{
    private Localizer _localizer = default!;

    protected Localizer T
    {
        get
        {
            if (_localizer == null)
            {
                _localizer = (key) =>
                {
                    return I18n.T(key);
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

