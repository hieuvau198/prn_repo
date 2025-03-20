using Microsoft.AspNetCore.SignalR;

namespace Services.Hubs
{
    public class ProductHub : Hub
    {
        public async Task NotifyProductUpdate()
        {
            await Clients.All.SendAsync("ReceiveProductUpdate");
            await Clients.All.SendAsync("ReceiveNotication");
        }
    }
}
