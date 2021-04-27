using Binance.Net;
using System;
using Binance.Net.Objects.Spot.MarketStream;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Tutorial.WPF.ViewModel
{
    public class NavigationViewModel: BaseViewModel
    {
        public List<UserControl> NavigationOptions { get; set; }
        public UserControl SelectedItem { get; set; }

    }

    public class ItemViewModel : BaseViewModel
    {
        readonly Guid _id;
        public Guid ID { get => _id; }

        readonly string _crypoName;

        public string CrypoName { get => _crypoName; }

        public ObservableCollection<BinanceStreamBookPrice> Data { get; set; } = new ObservableCollection<BinanceStreamBookPrice>();

        public ItemViewModel(string cryptoname)
        {
            _crypoName = cryptoname;
            BinanceSocketClient _client = new BinanceSocketClient();
            _client.Spot.SubscribeToBookTickerUpdates(_crypoName, data =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    Data.Insert(0, data);
                        NotifyChanges(nameof(Data));
                    }, DispatcherPriority.Normal);
            });
            _id = Guid.NewGuid();
        }
    }
}
