using System;
using System.Collections.Generic;
using System.Text;

namespace CoreModel.ViewModel
{
    public class MenuToDo
    {


        public string id { get; set; }

        public string text { get; set; }

        public string icon { get; set; }

        public string url { get; set; }

        public List<Menu> menus { get; set; }
    }
    public class Menu
    {
        public string id { get; set; }

        public string text { get; set; }

        public string icon { get; set; }

        public string url { get; set; }
    }
}
