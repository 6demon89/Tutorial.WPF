using Binance.Net;
using Binance.Net.Objects.Spot.MarketStream;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Tutorial.WPF.Service;
using Tutorial.WPF.View;

namespace Tutorial.WPF.ViewModel
{

    public class UpdatebleItem : BaseViewModel
    {
        public double Value { get; set; }
        public double ScaleFactor { get; set; }

        readonly Random _rnd;
        public UpdatebleItem(Random rnd)
        {
            _rnd = rnd;
            Change();
        }

        bool isUpdating = false;
        private async void Change()
        {
            isUpdating = true;
            await Update();
        }

        async Task Update()
        {
            while (true)
            {
                Value = _rnd.Next(0, 500);
                ScaleFactor = Value / 500.0;
                await Dispatcher.CurrentDispatcher?.InvokeAsync(() =>
                {
                    NotifyChanges(nameof(Value));
                    NotifyChanges(nameof(ScaleFactor));
                }, DispatcherPriority.Render);
                await Task.Delay(200);
            }
        }
    }

    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<string> KnownCurrecncies { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<UserControl> CryptoFollow { get; set; } = new ObservableCollection<UserControl>();

        public ObservableCollection<UpdatebleItem> TestCollection { get; set; } = new ObservableCollection<UpdatebleItem>();

        public string UserText { get; set; }
        public ICommand SaveUserTextCommand { get; set; }

        public ICommand AddToTestCommand { get; set; }

        readonly Random rnd;
        public MainWindowViewModel()
        {
            rnd = new Random();
            ViewLoadedCommand = new RelayCommand(AddNewCrypto);
            SaveUserTextCommand = new RelayCommand(SaveUserText);
            AddToTestCommand = new RelayCommand((o) =>
            {
                if (!string.IsNullOrEmpty(UserText))
                    CryptoFollow.Add(new ItemView(UserText));
            });
        }

        private async void SaveUserText(object obj)
        {
            try
            {
                await new LoadingUserDataLService().SaveUserData(new UserModel() { UserName = UserText });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void IncommingSocketDataAction(BinanceStreamBookPrice data)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var _ = KnownCurrecncies.Any(x => x == data.Symbol);
                if (!_)
                    KnownCurrecncies.Add(data.Symbol);
            }, DispatcherPriority.Background);
        }

        private async void AddNewCrypto(object param)
        {
            try
            {
                var userData = await new LoadingUserDataLService().LoadUserData();
                if (userData != null)
                {
                    UserText = userData.UserName;
                    NotifyChanges(nameof(UserText));
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            BinanceSocketClient _client = new BinanceSocketClient();
            _client.Spot.SubscribeToAllBookTickerUpdates(IncommingSocketDataAction);
        }
    }
}
