using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

class SmartParkingSystem
{
    // Semaphore for controlling entry and exit gates (max 1 car at a time for each gate)
    private SemaphoreSlim entryGate1 = new SemaphoreSlim(1, 1);
    private SemaphoreSlim entryGate2 = new SemaphoreSlim(1, 1);
    private SemaphoreSlim exitGate1 = new SemaphoreSlim(1, 1);
    private SemaphoreSlim exitGate2 = new SemaphoreSlim(1, 1);

    private Dictionary<string, DateTime> carEntryTimes = new Dictionary<string, DateTime>();
    private Dictionary<string, DateTime> carExitTimes = new Dictionary<string, DateTime>();

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

    public async Task OpenEntryBarrierAsync(string carNumber, SemaphoreSlim gate)
    {
        await gate.WaitAsync(); // Ensure only 1 car at a time for each entry gate
        try
        {
            PrintThreadInfo("OpenEntryBarrier");
            Console.WriteLine($"[Car {carNumber}] Opening entry barrier...");
            await Task.Delay(1500);
            Console.WriteLine($"[Car {carNumber}] Entry barrier opened.");
        }
        finally
        {
            gate.Release(); // Release the semaphore after the car enters
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

    public async Task OpenExitBarrierAsync(string carNumber, SemaphoreSlim gate)
    {
        await gate.WaitAsync(); // Ensure only 1 car at a time for each exit gate
        try
        {
            PrintThreadInfo("OpenExitBarrier");
            Console.WriteLine($"[Car {carNumber}] Opening exit barrier...");
            await Task.Delay(1500);
            Console.WriteLine($"[Car {carNumber}] Exit barrier opened.");
        }
        finally
        {
            gate.Release(); // Release the semaphore after the car exits
        }
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
        DateTime entryStartTime = DateTime.Now;
        carEntryTimes[carNumber] = entryStartTime; // Record entry time

        PrintThreadInfo("CarEnter");
        await CheckTicketAsync(carNumber);
        // Simulate entry gates (using either gate 1 or gate 2)
        await Task.WhenAny(
            OpenEntryBarrierAsync(carNumber, entryGate1),
            OpenEntryBarrierAsync(carNumber, entryGate2)
        );
        await ParkCarAsync(carNumber);

        DateTime entryEndTime = DateTime.Now;
        TimeSpan entryWaitTime = entryEndTime - entryStartTime;
        Console.WriteLine($"[Car {carNumber}] Total wait time to enter: {entryWaitTime.TotalSeconds} seconds.\n");
    }

    public async Task CarExitAsync(string carNumber)
    {
        DateTime exitStartTime = DateTime.Now;
        carExitTimes[carNumber] = exitStartTime; // Record exit time

        PrintThreadInfo("CarExit");
        await ProcessPaymentAsync(carNumber);
        // Simulate exit gates (using either gate 1 or gate 2)
        await Task.WhenAny(
            OpenExitBarrierAsync(carNumber, exitGate1),
            OpenExitBarrierAsync(carNumber, exitGate2)
        );
        await UpdateDatabaseAsync(carNumber);

        DateTime exitEndTime = DateTime.Now;
        TimeSpan exitWaitTime = exitEndTime - exitStartTime;
        Console.WriteLine($"[Car {carNumber}] Total wait time to exit: {exitWaitTime.TotalSeconds} seconds.\n");
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
        await parking.CarEnterAsync("A124");
        await parking.CarEnterAsync("A125");
        await parking.CarEnterAsync("A126");
        await parking.CarEnterAsync("A127");
        await parking.CarEnterAsync("B456");
        await parking.CarEnterAsync("B457");
        await parking.CarEnterAsync("B458");
        await parking.CarEnterAsync("B459");
        await parking.CarEnterAsync("B450");

        // Simulate waiting time before exit
        await Task.Delay(5000);

        // Simulate a car exiting the parking lot
        await parking.CarExitAsync("A123");

        Console.WriteLine("Parking system transaction completed!\n");
    }
}
