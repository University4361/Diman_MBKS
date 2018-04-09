using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class DirectoryObject : INotifyPropertyChanged
    {
        public string AccessFileName { get; set; }

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
            }
        }

        private LabAccessRule _accessRule;
        public LabAccessRule AccessRule
        {
            get
            {
                return _accessRule;
            }
            set
            {
                _accessRule = value;

                SetUpDirectoryRules();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccessRule"));
            }
        }

        private string _path;
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;

                if (string.IsNullOrEmpty(AccessFileName))
                    AccessFileName = System.IO.Path.Combine(_path, Guid.NewGuid().ToString() + ".txt");

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Path"));
            }
        }

        private int _filesCount;
        public int FilesCount
        {
            get

            {
                return _filesCount;
            }
            set
            {
                _filesCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FilesCount"));
            }
        }

        public DirectoryObject(LabAccessRule accessRule, string path, int filesCount)
        {
            Path = path;
            FilesCount = filesCount;
            AccessRule = accessRule;

            Id = DirectoryHelper.GenerateID();
        }

        [JsonConstructor]
        public DirectoryObject(int id, string accessFileName, LabAccessRule accessRule, string path, int filesCount)
        {
            Id = id;
            AccessFileName = accessFileName;
            Path = path;
            FilesCount = filesCount;
            AccessRule = accessRule;
        }

        private void SetUpDirectoryRules()
        {
            if (Directory.Exists(Path))
                File.WriteAllText(AccessFileName, AccessRule.AccessID.ToString());
        }

        public int GetDirectoryRule()
        {
            return int.Parse(File.ReadAllText(AccessFileName));
        }

        public void Dispose()
        {
            File.Delete(AccessFileName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
