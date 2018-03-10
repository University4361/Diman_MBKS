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
    /// Логика взаимодействия для CreateAccessWindow.xaml
    /// </summary>
    public partial class CreateAccessWindow : Window
    {
        public CreateAccessWindow()
        {
            InitializeComponent();
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            foreach (var obj in UserHelper.AllAccessObjects)
            {
                StackPanel stack = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness { Left = 2, Right = 2, Bottom = 0, Top = 0 },
                    VerticalAlignment = VerticalAlignment.Center
                };

                CheckBox checkBox = new CheckBox()
                {
                    IsChecked = UserHelper.CurrentAccessObjects.Contains(obj)
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

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
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

            UserHelper.CreateSubject(NameTB.Text, objects);

            Close();
        }
    }
}
