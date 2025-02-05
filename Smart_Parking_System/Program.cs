using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Parking_System
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            SmartParkingSystem parking = new SmartParkingSystem();

            Console.WriteLine("Smart Parking System is starting...\n");

            // Simulate a car entering the parking lot
            await parking.CarEnterAsync("A123");

            // Simulate waiting time before exit
            await Task.Delay(5000);

            // Simulate a car exiting the parking lot
            await parking.CarExitAsync("A123");

            Console.WriteLine("Parking system transaction completed!\n");
        }
    }
}
