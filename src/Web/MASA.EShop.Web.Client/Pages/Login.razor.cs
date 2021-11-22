namespace MASA.EShop.Web.Client.Pages;

public partial class Login : EShopPageBase
{
    private string _userName = "masa";
    private string _password = "eshop";
    private bool _registerDialog = false, _forgetPwdDialog = false;
    private StringNumber _registerTabIndex = 0, _forgetPwdTabIndex = 0, _loginTabIndex = 0;
    private bool _showPwd = false;

    protected override string PageName { get; set; } = "Login";

    private async void LoginHandler()
    {
        if (_userName.Equals("masa") && _password.Equals("eshop"))
        {
            await ProtectedSessionStore.SetAsync("user", _userName);
            Navigation("/catalog");
        }
        else
        {
            Message("UserName Or Password Invalid");
        }
    }
}

