using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Pick_lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyUser _currentAccessUser;
        private Dictionary<string, DockPanel> _panels = new Dictionary<string, DockPanel>();
        private List<RadioButton> _radios = new List<RadioButton>();

        public MainWindow()
        {
            InitializeComponent();

            InitialSetup();
            SetupEvents();
        }

        private void InitialSetup()
        {
            Dictionary<string, MyUser> users = new Dictionary<string, MyUser>();

            foreach (var item in MainStack.Children)
            {
                string name = string.Empty;
                if (item is DockPanel dock)
                {
                    foreach (var dockItem in dock.Children)
                    {
                        if (dockItem is RadioButton radio)
                        {
                            name = (string)radio.Content;
                            _radios.Add(radio);
                        }

                    }
                    _panels.Add(name, dock);
                    UserHelper.SetupUser(dock, out MyUser myUser);
                    users.Add(myUser.Name, myUser);
                }
            }
            
            UserHelper.AccessUsers = users;
        }

        private void SetupEvents()
        {
            foreach (var item in MainStack.Children)
            {
                if (item is DockPanel panel)
                {
                    foreach (var view in panel.Children)
                    {
                        if (view is StackPanel stack)
                        {
                            foreach (var stackItem in stack.Children)
                            {
                                if (stackItem is CheckBox check)
                                {
                                    check.Checked += Check_Checked;
                                    check.Unchecked += Check_Checked;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetupMainStack()
        {
            MainStack.Children.Clear();
            _radios.Clear();
            _panels.Clear();

            Dictionary<string, MyUser> users = UserHelper.AccessUsers;

            foreach (var item in users)
            {
                UserHelper.SetupDock(item.Value, "test", out DockPanel dockPanel, out RadioButton radio);

                if (_panels.ContainsKey(item.Key))
                    _panels[item.Key] = dockPanel;
                else
                    _panels.Add(item.Key, dockPanel);

                _radios.Add(radio);

                MainStack.Children.Add(dockPanel);
            }

            SetupEvents();
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            _currentAccessUser = null;

            Dictionary<string, MyUser> users = new Dictionary<string, MyUser>();

            foreach (var item in MainStack.Children)
            {
                if (item is DockPanel dock)
                {
                    UserHelper.SetupUser(dock, out MyUser myUser);
                    users.Add(myUser.Name, myUser);
                }
            }

            UserHelper.AccessUsers = users;

            foreach (var panel in _panels)
            {
                if (_radios.FirstOrDefault(item => (string)item.Content == panel.Key).IsChecked ?? false)
                    UserHelper.SetupUser(panel.Value, out _currentAccessUser);
            }

            if (_currentAccessUser == null)
                return;

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _currentAccessUser = null;

            foreach (var panel in _panels)
            {
                if (_radios.FirstOrDefault(item => (string)item.Content == panel.Key).IsChecked ?? false)
                    UserHelper.SetupUser(panel.Value, out _currentAccessUser);
            }

            if (_currentAccessUser == null)
                return;

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void InputTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentAccessUser = null;

            foreach (var panel in _panels)
            {
                if (_radios.FirstOrDefault(item => (string)item.Content == panel.Key).IsChecked ?? false)
                    UserHelper.SetupUser(panel.Value, out _currentAccessUser);
            }

            if (_currentAccessUser == null)
                return;

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void GrantButton_Click(object sender, RoutedEventArgs e)
        {
            RightAccessWindow rightAccessWindow = new RightAccessWindow();
            rightAccessWindow.Closed += Window_Closed;
            rightAccessWindow.Show();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveAccessWindow removeAccessWindow = new RemoveAccessWindow();
            removeAccessWindow.Closed += Window_Closed;
            removeAccessWindow.Show();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateAccessWindow createAccessWindow = new CreateAccessWindow();
            createAccessWindow.Closed += Window_Closed;
            createAccessWindow.Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            SetupMainStack();
            OutputTB.Text = string.Empty;
            _currentAccessUser = null;
        }
    }
}
