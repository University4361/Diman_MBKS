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

namespace Pick_lab2
{
    /// <summary>
    /// Логика взаимодействия для RightAccessWindow.xaml
    /// </summary>
    public partial class RightAccessWindow : Window
    {
        MyUser _fromUser;
        MyUser _toUser;

        public RightAccessWindow()
        {
            InitializeComponent();
            InitializeStackPanels();
            SetupEvents();
            InitializeObjects();
        }


        private void SetupEvents()
        {
            foreach (var item in FromSP.Children)
            {
                if (item is RadioButton radio)
                {
                    radio.Checked += Radio_Check;
                }
            }

            foreach (var item in ToSP.Children)
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

            if (radio.GroupName == "From")
            {
                _fromUser = UserHelper.AccessUsers[(string)radio.Content];
                UpdateRules(_fromUser);
            }
            else if (radio.GroupName == "To")
            {
                _toUser = UserHelper.AccessUsers[(string)radio.Content];
            }
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
                RadioButton fromRadioButton = new RadioButton()
                {
                    GroupName = "From",
                    Content = key
                };

                RadioButton toRadioButton = new RadioButton()
                {
                    GroupName = "To",
                    Content = key
                };

                FromSP.Children.Add(fromRadioButton);
                ToSP.Children.Add(toRadioButton);
            }
        }

        private void GrantButton_Click(object sender, RoutedEventArgs e)
        {
            if (_fromUser == null || _toUser == null)
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

            UserHelper.GrantRules(_toUser, objects);

            Close();
        }
    }
}
