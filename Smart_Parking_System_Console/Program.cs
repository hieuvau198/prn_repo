using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

class SmartParkingSystem
{
    private void PrintThreadInfo(string operation)
    {
        Process currentProcess = Process.GetCurrentProcess();
        ProcessThreadCollection threads = currentProcess.Threads;

        Console.WriteLine($"\nThread Information for: {operation}");
        Console.WriteLine($"Process ID: {currentProcess.Id}");
        Console.WriteLine($"Current Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"Total Threads in Process: {threads.Count}\n");
    }

    public async Task CheckTicketAsync(string carNumber)
    {
        PrintThreadInfo("CheckTicket");
        Console.WriteLine($"[Car {carNumber}] Checking ticket...");
        await Task.Delay(2000);
        Console.WriteLine($"[Car {carNumber}] Ticket is valid!");
    }

    public async Task OpenEntryBarrierAsync(string carNumber)
    {
        PrintThreadInfo("OpenEntryBarrier");
        Console.WriteLine($"[Car {carNumber}] Opening entry barrier...");
        await Task.Delay(1500);
        Console.WriteLine($"[Car {carNumber}] Entry barrier opened.");
    }

    public async Task ParkCarAsync(string carNumber)
    {
        PrintThreadInfo("ParkCar");
        Console.WriteLine($"[Car {carNumber}] Searching for a parking spot...");
        await Task.Delay(3000);
        Console.WriteLine($"[Car {carNumber}] Found a spot and parked.");
    }

    public async Task ProcessPaymentAsync(string carNumber)
    {
        PrintThreadInfo("ProcessPayment");
        Console.WriteLine($"[Car {carNumber}] Processing payment...");
        await Task.Delay(2500);
        Console.WriteLine($"[Car {carNumber}] Payment successful!");
    }

    public async Task OpenExitBarrierAsync(string carNumber)
    {
        PrintThreadInfo("OpenExitBarrier");
        Console.WriteLine($"[Car {carNumber}] Opening exit barrier...");
        await Task.Delay(1500);
        Console.WriteLine($"[Car {carNumber}] Exit barrier opened.");
    }

    public async Task UpdateDatabaseAsync(string carNumber)
    {
        PrintThreadInfo("UpdateDatabase");
        Console.WriteLine($"[Car {carNumber}] Updating database...");
        await Task.Delay(2000);
        Console.WriteLine($"[Car {carNumber}] Database updated successfully.");
    }

    public async Task CarEnterAsync(string carNumber)
    {
        PrintThreadInfo("CarEnter");
        await CheckTicketAsync(carNumber);
        await OpenEntryBarrierAsync(carNumber);
        await ParkCarAsync(carNumber);
        Console.WriteLine($"[Car {carNumber}] Successfully parked.\n");
    }

    public async Task CarExitAsync(string carNumber)
    {
        PrintThreadInfo("CarExit");
        await ProcessPaymentAsync(carNumber);
        await Task.WhenAll(OpenExitBarrierAsync(carNumber), UpdateDatabaseAsync(carNumber));
        Console.WriteLine($"[Car {carNumber}] Successfully exited.\n");
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        SmartParkingSystem parking = new SmartParkingSystem();

        Console.WriteLine("Smart Parking System is starting...\n");

        Process currentProcess = Process.GetCurrentProcess();
        Console.WriteLine($"Initial Process Information:");
        Console.WriteLine($"Process ID: {currentProcess.Id}");
        Console.WriteLine($"Initial Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"Initial Total Threads: {currentProcess.Threads.Count}\n");

        // Simulate a car entering the parking lot
        await parking.CarEnterAsync("A123");

        // Simulate waiting time before exit
        await Task.Delay(5000);

        // Simulate a car exiting the parking lot
        await parking.CarExitAsync("A123");

        Console.WriteLine("Parking system transaction completed!\n");
    }
}