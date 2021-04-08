using Binance.Net;
using Binance.Net.Objects.Spot.MarketStream;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Tutorial.WPF.Service;

namespace Tutorial.WPF.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<string> KnownCurrecncies { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<UserControl> CryptoFollow { get; set; } = new ObservableCollection<UserControl>();

        public string UserText { get; set; }
        public ICommand SaveUserTextCommand { get; set; }

        public MainWindowViewModel()
        {
            ViewLoadedCommand = new RelayCommand(AddNewCrypto);
            SaveUserTextCommand = new RelayCommand(SaveUserText);
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
