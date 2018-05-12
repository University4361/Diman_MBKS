using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace lab_5
{
    public class MyUser : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private List<int> _ids;

        [JsonIgnore]
        public ObservableCollection<AccessRole> AllRoles;

        private int _userId;
        public int UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserId"));
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        [JsonIgnore]
        public string CurrentRolesNames
        {
            get
            {
                string result = string.Empty;

                foreach (var role in CurrentRoles)
                    result += role.RoleName + ", ";

                return result.TrimEnd(' ', ',');
            }
        }


        private string _currentRolesIds;

        [JsonIgnore]
        public string CurrentRolesIds
        {
            get
            {
                if (string.IsNullOrEmpty(_currentRolesIds))
                {
                    _currentRolesIds = string.Empty;

                    foreach (var item in CurrentRoles)
                        _currentRolesIds += item.RoleID + ",";
                    
                }
                return _currentRolesIds.TrimEnd(' ', ',');
            }
            set
            {
                if (value != null && CheckNewRoles(value.TrimEnd(' ', ',')))
                    _currentRolesIds = value.TrimEnd(' ', ',');

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRolesIds"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRolesNames"));
            }
        }

        private ObservableCollection<AccessRole> _currentRoles;
        public ObservableCollection<AccessRole> CurrentRoles
        {
            get
            {
                if (AllRoles != null && AllRoles.Any())
                {
                    List<int> ids = _currentRoles.Select(role => role.RoleID).ToList();

                    _currentRoles.Clear();

                    foreach (var id in ids)
                        _currentRoles.Add(AllRoles.FirstOrDefault(role => role.RoleID == id));
                }

                return _currentRoles;
            }
            set
            {
                _currentRoles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRoles"));
            }
        }

        public MyUser(int id, string name, string currentRolesIds, ObservableCollection<AccessRole> roles)
        {
            UserId = id;
            UserName = name;
            AllRoles = roles;
            CurrentRolesIds = currentRolesIds;
        }

        [JsonConstructor]
        public MyUser(int id, string name, ObservableCollection<AccessRole> roles)
        {
            UserId = id;
            UserName = name;
            CurrentRoles = roles;
        }

        private bool CheckNewRoles(string value)
        {
            List<int> list = new List<int>();

            foreach (var idStr in value.Split(','))
            {
                if (int.TryParse(idStr, out int id))
                {
                    if (!AllRoles.Select(rule => rule.RoleID).Contains(id))
                        return false;
                    else
                        list.Add(id);
                }
                else
                    return false;
            }

            _ids = list;

            CurrentRoles = new ObservableCollection<AccessRole>();

            foreach (var id in _ids)
                CurrentRoles.Add(AllRoles.FirstOrDefault(role => role.RoleID == id));

            return true;
        }
    }
}
