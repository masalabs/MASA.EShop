using Masa.Blazor;

namespace Masa.EShop.Web.Client.Pages;

public delegate string Localizer(string text);

public class EShopPageBase : ComponentBase
{
    private Localizer _localizer = default!;

    [Inject]
    protected I18n I18n { get; set; } = default!;

    public Localizer T
    {
        get
        {
            if (_localizer == null)
            {
                _localizer = (key) =>
                {
                    var value = I18n.T(key);
                    return value ?? "";
                };
            }
            return _localizer;
        }
    }

    public bool IsAuthenticated;

    public ClaimsPrincipal User { get; private set; } = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected ProtectedSessionStorage ProtectedSessionStore { get; set; } = default!;

    [Inject]
    protected IOptions<Settings> Settings { get; set; } = default!;

    [Inject]
    protected PopupService PopupService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationStateTask.ContinueWith(task =>
        {
            User = task.Result.User;
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
        });
    }

    public void Navigation(string path)
    {
        NavigationManager.NavigateTo(path, true);
    }

    #region Message

    public async Task MessageAsync(string content, AlertTypes type = default, int timeout = 3000)
    {
        await PopupService.EnqueueSnackbarAsync(content, type, timeout: timeout);
    }

    #endregion
}

