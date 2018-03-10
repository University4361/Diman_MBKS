using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Pick_lab2
{
    public static class UserHelper
    {
        public static string CurrentAccessObjects { get; set; }
        public const string AllAccessObjects = "abcdefghijklmnopqrstuvwxyz";

        public static Dictionary<string, MyUser> AccessUsers { get; set; }

        public static void RemoveRules(MyUser user, string objects)
        {
            foreach (var obj in objects)
                user.AccessDictionary[obj] = 0;
        }

        public static void GrantRules(MyUser user, string objects)
        {
            foreach (var obj in objects)
                user.AccessDictionary[obj] = 1;
        }

        public static void CreateSubject(string text, string objects)
        {
            string newObjects = string.Empty;

            foreach (char obj in objects)
            {
                if (!CurrentAccessObjects.Contains(obj.ToString()))
                {
                    foreach (var user in AccessUsers.Values)
                    {
                        user.AccessDictionary.Add(obj, 0);
                        newObjects += obj;
                    }
                }
            }

            foreach (char obj in CurrentAccessObjects)
            {
                if (!objects.Contains(obj.ToString()))
                {
                    foreach (var user in AccessUsers.Values)
                        user.AccessDictionary.Remove(obj);
                }
            }

            string accessString = string.Empty;

            foreach (char obj in objects)
            {
                accessString += newObjects.Contains(obj.ToString()) ? "1" : "0";
            }

            CurrentAccessObjects = objects;
            AccessUsers.Add(text, new MyUser(text, accessString, objects));
        }

        public static void SetupUser(DockPanel panel, out MyUser myUser)
        {
            string name = string.Empty;
            string elements = string.Empty;
            string currentCode = string.Empty;

            foreach (var view in panel.Children)
            {
                if (view is StackPanel stack)
                {
                    foreach (var item in stack.Children)
                    {
                        if (item is CheckBox check)
                            currentCode += check.IsChecked ?? false ? 1 : 0;
                        else if (item is Label label)
                            elements += label.Content;
                    }
                }
                else if (view is RadioButton radio)
                    name = radio.Content as string;
            }

            CurrentAccessObjects = elements;
            myUser = new MyUser(name, currentCode, elements);
        }

        public static void SetupDock(MyUser myUser, string groupName, out DockPanel panel, out RadioButton radio)
        {
            panel = new DockPanel
            {
                LastChildFill = true,
                Margin = new System.Windows.Thickness { Bottom = 5, Top = 5 }
            };


            radio = new RadioButton()
            {
                Content = myUser.Name,
                GroupName = groupName,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness { Left = 10, Right = 10 }
            };

            panel.Children.Add(radio);

            foreach (var item in myUser.AccessDictionary)
            {
                StackPanel stack = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new System.Windows.Thickness { Left = 2, Right = 2, Bottom = 0, Top = 0 }
                };

                CheckBox checkBox = new CheckBox()
                {
                    IsChecked = item.Value == 1 ? true : false
                };

                Label label = new Label()
                {
                    Content = item.Key
                };

                stack.Children.Add(checkBox);
                stack.Children.Add(label);

                panel.Children.Add(stack);
            }
        }
    }
}
