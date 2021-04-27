using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using Tutorial.WPF.Model;

namespace Tutorial.WPF.Converters
{
    public class NavigationManager
    {
        public List<NavigationModel> NavigationOptions { get => NavigationNameToUserControl.Keys.ToList(); }

        public UserControl NavigateToModel(NavigationModel model)
        {
            if (model is null) return null;
            if (NavigationNameToUserControl.ContainsKey(model))
                return NavigationNameToUserControl[model].Invoke();
            return null;
        }

        public Dictionary<NavigationModel, Func<UserControl>> NavigationNameToUserControl = new Dictionary<NavigationModel, Func<UserControl>>
        {
            {new NavigationModel("ViewA"), ()=>{ return new View.ViewA(); } },
            {new NavigationModel("ViewA"), ()=>{ return new View.ViewB(); } },
        };

        private static readonly object Instancelock = new object();
        private static NavigationManager instance;
        public static NavigationManager GetInstance
        {
            get
            {
                if (instance is null)
                {
                    lock (Instancelock)
                    {
                        if (instance is null)
                        {
                            instance = new NavigationManager();
                        }
                    }
                }
                return instance;
            }
        }

    }

    public class NavigationConverter : MarkupExtension, IValueConverter
    {
        private static NavigationConverter _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter is null)
            {
                _converter = new NavigationConverter();
            }
            return _converter;
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NavigationModel navigateTo = (NavigationModel)value;
            NavigationManager navigation = NavigationManager.GetInstance;

            if (navigateTo is null) return null;
            if (navigation.NavigationNameToUserControl.ContainsKey(navigateTo))
            {
                return navigation.NavigationNameToUserControl[navigateTo].Invoke();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;

    }

    public class ScaleFactorConverter : MarkupExtension, IValueConverter
    {
        private static ScaleFactorConverter _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter is null)
            {
                _converter = new ScaleFactorConverter();
            }
            return _converter;
        }

        private decimal maxValue = 0;


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value.ToString(), out decimal newVal))
            {
                if (maxValue < newVal)
                    maxValue = newVal;
                return newVal/maxValue;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;

    }
}
