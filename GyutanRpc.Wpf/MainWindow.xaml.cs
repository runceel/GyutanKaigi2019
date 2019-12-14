using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GyutanRpc.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GrpcChannel _channel = GrpcChannel.ForAddress("https://localhost:5001");
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new GyutanRpc.Greeter.GreeterClient(_channel);
            var helloReply = await client.SayHelloAsync(new HelloRequest { Name = textBox.Text });
            MessageBox.Show(helloReply.Message);
        }
    }
}
