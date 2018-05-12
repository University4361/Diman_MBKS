using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab_5
{
    public static class DirectoryHelper
    {
        static int CurrentId;

        public static string CurrentFoldersPath = Path.Combine("C:\\Users\\Public\\Pikin", "data.txt");
        public static string CurrentRolesPath = Path.Combine("C:\\Users\\Public\\Pikin", "roles.txt");
        public static string CurrentRulesPath = Path.Combine("C:\\Users\\Public\\Pikin", "rules.txt");
        public static string CurrentMyUsersPath = Path.Combine("C:\\Users\\Public\\Pikin", "users.txt");


        static DirectoryHelper()
        {
            SetInitialId();
        }

        private static void SetInitialId()
        {
            string dirsString = string.Empty;

            if (File.Exists(CurrentFoldersPath))
                dirsString = File.ReadAllText(CurrentFoldersPath);

            List<DirectoryObject> dirs = JsonConvert.DeserializeObject<List<DirectoryObject>>(dirsString);

            if (dirs != null && dirs.Any())
                CurrentId = dirs.Select(dir => dir.Id).Max();
        }

        public static int GenerateID()
        {
            return ++CurrentId;
        }
    }
}
