using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

class SmartParkingSystem
{
    private SemaphoreSlim entryGate1 = new SemaphoreSlim(1, 1);
    private SemaphoreSlim entryGate2 = new SemaphoreSlim(1, 1);
    private SemaphoreSlim exitGate1 = new SemaphoreSlim(1, 1);
    private SemaphoreSlim exitGate2 = new SemaphoreSlim(1, 1);

    private Dictionary<string, DateTime> carEntryTimes = new Dictionary<string, DateTime>();
    private Dictionary<string, DateTime> carExitTimes = new Dictionary<string, DateTime>();
    private List<string> entryLogs = new List<string>();
    private List<string> exitLogs = new List<string>();

    private void PrintThreadInfo(string operation)
    {
        Console.WriteLine($"\nThread Information for: {operation}");
        Console.WriteLine($"Current Thread ID: {Thread.CurrentThread.ManagedThreadId}\n");
    }

    public async Task CheckTicketAsync(string carNumber)
    {
        PrintThreadInfo("CheckTicket");
        Console.WriteLine($"[Car {carNumber}] Checking ticket...");
        await Task.Delay(1000);
        Console.WriteLine($"[Car {carNumber}] Ticket is valid!");
    }

    public async Task<string> OpenEntryBarrierAsync(string carNumber)
    {
        SemaphoreSlim chosenGate = entryGate1.WaitAsync().IsCompletedSuccessfully ? entryGate1 : entryGate2;
        string gateName = chosenGate == entryGate1 ? "Entry Gate 1" : "Entry Gate 2";

        await chosenGate.WaitAsync(); // Ensure only 1 car at a time for the chosen gate
        try
        {
            PrintThreadInfo("OpenEntryBarrier");
            Console.WriteLine($"[Car {carNumber}] Opening {gateName}...");
            await Task.Delay(1500);
            Console.WriteLine($"[Car {carNumber}] {gateName} barrier opened.");
            return gateName;
        }
        finally
        {
            chosenGate.Release(); // Release the semaphore after the car enters
        }
    }

    public async Task<string> OpenExitBarrierAsync(string carNumber)
    {
        SemaphoreSlim chosenGate = exitGate1.WaitAsync().IsCompletedSuccessfully ? exitGate1 : exitGate2;
        string gateName = chosenGate == exitGate1 ? "Exit Gate 1" : "Exit Gate 2";

        await chosenGate.WaitAsync(); // Ensure only 1 car at a time for the chosen gate
        try
        {
            PrintThreadInfo("OpenExitBarrier");
            Console.WriteLine($"[Car {carNumber}] Opening {gateName}...");
            await Task.Delay(1500);
            Console.WriteLine($"[Car {carNumber}] {gateName} barrier opened.");
            return gateName;
        }
        finally
        {
            chosenGate.Release(); // Release the semaphore after the car exits
        }
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

    public async Task CarEnterAsync(string carNumber)
    {
        DateTime readyToEnter = DateTime.Now;
        Console.WriteLine($"\n[Car {carNumber}] Ready to enter at {readyToEnter:HH:mm:ss}");

        await CheckTicketAsync(carNumber);
        string gateUsed = await OpenEntryBarrierAsync(carNumber);
        await ParkCarAsync(carNumber);

        DateTime entryTime = DateTime.Now;
        carEntryTimes[carNumber] = entryTime;
        TimeSpan entryDuration = entryTime - readyToEnter;

        entryLogs.Add($"[Car {carNumber}] Entered at {entryTime:HH:mm:ss} using {gateUsed}. Total time from ready to entry: {entryDuration.TotalSeconds:F2} seconds.");
        Console.WriteLine(entryLogs[^1] + "\n");
    }

    public async Task CarExitAsync(string carNumber)
    {
        DateTime readyToExit = DateTime.Now;
        Console.WriteLine($"\n[Car {carNumber}] Ready to exit at {readyToExit:HH:mm:ss}");

        await ProcessPaymentAsync(carNumber);
        string gateUsed = await OpenExitBarrierAsync(carNumber);
        await UpdateDatabaseAsync(carNumber);

        DateTime exitTime = DateTime.Now;
        carExitTimes[carNumber] = exitTime;
        TimeSpan exitDuration = exitTime - readyToExit;

        exitLogs.Add($"[Car {carNumber}] Exited at {exitTime:HH:mm:ss} using {gateUsed}. Total time from ready to exit: {exitDuration.TotalSeconds:F2} seconds.");
        Console.WriteLine(exitLogs[^1] + "\n");
    }

    public async Task UpdateDatabaseAsync(string carNumber)
    {
        PrintThreadInfo("UpdateDatabase");
        Console.WriteLine($"[Car {carNumber}] Updating database...");
        await Task.Delay(2000);
        Console.WriteLine($"[Car {carNumber}] Database updated successfully.");
    }

    public void PrintSummaryReport()
    {
        Console.WriteLine("\nSummary Report:");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("Entry Logs:");
        foreach (var log in entryLogs)
        {
            Console.WriteLine(log);
        }

        Console.WriteLine("\nExit Logs:");
        foreach (var log in exitLogs)
        {
            Console.WriteLine(log);
        }
        Console.WriteLine("-------------------------------------------------");
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        SmartParkingSystem parking = new SmartParkingSystem();
        Console.WriteLine("Smart Parking System is starting...\n");

        List<Task> tasks = new List<Task>
        {
            parking.CarEnterAsync("A123"),
            parking.CarEnterAsync("A124"),
            parking.CarEnterAsync("A125"),
            parking.CarEnterAsync("A126"),
            parking.CarExitAsync("A123"),
            parking.CarExitAsync("A124"),
            parking.CarExitAsync("A125"),
            parking.CarExitAsync("A126")
        };

        await Task.WhenAll(tasks);

        parking.PrintSummaryReport();

        Console.WriteLine("Parking system transactions completed!\n");
    }
}
