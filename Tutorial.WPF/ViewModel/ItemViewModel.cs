using Binance.Net;
using System;
using System.Collections.ObjectModel;
using Binance.Net.Objects.Spot.MarketStream;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Tutorial.WPF.ViewModel
{
    public class ItemViewModel : BaseViewModel
    {
        readonly Guid _id;
        public Guid ID { get => _id; }

        readonly string _crypoName;

        public string CrypoName { get => _crypoName; }

        public List<BinanceStreamBookPrice> Data { get; set; } = new List<BinanceStreamBookPrice>();

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
