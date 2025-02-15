using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

class SecureTcpServer
{
    private static readonly int port = 5000;
    private static Dictionary<int, (SslStream Stream, string Name)> clients = new Dictionary<int, (SslStream, string)>();
    private static int clientCounter = 0;
    private static readonly string logFilePath = "chatlog.txt";

    static void Main()
    {
        try
        {
            X509Certificate2 serverCertificate = new X509Certificate2(@"C:\Windows\System32\server.pfx", "password");
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Server đang chạy trên cổng {port}...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client, serverCertificate));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Lỗi Server: {ex.Message}");
        }
    }

    static void HandleClient(TcpClient client, X509Certificate2 serverCertificate)
    {
        int clientId = ++clientCounter;
        string clientName = $"Client{clientId}";

        try
        {
            using (NetworkStream stream = client.GetStream())
            using (SslStream sslStream = new SslStream(stream, false))
            {
                sslStream.AuthenticateAsServer(serverCertificate, false, System.Security.Authentication.SslProtocols.Tls12, true);

                lock (clients) { clients[clientId] = (sslStream, clientName); } // Thêm client vào danh sách
                Console.WriteLine($" {clientName} đã kết nối!");
                LogMessage($"[SERVER] {clientName} đã kết nối!");

                SendToAll($" {clientName} đã tham gia!");

                byte[] buffer = new byte[1024];
                while (true)
                {
                    int bytesRead = sslStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"{clientName}: {message}");
                    LogMessage($"[{clientName}] {message}");

                    if (message.StartsWith("@")) // Gửi riêng
                    {
                        string[] parts = message.Split(' ', 2);
                        if (parts.Length > 1)
                        {
                            string targetName = parts[0].Substring(1); // Bỏ ký tự '@'
                            SendToClient(targetName, $"{clientName} (private): {parts[1]}");
                        }
                    }
                    else // Gửi tất cả
                    {
                        SendToAll($"{clientName}: {message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi {clientName}: {ex.Message}");
        }
        finally
        {
            lock (clients) { clients.Remove(clientId); }
            Console.WriteLine($"{clientName} đã thoát!");
            SendToAll($"{clientName} đã rời khỏi!");
            LogMessage($"[SERVER] {clientName} đã rời khỏi!");
        }
    }

    static void SendToAll(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        lock (clients)
        {
            foreach (var client in clients.Values)
            {
                try { client.Stream.Write(data, 0, data.Length); } catch { }
            }
        }
    }

    static void SendToClient(string clientName, string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        lock (clients)
        {
            foreach (var client in clients.Values)
            {
                if (client.Name == clientName)
                {
                    try { client.Stream.Write(data, 0, data.Length); } catch { }
                    break;
                }
            }
        }
    }

    static void LogMessage(string message)
    {
        File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
    }
}
