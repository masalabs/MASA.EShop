namespace MASA.EShop.Web.Client.Pages;

public partial class Login : EShopBasePage
{
    private string _userName = "masa";
    private string _password = "eshop";

    private async void LoginHandler()
    {
        if (_userName.Equals("masa") && _password.Equals("eshop"))
        {
            await ProtectedSessionStore.SetAsync("user", _userName);
            Navigation("/");
        }
        else
        {
            Message("UserName Or Password Invalid");
        }
    }
}

