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

namespace TcpCilentTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mainprocess test124 = new mainprocess();
        }

        private void btnSendLMK6_Click(object sender, RoutedEventArgs e)
        {
            mainprocess.MainQueue.Enqueue(string.Format("LMK6.GetInputIo."));
        }

        private void btnSendMicroshake_Click(object sender, RoutedEventArgs e)
        {
            mainprocess.MainQueue.Enqueue(string.Format("Microshake.GetInputIo."));
        }
    }
}
