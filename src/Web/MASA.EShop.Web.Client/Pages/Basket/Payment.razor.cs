namespace MASA.EShop.Web.Client.Pages.Basket;

public partial class Payment : ComponentBase
{
    private int _payOptions = 1;

    [CascadingParameter]
    protected Basket Basket { get; set; } = default!;

    public Localizer T { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        T = Basket.T;
        await base.OnInitializedAsync();
    }
}

