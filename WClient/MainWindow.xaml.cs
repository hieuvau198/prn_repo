using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChatClientWPF
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;
        private Thread receiveThread;

        private const string ServerAddress = "192.168.26.69"; // Replace with your server IP
        private const int Port = 12345;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient(ServerAddress, Port);
                NetworkStream stream = client.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);

                // Start a thread to listen for messages
                receiveThread = new Thread(ReceiveMessages)
                {
                    IsBackground = true
                };
                receiveThread.Start();

                AddMessage("Connected to server.", false);
            }
            catch (SocketException)
            {
                AddMessage("Unable to connect to the server.", false);
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    string message = reader.ReadLine();
                    if (message != null)
                    {
                        Dispatcher.Invoke(() => AddMessage($"He/him: {message}", false));
                    }
                }
            }
            catch (IOException)
            {
                Dispatcher.Invoke(() => AddMessage("Disconnected from server.", false));
            }
        }

        public void AddMessage(string message, bool isSentByUser)
        {
            // Create a Border to wrap the TextBlock (message bubble)
            var messageBubble = new Border
            {
                Background = isSentByUser ? Brushes.LightBlue : Brushes.LightGray,
                CornerRadius = new CornerRadius(15),
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                MaxWidth = 250,
                HorizontalAlignment = isSentByUser ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };

            // Create the TextBlock for the message content
            var messageText = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14,
                Foreground = Brushes.Black
            };

            // Add the TextBlock to the Border
            messageBubble.Child = messageText;

            // Create a TextBlock for the timestamp
            var timestamp = new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"), // Use 24-hour time format (e.g., "14:35")
                FontSize = 10,
                Foreground = Brushes.Gray,
                Margin = new Thickness(5, 0, 5, 5),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            // Group the message bubble and timestamp in a StackPanel
            var messageContainer = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = isSentByUser ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };
            messageContainer.Children.Add(messageBubble);
            messageContainer.Children.Add(timestamp);

            // Add the StackPanel to the chat stack
            ChatStackPanel.Children.Add(messageContainer);

            // Scroll to the bottom
            ChatScrollViewer.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                writer.WriteLine(message);
                AddMessage($"You: {message}", true);
                MessageTextBox.Clear();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            writer?.Close();
            reader?.Close();
            client?.Close();
            receiveThread?.Abort();
        }
    }
}
