using System.Windows;
using System.Windows.Controls;

namespace Pick_lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _elements;
        private string _currentCode;
        private MyUser _currentAccessUser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetupUser(DockPanel panel)
        {
            _elements = string.Empty;
            _currentCode = string.Empty;

            foreach (var view in panel.Children)
            {
                if (view is StackPanel stack)
                {
                    foreach (var item in stack.Children)
                    {
                        if (item is CheckBox check)
                        {
                            _currentCode += check.IsChecked ?? false ? 1 : 0;
                        }
                        else if (item is Label label)
                        {
                            _elements += label.Content;
                        }
                    }
                }
            }

            _currentAccessUser = new MyUser(_currentCode, _elements);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstRB.IsChecked ?? false)
                SetupUser(Dock1);
            if (SecondRB.IsChecked ?? false)
                SetupUser(Dock2);
            if (ThridRB.IsChecked ?? false)
                SetupUser(Dock3);

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void InputTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstRB.IsChecked ?? false)
                SetupUser(Dock1);
            if (SecondRB.IsChecked ?? false)
                SetupUser(Dock2);
            if (ThridRB.IsChecked ?? false)
                SetupUser(Dock3);

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }
    }
}
