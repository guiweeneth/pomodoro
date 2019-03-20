using System;
using System.Collections.Generic;
using System.Text;

namespace pomodoro_menu.Models
{
    public enum MenuItemType
    {
        Pomodoro,
        Browse,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
