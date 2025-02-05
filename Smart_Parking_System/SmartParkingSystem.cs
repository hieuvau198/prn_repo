using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Parking_System
{
    public class SmartParkingSystem
    {
        // Existing methods
        public async Task CheckTicketAsync(string carNumber)
        {
            Console.WriteLine($"[Car {carNumber}] Checking ticket...");
            await Task.Delay(2000); // Simulating processing time
            Console.WriteLine($"[Car {carNumber}] Ticket is valid!");
        }

        public async Task OpenEntryBarrierAsync(string carNumber)
        {
            Console.WriteLine($"[Car {carNumber}] Opening entry barrier...");
            await Task.Delay(1500);
            Console.WriteLine($"[Car {carNumber}] Entry barrier opened.");
        }

        // New implementations
        public async Task ParkCarAsync(string carNumber)
        {
            Console.WriteLine($"[Car {carNumber}] Searching for a parking spot...");
            await Task.Delay(3000); // Simulating time to find a spot and park
            Console.WriteLine($"[Car {carNumber}] Found a spot and parked.");
        }

        public async Task ProcessPaymentAsync(string carNumber)
        {
            Console.WriteLine($"[Car {carNumber}] Processing payment...");
            await Task.Delay(2500); // Simulating payment processing
            Console.WriteLine($"[Car {carNumber}] Payment successful!");
        }

        public async Task OpenExitBarrierAsync(string carNumber)
        {
            Console.WriteLine($"[Car {carNumber}] Opening exit barrier...");
            await Task.Delay(1500); // Simulating barrier opening
            Console.WriteLine($"[Car {carNumber}] Exit barrier opened.");
        }

        public async Task UpdateDatabaseAsync(string carNumber)
        {
            Console.WriteLine($"[Car {carNumber}] Updating database...");
            await Task.Delay(2000); // Simulating database update
            Console.WriteLine($"[Car {carNumber}] Database updated successfully.");
        }

        // Existing process methods
        public async Task CarEnterAsync(string carNumber)
        {
            await CheckTicketAsync(carNumber);
            await OpenEntryBarrierAsync(carNumber);
            await ParkCarAsync(carNumber);
            Console.WriteLine($"[Car {carNumber}] Successfully parked.\n");
        }

        public async Task CarExitAsync(string carNumber)
        {
            await ProcessPaymentAsync(carNumber);
            await Task.WhenAll(OpenExitBarrierAsync(carNumber), UpdateDatabaseAsync(carNumber));
            Console.WriteLine($"[Car {carNumber}] Successfully exited.\n");
        }
    }
}
