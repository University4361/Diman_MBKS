using System;
using System.Linq;
using System.Windows;

namespace Kripta_Hex_Bin_Lav
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string HexToBinary(string hexstring)
        {
            string binarystring = String.Join(String.Empty,
                hexstring.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            return binarystring;
        }

        private double CompareStrings(string str1, string str2)
        {
            double counter = 0;

            for (int i = 0; i < Math.Min(str1.Length, str2.Length); i++)
            {
                if (str1[i] != str2[i])
                    counter++;
            }

            ResultLabel.Content = string.Format("Ответ: {0}/{1} бит", counter, Math.Min(str1.Length, str2.Length));

            return counter / (double)Math.Min(str1.Length, str2.Length);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string bin1 = HexToBinary(InitialTB.Text);
            string bin2 = HexToBinary(ChangedTB.Text);

            double result = CompareStrings(bin1, bin2);

            AnswerTB.Text = result.ToString();
        }
    }
}
