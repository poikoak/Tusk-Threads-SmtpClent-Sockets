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
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConnected = false;
        ServiceChatClient client;
        int ID;
        private const string defaultText = "Enter text...";
        private const string defaultName = "Enter name";
        public MainWindow()
        {
            InitializeComponent();
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);//передаем имя
                tbUserName.IsEnabled = false;
                bConnDicon.Content = "Disconnect";
                isConnected = true;
            }
        }


        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);//передаем айди серверу кого нужно убрать
                client = null;
                tbUserName.IsEnabled = true;
                bConnDicon.Content = "Connect";
                isConnected = false;
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
           else
            {
                ConnectUser();
            }
        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
            
        }


        //когда клиент окна загружен создаем и выделяяем память под сервис чат
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (client!=null)
                {
                    client.SendMSG(tbMessage.Text, ID);
                    tbMessage.Text = string.Empty;
                }
               
            }    
        }

        private void tbMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            tbMessage.Text = tbMessage.Text == defaultText ? string.Empty : tbMessage.Text;
        }

        private void tbMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            tbMessage.Text = tbMessage.Text == string.Empty ? defaultText : tbMessage.Text;
        }

        private void tbUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbUserName.Text = tbUserName.Text == defaultName ? string.Empty : tbUserName.Text;
        }

        private void tbUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            tbUserName.Text = tbUserName.Text == string.Empty ? defaultName : tbUserName.Text;
        }
    }
}
