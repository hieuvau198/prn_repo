using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class ProductHub : Hub
    {
        public async Task NotifyProductUpdate()
        {
            await Clients.All.SendAsync("ReceiveProductUpdate");
        }
    }
}
