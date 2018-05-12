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
using System.Management;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab_5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<DirectoryObject> _allDirectories;

        public ObservableCollection<DirectoryObject> ListOfDirectories { get; set; }

        public ObservableCollection<AccessRule> Rules { get; set; }

        public ObservableCollection<AccessRole> Roles { get; set; }

        public ObservableCollection<MyUser> Users { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Closed += MainWindow_Closed;

            string dirsString = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentFoldersPath))
                dirsString = File.ReadAllText(DirectoryHelper.CurrentFoldersPath);

            string usersString = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentMyUsersPath))
                usersString = File.ReadAllText(DirectoryHelper.CurrentMyUsersPath);

            string rolesString = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentRolesPath))
                rolesString = File.ReadAllText(DirectoryHelper.CurrentRolesPath);

            string rulesPath = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentRulesPath))
                rulesPath = File.ReadAllText(DirectoryHelper.CurrentRulesPath);

            List<AccessRule> rules = JsonConvert.DeserializeObject<List<AccessRule>>(rulesPath);

            if (rules != null && rules.Any())
            {
                Rules = new ObservableCollection<AccessRule>(rules);
            }
            else
                Rules = new ObservableCollection<AccessRule>
                {
                    new AccessRule(0, "По умолчанию"),
                    new AccessRule(1, "Не секретно"),
                    new AccessRule(2, "Секретно"),
                    new AccessRule(3, "Очень секретно"),
                };

            _allDirectories = JsonConvert.DeserializeObject<List<DirectoryObject>>(dirsString);

            if (_allDirectories == null)
                _allDirectories = new List<DirectoryObject>();

            if (_allDirectories.Any())
            {
                foreach (var item in _allDirectories)
                    item.AccessRule = Rules.FirstOrDefault(rule => rule.AccessID == item.AccessRule.AccessID);

                ListOfDirectories = new ObservableCollection<DirectoryObject>(_allDirectories);
            }
            else
                ListOfDirectories = new ObservableCollection<DirectoryObject>();

            List<AccessRole> roles = JsonConvert.DeserializeObject<List<AccessRole>>(rolesString);

            if (roles != null && roles.Any())
            {
                foreach (var item in roles)
                    item.AllRules = Rules;

                Roles = new ObservableCollection<AccessRole>(roles);
            }
            else
                Roles = new ObservableCollection<AccessRole>
                {
                    new AccessRole(0, "По умолчанию", "0, 1", Rules),
                    new AccessRole(1, "Тест 1", "1, 2", Rules),
                    new AccessRole(2, "Тест 2", "2, 3", Rules),
                };

            List<MyUser> users = JsonConvert.DeserializeObject<List<MyUser>>(usersString);

            if (users != null && users.Any())
            {
                foreach (var item in users)
                    item.AllRoles = Roles;

                Users = new ObservableCollection<MyUser>(users);
            }
            else
                Users = new ObservableCollection<MyUser>();

            MainDataGrid.ItemsSource = ListOfDirectories;
            RulesDataGrid.ItemsSource = Rules;
            RolesDataGrid.ItemsSource = Roles;
            UsersDataGrid.ItemsSource = Users;

            CBItem.ItemsSource = Rules;

            UsersDataGrid.SelectionChanged += UsersDataGrid_SelectionChanged;
            RulesDataGrid.SelectionChanged += RulesDataGrid_SelectionChanged;
            RolesDataGrid.SelectionChanged += RolesDataGrid_SelectionChanged;
        }

        private void RolesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsersDataGrid.UnselectAll();
        }

        private void RulesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsersDataGrid.UnselectAll();
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyUser selectedUser = UsersDataGrid.SelectedItem as MyUser;

            if (selectedUser == null)
            {
                ListOfDirectories = new ObservableCollection<DirectoryObject>(_allDirectories);

                MainDataGrid.ItemsSource = ListOfDirectories;

                return;
            }

            List<AccessRule> listOfRules = new List<AccessRule>();

            foreach (var myRules in selectedUser.CurrentRoles.Select(role => role.SelectedRules).ToList())
                listOfRules.AddRange(myRules);

            List<DirectoryObject> objects = new List<DirectoryObject>();

            foreach (var dir in _allDirectories)
            {
                if (listOfRules.Select(rule => rule.AccessID).Contains(dir.AccessRule.AccessID))
                    objects.Add(dir);
            }

            ListOfDirectories = new ObservableCollection<DirectoryObject>(objects);
            MainDataGrid.ItemsSource = ListOfDirectories;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            var folders = JsonConvert.SerializeObject(_allDirectories.ToList());
            File.WriteAllText(DirectoryHelper.CurrentFoldersPath, folders);

            var users = JsonConvert.SerializeObject(Users.ToList());
            File.WriteAllText(DirectoryHelper.CurrentMyUsersPath, users);

            var roles = JsonConvert.SerializeObject(Roles.ToList());
            File.WriteAllText(DirectoryHelper.CurrentRolesPath, roles);

            var rules = JsonConvert.SerializeObject(Rules.ToList());
            File.WriteAllText(DirectoryHelper.CurrentRulesPath, rules);
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            UsersDataGrid.UnselectAll();

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

            DirectoryObject directoryObject = new DirectoryObject(Rules.FirstOrDefault(rule => rule.AccessID == 0), resultPath, count);

            _allDirectories.Add(directoryObject);
            ListOfDirectories.Add(directoryObject);
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

            _allDirectories.Add(currentDir);
            ListOfDirectories.Remove(currentDir);
        }

        private void AddRuleClick(object sender, RoutedEventArgs e)
        {
            UsersDataGrid.UnselectAll();

            int id = Rules.Select(rule => rule.AccessID).Max() + 1;
            Rules.Add(new AccessRule(id, $"Новый уровень доступа {id}"));
        }

        private void DeleteRuleClick(object sender, RoutedEventArgs e)
        {
            AccessRule currentRule = RulesDataGrid.SelectedItem as AccessRule;
            Rules.Remove(currentRule);
        }

        private void AddRoleClick(object sender, RoutedEventArgs e)
        {
            UsersDataGrid.UnselectAll();

            int id = Roles.Select(rule => rule.RoleID).Max() + 1;
            Roles.Add(new AccessRole(id, $"Новая роль {id}", "0,1", Rules));
        }

        private void DeleteRoleClick(object sender, RoutedEventArgs e)
        {
            AccessRole currentRole = RolesDataGrid.SelectedItem as AccessRole;
            Roles.Remove(currentRole);
        }

        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            UsersDataGrid.UnselectAll();

            int id = 0;

            if (Users.Any())
                id = Users.Select(rule => rule.UserId).Max() + 1;

            Users.Add(new MyUser(id, $"Новый пользователь {id}", "0", Roles));
        }

        private void DeleteUserClick(object sender, RoutedEventArgs e)
        {
            MyUser currentUser = UsersDataGrid.SelectedItem as MyUser;
            Users.Remove(currentUser);
        }

        private void ClearFocusClick(object sender, RoutedEventArgs e)
        {
            UsersDataGrid.UnselectAll();
        }
    }
}
