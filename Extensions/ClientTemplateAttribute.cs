using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Smoking.Extensions.Helpers;

namespace Smoking.Extensions
{

    public class AdminMenu
    {
        public static MenuItem CurrentItem { get; set; }
        public static MenuItem MainItem { get; set; }


        private static List<MenuItem> _itemList;
        public static List<MenuItem> ItemList
        {
            get
            {
                if (_itemList == null || _itemList.Count == 0)
                {
                    _itemList = new List<MenuItem>();
                    var controllers = typeof(AdminMenu).Assembly.GetTypes().Where(
                        type => type.IsSubclassOf(typeof(Controller))).ToList();

                    foreach (var controller in controllers)
                    {
                        var methods = controller.GetMethods();
                        MenuItemAttribute token = null;
                        foreach (var method in methods)
                        {
                            token = Attribute.GetCustomAttribute(method,
                                typeof(MenuItemAttribute), false) as MenuItemAttribute;
                            if (token == null)
                                continue;

                            var item = new MenuItem()
                                {
                                    ID = token.ID,
                                    ParentID = token.ParentID,
                                    Name = token.Name,
                                    Icon = token.Icon.IsFilled() ? token.Icon : "home",
                                    Action = method.Name,
                                    URL = string.Format("/Master/{0}/{1}/{2}", AccessHelper.MasterPanel.DefaultLang,
                                                        controller.Name.Replace("Controller", ""), method.Name),
                                    Controller = controller.Name.Replace("Controller",
                                                                         ""),
                                    Path =
                                        string.Format("~/Views/{0}/{1}.cshtml",
                                                      controller.Name.Replace("Controller", ""), method.Name)
                                };

                            if (item.ParentID > 0)
                            {
                                var p = _itemList.FirstOrDefault(x => x.ID == item.ParentID);
                                if (p != null)
                                {
                                    if (p.Children == null)
                                        p.Children = new List<MenuItem>();
                                    item.Parent = p;
                                    p.Children.Add(item);
                                }
                                else
                                {
                                    _itemList.Add(item);
                                }
                            }
                            else
                            {
                                _itemList.Add(item);
                            }




                            var forDel = new List<int>();
                            foreach (var i in _itemList)
                            {
                                if (i.ParentID <= 0) continue;
                                var p = _itemList.FirstOrDefault(x => x.ID == i.ParentID);
                                if (p != null)
                                {
                                    if (p.Children == null)
                                        p.Children = new List<MenuItem>();
                                    i.Parent = p;
                                    p.Children.Add(i);
                                    forDel.Add(i.ID);
                                }
                                i.URL = string.Format("/Master/{0}/{1}/{2}", AccessHelper.MasterPanel.DefaultLang,
                                                      i.Controller, i.Action);

                            }
                            foreach (var i in _itemList)
                            {
                                if (i.ParentID <= 0)
                                {
                                    i.HasChildren = i.Children != null && i.Children.Any();
                                }
                            }
                            _itemList = _itemList.Where(x => !forDel.Contains(x.ID)).OrderBy(x => x.ID).ToList();
                        }
                    }

                    _itemList = _itemList.OrderBy(x => x.ID).ToList();
                    foreach (var item in _itemList.Where(item => item.Children != null))
                    {
                        item.Children = item.Children.OrderBy(x => x.ID).ToList();
                    }

                }
                return _itemList;
            }
        }

        public static bool HasFilter { get; set; }
    }

    public class MenuItem
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int ParentID { get; set; }
        public List<MenuItem> Children { get; set; }
        public MenuItem Parent { get; set; }
        public string URL { get; set; }
        public bool HasChildren { get; set; }
        public string Icon { get; set; }
        public bool IsCurrent
        {
            get
            {
                return this.Controller == AccessHelper.CurrentPageInfo.Controller && this.Action == AccessHelper.CurrentPageInfo.Action;
            }
        }
        public bool IsMain
        {
            get
            {
                return this.Controller == AccessHelper.MasterPanel.DefaultController && this.Action == AccessHelper.MasterPanel.DefaultAction;
            }
        }
    }


    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class MenuItemAttribute : Attribute
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Icon { get; set; }
        public MenuItemAttribute(string name)
        {
            Name = name;
            ID = 0;
            ParentID = 0;
        }
        public MenuItemAttribute(string name, int id)
            : this(name)
        {
            ID = id;
        }
        public MenuItemAttribute(string name, int id, int parentID)
            : this(name, id)
        {
            ParentID = parentID;
        }
    }

    public class ClientTemplate
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string FileContent { get; set; }
        public bool IsModul { get; set; }

    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ClientTemplateAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsModule { get; set; }
        public ClientTemplateAttribute(string name)
        {
            this.Name = name;
            this.IsModule = true;
        }
        public ClientTemplateAttribute(string name, bool isModul)
            : this(name)
        {
            this.IsModule = isModul;
        }
    }
}