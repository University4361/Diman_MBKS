using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DirectoryObject> ListOfDirectories { get; set; }

        public ObservableCollection<LabAccessRule> Rules { get; set; }

        public ObservableCollection<int> Ids { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            Closed += MainWindow_Closed;

            string usersString = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentUsersPath))
                usersString = File.ReadAllText(DirectoryHelper.CurrentUsersPath);

            Rules = new ObservableCollection<LabAccessRule>
            {
                new LabAccessRule(3, "Top secret"),
                new LabAccessRule(2, "Secret"),
                new LabAccessRule(1, "Non secret"),
                new LabAccessRule(0, "Default"),
            };

            List<DirectoryObject> dirs = JsonConvert.DeserializeObject<List<DirectoryObject>>(usersString);

            if (dirs != null && dirs.Any())
            {
                foreach (var item in dirs)
                    item.AccessRule = Rules.FirstOrDefault(rule => rule.AccessID == item.AccessRule.AccessID);

                ListOfDirectories = new ObservableCollection<DirectoryObject>(dirs);
            }
            else
                ListOfDirectories = new ObservableCollection<DirectoryObject>();

            Ids = new ObservableCollection<int>(ListOfDirectories.Select(item => item.Id));

            ToCB.ItemsSource = Ids;
            FromCB.ItemsSource = Ids;
            MainDataGrid.ItemsSource = ListOfDirectories;
            CBItem.ItemsSource = Rules;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            var users = JsonConvert.SerializeObject(ListOfDirectories.ToList());
            File.WriteAllText(DirectoryHelper.CurrentUsersPath, users);
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            string resultPath = string.Empty;

            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                    resultPath = dialog.SelectedPath;
                else
                    return;
            }

            if (ListOfDirectories.FirstOrDefault(dir => dir.Path == resultPath) != null)
                return;

            int count = Directory.CreateDirectory(resultPath).GetFiles().Length;

            var newItem = new DirectoryObject(Rules.FirstOrDefault(rule => rule.AccessID == 0), resultPath, count);

            ListOfDirectories.Add(newItem);

            Ids.Add(newItem.Id);
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedIndex < 0)
            {
                System.Windows.MessageBox.Show("Выберите папку, которую хотите удалить");
                return;
            }

            DirectoryObject currentDir = MainDataGrid.SelectedItem as DirectoryObject;
            currentDir.Dispose();

            Ids.Remove(currentDir.Id);

            ToCB.SelectedItem = null;
            FromCB.SelectedItem = null;

            ListOfDirectories.Remove(currentDir);
        }

        private void CopyClick(object sender, RoutedEventArgs e)
        {
            if (FromCB.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Введите идентификатор исходной папки");
                return;
            }

            DirectoryObject currentDir = ListOfDirectories.FirstOrDefault(dir => dir.Id == FromCB.SelectedItem as int?);

            if (ToCB.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Введите идентификатор целевой папки");
                return;
            }

            DirectoryObject toDir = ListOfDirectories.FirstOrDefault(dir => dir.Id == ToCB.SelectedItem as int?);

            if (toDir == null || toDir.Id == currentDir.Id)
            {
                System.Windows.MessageBox.Show("Введите корректное значение идентификатора папки");
                return;
            }

            if (toDir.AccessRule.AccessID < currentDir.AccessRule.AccessID)
            {
                System.Windows.MessageBox.Show("Исходная папка имеет более высокий уровень секретности. Копирование невозможно");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string dirName = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                if (!dirName.Contains(currentDir.Path))
                {
                    System.Windows.MessageBox.Show("Вы выбрали файл, который находится за пределами исходной папки. Выберете другой файл");
                    return;
                }

                try
                {
                    string name = System.IO.Path.GetFileName(openFileDialog.FileName);

                    string newPath = System.IO.Path.Combine(toDir.Path, name);

                    File.WriteAllBytes(newPath, File.ReadAllBytes(openFileDialog.FileName));
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Ошибка");
                    return;
                }

                System.Windows.MessageBox.Show("Файл был успешно скопирован");
            }

        }

    }
}
