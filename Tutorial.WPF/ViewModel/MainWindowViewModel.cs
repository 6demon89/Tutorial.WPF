using Binance.Net;
using Binance.Net.Objects.Spot;
using Binance.Net.Objects.Spot.MarketStream;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tutorial.WPF.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand AddCryptoCommand { get; set; }
        public string NewCryptoName { get; set; } = string.Empty;
        //Symbols
        public ObservableCollection<BinanceStreamBookPrice> Symbols { get; set; } = new ObservableCollection<BinanceStreamBookPrice>();
        public ObservableCollection<UserControl> CryptoFollow { get; set; } = new ObservableCollection<UserControl>();

        public MainWindowViewModel()
        {
            AddCryptoCommand = new RelayCommand(AddNewCrypto);
        }

        private void s(BinanceStreamBookPrice data)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var _ = Symbols.FirstOrDefault(x => x.Symbol == data.Symbol);
                if (_ is null)
                    Symbols.Add(data);
            }, DispatcherPriority.Normal);
        }

        private void AddNewCrypto(object param)
        {
            BinanceSocketClient _client = new BinanceSocketClient();
            _client.Spot.SubscribeToAllBookTickerUpdates(s);
        }
    }
}
