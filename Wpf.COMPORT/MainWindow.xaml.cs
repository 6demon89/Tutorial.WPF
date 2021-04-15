using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Net;
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

namespace Wpf.COMPORT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Collection Property of avalable virtual ports
        /// </summary>
        public List<string> AvailablePorts { get; set; } = new List<string>();

        /// <summary>
        /// Collection Property of recieved data over virual port
        /// </summary>
        public ObservableCollection<string> InputData { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Currently selected virtual port name (COM port name)
        /// </summary>
        public string SelectedPort 
        { 
            get=>selectedPort;
            set
            {
                selectedPort = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPort)));
            } 
        }
        private string selectedPort;

        /// <summary>
        /// Currently used SerialPort instance.
        /// We need it inorder to manipulate UI elements, based on its value
        /// </summary>
        public SerialPort currentPort { get; set; } 

        public MainWindow()
        {
            InitializeComponent();
            //Connecting this code as datacontext
            DataContext = this;
        }

        /// <summary>
        /// Instance of our SerialPort Manager, which tracks the changes in the Win32_PnP
        /// </summary>
        SerialManager manager;

        /// <summary>
        /// Action is connected in XAML
        /// Raised when the window instance is initialized 
        /// Here you can check both methods of getting the virtual port values
        /// </summary>
        private void Window_Initialized(object sender, EventArgs e)
        {
            manager = new SerialManager();
            manager.ReadersListUpdated += Manager_ReadersListUpdated;
            manager.CheckCurrentConnectedDevice();
            
            
            //SetPorts();
        }

        /// <summary>
        /// Callign SerialPort static method GetPortNames, to get information of all available ports
        /// You can set this into a timer loop, to update the list.
        /// </summary>
        private void SetPorts()
        {
            AvailablePorts.AddRange(SerialPort.GetPortNames());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvailablePorts)));
        }

        /// <summary>
        /// SerialManager notifies us with new list.
        /// We are checking if list contains a single port, in that case we are trying to automaticly connect to it
        /// Checking if the new list does not contain any ports, then we are disconnecting/diselecting currently connected one
        /// </summary>
        /// <param name="sender">SerialManager</param>
        /// <param name="ports">Collection of virtual ports</param>
        private void Manager_ReadersListUpdated(object sender, List<string> ports)
        {
            AvailablePorts = ports;
            if (AvailablePorts.Count == 1)
            {
                SelectedPort = AvailablePorts[0];
                OpenPortForReading();
            }
            if (AvailablePorts.Count == 0)
            {
                SelectedPort = "";
                ClosePortForReading();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPort)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AvalableComPortsList"));
        }

        /// <summary>
        /// Trying to open the selected port, connect to it and subsribe to the datarecieving event
        /// note that we are first delaying the process by 300 milliseconds, this is due to slow GC process
        /// we can get unauthorized access exception, if the port is still busy or is used by another process
        /// </summary>
        private async void OpenPortForReading()
        {
            try
            {
                await Task.Delay(300);
                currentPort = new SerialPort(SelectedPort);
                currentPort.Open();
                currentPort.DataReceived += CurrentPort_DataReceived;
            }
            catch(UnauthorizedAccessException ex) { /*  */ }
            finally
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentPort)));
            }
        }

        /// <summary>
        /// Disconnecting from current port and releasing resources
        /// </summary>
        private void ClosePortForReading()
        {
            if (currentPort is null) return;
            if (currentPort.IsOpen) currentPort.Close();
            currentPort.DataReceived -= CurrentPort_DataReceived;
            currentPort.Dispose();
            currentPort = null;
            GC.SuppressFinalize(this);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentPort)));
        }

        /// <summary>
        /// this is called from UI
        /// Checking if we have instance of current serialport, based on that we either connecting or disconnecting
        /// </summary>
        private void ConntectBTN_Click(object sender, RoutedEventArgs e)
        {
            if (currentPort != null)
                ClosePortForReading();
            else
                OpenPortForReading();
        }

        /// <summary>
        /// Recieved data from serial port
        /// Reading contained new data from buffer,formating it and adding to the input collection
        /// </summary>
        private void CurrentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (currentPort.IsOpen)
            {
                var _ = currentPort.BytesToRead;
                var read = new byte[_];
                currentPort.Read(read, 0, _);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in read)
                    stringBuilder.Append("0x"+item.ToString("x2").ToUpper()+"  ");
                Dispatcher?.Invoke(() =>
                {
                    InputData.Add(stringBuilder.ToString());
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputData)));
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
