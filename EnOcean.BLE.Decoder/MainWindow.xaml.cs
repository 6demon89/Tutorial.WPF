using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Bluetooth.Advertisement;

namespace EnOcean.BLE.Decoder
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly BluetoothLEAdvertisementWatcher _watcher;
        public ObservableCollection<TelegramRaw> RecievedTelegrams { get; set; } = new();
        public ObservableCollection<TelegramDescriptor> DecodedSelectedTelegram { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _watcher = new BluetoothLEAdvertisementWatcher();
            _watcher.Received += _watcher_Received;
            WatcherStatusTextBlock.Text = _watcher.Status.ToString();
        }

        private void _watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (args.Advertisement.ManufacturerData.Count > 0)
            {
                var buffer = args.Advertisement.ManufacturerData[0].Data;
                var ManufacturerID = args.Advertisement.ManufacturerData[0].CompanyId;
                var Payload = buffer.ToArray();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RecievedTelegrams.Add(new(args.BluetoothAddress, args.RawSignalStrengthInDBm, ManufacturerID, Payload));
                }));
            }
        }

        private void WatcherButton_Click(object sender, RoutedEventArgs e)
        {
            if (_watcher.Status == BluetoothLEAdvertisementWatcherStatus.Started)
                _watcher.Stop();
            else
            {
                var manufacturerDataFilter = new BluetoothLEManufacturerData();
                manufacturerDataFilter.CompanyId = 0x03DA;
                _watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerDataFilter);
                _watcher.ScanningMode = BluetoothLEScanningMode.Active;
                _watcher.Start();
            }
            WatcherStatusTextBlock.Text = _watcher.Status != BluetoothLEAdvertisementWatcherStatus.Stopping ? "Observing" : "Stopped";
            WatcherButton.Content = _watcher.Status != BluetoothLEAdvertisementWatcherStatus.Stopping ? "Stop" : "Start";

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is TelegramRaw telegram)
            {
                DecodedSelectedTelegram.Clear();
                try
                {
                    IEnOceanBLEEncoder encoder = EncoderSelector(telegram.Address);
                    DecodedSelectedTelegram.Add(new("Company", "EnOcean"));
                    DecodedSelectedTelegram.Add(new("Signal Strength", $"{telegram.dBm}dBm"));
                    foreach (var item in encoder.Encode(telegram.Data))
                        DecodedSelectedTelegram.Add(item);
                }
                catch (Exception ex)
                {
                    DecodedSelectedTelegram.Add(new("device has unknown address", ex.Message));
                }
            }
        }

        private IEnOceanBLEEncoder EncoderSelector(ulong address)
        {
            var productID = address >> 32;
            if (productID == 0xE215) return new PTM215Encoder();
            else if (productID == 0xE500) return new STM550BEncoder();
            throw new Exception("Unknown Encoder");
        }
    }
}
