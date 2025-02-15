using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class ChatServer
{
    private static TcpListener server;
    private static List<TcpClient> clients = new List<TcpClient>();
    private static readonly string logFilePath = "ChatLog.txt"; // File to store chat logs

    static void Main(string[] args)
    {
        const int port = 12345;
        server = new TcpListener(IPAddress.Parse("192.168.26.69"), port);
        server.Start();
        Console.WriteLine($"Server started on port {port}...");

        // Ensure the log file exists
        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Close();
        }

        // Accept clients in a separate thread
        Thread acceptClientsThread = new Thread(AcceptClients);
        acceptClientsThread.Start();

        // Main thread can handle server-side commands or simply run indefinitely
        Console.WriteLine("Press Enter to stop the server.");
        Console.ReadLine();
        server.Stop();
    }

    private static void AcceptClients()
    {
        while (true)
        {
            try
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("New client connected!");

                // Start a new thread for handling the client
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
            catch (SocketException)
            {
                Console.WriteLine("Server stopped.");
                break;
            }
        }
    }

    private static void HandleClient(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            while (true)
            {
                string message = reader.ReadLine();
                if (message == null) break;

                string timestampedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                Console.WriteLine($"Received: {timestampedMessage}");

                // Save the message to the log file
                SaveMessageToFile(timestampedMessage);

                // Broadcast the message to other clients
                BroadcastMessage(message, client);
            }
        }
        catch (IOException)
        {
            Console.WriteLine("Client disconnected.");
        }
        finally
        {
            clients.Remove(client);
            client.Close();
        }
    }

    private static void BroadcastMessage(string message, TcpClient sender)
    {
        foreach (var client in clients)
        {
            if (client != sender)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(message);
                }
                catch
                {
                    Console.WriteLine("Error broadcasting message.");
                }
            }
        }
    }

    private static void SaveMessageToFile(string message)
    {
        try
        {
            lock (logFilePath) // Ensure thread-safe file writing
            {
                File.AppendAllText(logFilePath, message + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving message to log file: {ex.Message}");
        }
    }
}
