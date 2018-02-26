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

namespace Pick_lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _elements = "abcdefghijklmnopqrstuvwxyz1234567890";
        private string _currentCode;
        private MyUser _currentAccessUser;

        public MainWindow()
        {
            InitializeComponent();
            ElementsTB.Text = _elements;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            FirstTB.Text = string.Empty;
            SecondTB.Text = string.Empty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstRB.IsChecked ?? false)
                _currentCode = FirstTB.Text;
            if (SecondRB.IsChecked ?? false)
                _currentCode = SecondTB.Text;

            _currentAccessUser = new MyUser(_currentCode, ElementsTB.Text);

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void InputTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCode) || _currentAccessUser == null)
                return;

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }
    }
}
