using Microsoft.AspNetCore.SignalR;

namespace MASA.EShop.Services.Ordering
{
    //[Authorize]
    public class NotificationsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            //todo service add inentity
            //await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await Groups.AddToGroupAsync(Context.ConnectionId, "masa");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "masa");
            await base.OnDisconnectedAsync(ex);
        }
    }
}
