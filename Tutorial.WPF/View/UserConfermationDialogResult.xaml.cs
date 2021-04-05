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
using System.Windows.Shapes;

namespace Tutorial.WPF.View
{
    /// <summary>
    /// Interaction logic for UserConfermationDialogResult.xaml
    /// </summary>
    public partial class UserConfermationDialogResult : Window
    {
        readonly string _questionText;
        public UserConfermationDialogResult( string questionText)
        {
            InitializeComponent();
            _questionText = questionText;
            this.QuestionTextBlock.Text = _questionText;
        }

        private void Confim_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Decline_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
