using System;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class SecureTcpClient
{
    private static readonly string serverIp = "127.0.0.1";
    private static readonly int port = 5000;
    private static string userName;

    static void Main()
    {
        Console.Write("Nhập tên của bạn: ");
        userName = Console.ReadLine();

        try
        {
            using (TcpClient client = new TcpClient(serverIp, port))
            using (NetworkStream stream = client.GetStream())
            using (SslStream sslStream = new SslStream(stream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
            {
                sslStream.AuthenticateAsClient(serverIp);
                Console.WriteLine("🔒 Kết nối an toàn đến server.");

                // Gửi tên khi kết nối
                byte[] nameData = Encoding.UTF8.GetBytes(userName);
                sslStream.Write(nameData, 0, nameData.Length);

                // Luồng nhận tin nhắn từ server
                Task.Run(() => ReceiveMessages(sslStream));

                while (true)
                {
                    Console.Write("✍ Nhập tin nhắn: ");
                    string message = Console.ReadLine();
                    if (message.ToLower() == "exit") break;

                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    sslStream.Write(messageBytes, 0, messageBytes.Length);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi client: {ex.Message}");
        }
    }

    static void ReceiveMessages(SslStream sslStream)
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            try
            {
                int bytesRead = sslStream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"\n📩 Tin nhắn: {message}");
                Console.Write("✍ Nhập tin nhắn: "); // Hiển thị lại prompt nhập
            }
            catch
            {
                Console.WriteLine("\n⚠ Mất kết nối với server!");
                break;
            }
        }
    }

    private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}
