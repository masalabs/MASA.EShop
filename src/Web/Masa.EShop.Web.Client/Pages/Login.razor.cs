namespace Masa.EShop.Web.Client.Pages;

public partial class Login : EShopPageBase
{
    private string _userName = "masa";
    private string _password = "eshop";
    private bool _forgetPwdDialog;
    private StringNumber _forgetPwdTabIndex = 0;
    private bool _showPwd = false;

    private async void LoginHandler()
    {
        if (_userName.Equals("masa") && _password.Equals("eshop"))
        {
            await ProtectedSessionStore.SetAsync("user", _userName);
            Navigation("/catalog");
        }
        else
        {
            await MessageAsync("UserName Or Password Invalid");
        }
    }
}

