using System.Windows;
using System.Windows.Controls;

namespace Pick_lab2
{
    /// <summary>
    /// Логика взаимодействия для RemoveAccessWindow.xaml
    /// </summary>
    public partial class RemoveAccessWindow : Window
    {
        MyUser _fromUser;

        public RemoveAccessWindow()
        {
            InitializeComponent();
            InitializeStackPanels();
            SetupEvents();
            InitializeObjects();
        }

        private void SetupEvents()
        {
            foreach (var item in MainSP.Children)
            {
                if (item is RadioButton radio)
                {
                    radio.Checked += Radio_Check;
                }
            }
        }

        private void Radio_Check(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            _fromUser = UserHelper.AccessUsers[(string)radio.Content];
            UpdateRules(_fromUser);
        }

        private void UpdateRules(MyUser user)
        {
            foreach (var item in RulesDP.Children)
            {
                if (item is StackPanel stack)
                {
                    CheckBox check = new CheckBox();

                    foreach (var stackItem in stack.Children)
                    {
                        if (stackItem is CheckBox st)
                            check = st;
                    }

                    foreach (var stackItem in stack.Children)
                    {
                        if (stackItem is Label label && user.AccessDictionary[(char)label.Content] == 1)
                            check.IsEnabled = true;
                        else
                            check.IsEnabled = false;
                    }
                }
            }
        }

        private void InitializeObjects()
        {
            foreach (var obj in UserHelper.CurrentAccessObjects)
            {
                StackPanel stack = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness { Left = 2, Right = 2, Bottom = 0, Top = 0 }
                };

                CheckBox checkBox = new CheckBox()
                {
                    IsEnabled = false
                };

                Label label = new Label()
                {
                    Content = obj
                };

                stack.Children.Add(checkBox);
                stack.Children.Add(label);

                RulesDP.Children.Add(stack);
            }
        }

        private void InitializeStackPanels()
        {
            foreach (var key in UserHelper.AccessUsers.Keys)
            {
                RadioButton radioButton = new RadioButton()
                {
                    GroupName = "Test",
                    Content = key
                };

                MainSP.Children.Add(radioButton);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_fromUser == null)
                return;

            string objects = string.Empty;

            foreach (var item in RulesDP.Children)
            {
                if (item is StackPanel stack)
                {
                    CheckBox check = new CheckBox();

                    foreach (var stackItem in stack.Children)
                    {
                        if (stackItem is CheckBox st)
                            check = st;
                    }

                    foreach (var stackItem in stack.Children)
                    {
                        if (stackItem is Label label && (check.IsChecked ?? false))
                            objects += label.Content;
                    }
                }
            }

            UserHelper.RemoveRules(_fromUser, objects);

            Close();
        }
    }
}
