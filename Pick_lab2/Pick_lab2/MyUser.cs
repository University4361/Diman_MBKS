using System.Collections.Generic;
using System.Linq;

namespace Pick_lab2
{
    class MyUser
    {
        private string _template;
        public Dictionary<char, byte> AccessDictionary;

        public MyUser(string accessString, string template)
        {
            _template = template;
            InitializeDictionary(accessString);
        }

        private void InitializeDictionary(string accessString)
        {
            AccessDictionary = new Dictionary<char, byte>();

            for (int i = 0; i < _template.Count(); i++)
            {
                byte accessPoint = 0;

                if (i <= accessString.Length - 1)
                    byte.TryParse(accessString[i].ToString(), out accessPoint);

                if (AccessDictionary.ContainsKey(_template[i]))
                    AccessDictionary[_template[i]] = accessPoint != 1 ? (byte)0 : accessPoint;
                else
                    AccessDictionary.Add(_template[i], accessPoint != 1 ? (byte)0 : accessPoint);
            }
        }
    }
}
