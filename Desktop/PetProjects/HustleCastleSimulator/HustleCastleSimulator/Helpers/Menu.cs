using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HustleCastleSimulator.Helpers
{
    public class Menu
    {
        private string _text = null;

        private int? _textLength = null;

        private List<Menu> childMenus = null;

        public IntPtr MainWindowHandle { get; set; }

        public IntPtr ParentHandle { get; set; }

        public IntPtr Handle { get; set; }

        public List<Menu> ChildMenus
        {
            get
            {
                if(childMenus == null && (int)Handle != Constants.NullableHandler)
                {
                    childMenus = new List<Menu>();

                    var menuItemsCount = Win32Helpers.GetMenuItemCount(Handle);

                    for (int i = 0; i <= menuItemsCount; i++)
                    {
                        var subMenuHandler = Win32Helpers.GetSubMenu(Handle, i);

                        var menuItem = new Menu(Handle, subMenuHandler, MainWindowHandle);

                        if (menuItem.Handle != IntPtr.Zero && (int)menuItem.Handle != Constants.NullableHandler)
                        {
                            childMenus.Add(menuItem);
                        }
                    }
                }

                return childMenus;
            }
        }

        public string Text
        {
            get
            {
                if (_text == null)
                {
                    var tmpText = new StringBuilder(11);

                    Win32Helpers.GetMenuString(ParentHandle, Handle, tmpText, tmpText.Capacity, 1);

                    _text = tmpText.Replace("&", "").ToString();
                }
                return _text;
            }
        }

        public int TextLength
        {
            get
            {
                if (!_textLength.HasValue)
                {
                    _textLength = (int)SendMessage(Constants.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
                }
                return _textLength.Value;
            }
        }

        public Menu(IntPtr handler)
        {
            Handle = handler;
        }

        public Menu(IntPtr parentHandle, IntPtr handle, IntPtr mainWindowHandle = default(IntPtr))
        {
            ParentHandle = parentHandle;
            Handle = handle;
            MainWindowHandle = mainWindowHandle;
        }
        
        public IntPtr SendMessage(UInt32 msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hwnd = Handle;
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.SendMessage(hwnd, msg, wParam, lParam);
            else
                return IntPtr.Zero;
        }

        public IntPtr SendMessage(IntPtr hwnd, UInt32 msg, IntPtr wParam, IntPtr lParam)
        {
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.SendMessage(hwnd, msg, wParam, lParam);
            else
                return IntPtr.Zero;
        }

        public IntPtr SendMessage(UInt32 msg, IntPtr wParam, StringBuilder lParam)
        {
            IntPtr hwnd = Handle;
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.SendMessage(hwnd, msg, wParam, lParam);
            else
                return IntPtr.Zero;
        }

        public bool PostMessage(IntPtr hwnd, UInt32 msg, IntPtr wParam, IntPtr lParam)
        {
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.PostMessage(hwnd, msg, wParam, lParam);
            else
                return false;
        }

        public bool PostMessage(UInt32 msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hwnd = Handle;
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.PostMessage(hwnd, msg, wParam, lParam);
            else
                return false;
        }

        public bool PostThreadMessage(UInt32 msg, IntPtr wParam, IntPtr lParam, bool ensureTargetThreadHasWindow = true)
        {
            uint targetThreadId = 0;
            IntPtr hwnd = Handle;
            if (ensureTargetThreadHasWindow)
            {
                uint processId = 0;
                if (hwnd != IntPtr.Zero)
                    targetThreadId = Win32Helpers.GetWindowThreadProcessId(hwnd, out processId);
            }

            if (targetThreadId != 0)
                return Win32Helpers.PostThreadMessage(targetThreadId, msg, wParam, lParam);
            else
                return false;
        }

        public void LButtonMouseClick()
        {
            PostMessage(Handle, Constants.WM_COMMAND, IntPtr.Zero, Handle);
            //System.Threading.Thread.Sleep(300);
            //PostMessage(MainWindowHandle, Constants.WM_LBUTTONUP, Handle, IntPtr.Zero);
        }

        public List<Menu> FindAllMenuItems()
        {
            List<Menu> subMenus = new List<Menu>();

            int menuItemCount = Win32Helpers.GetMenuItemCount(Handle);

            for (int i = 0; i < 5; i++)
            {
                var subMenuHandler = Win32Helpers.GetSubMenu(Handle, i);
                subMenus.Add(new Menu(Handle, subMenuHandler, MainWindowHandle));
            }

            var lastHandler = IntPtr.Zero;
            for (int i = 0; lastHandler != IntPtr.Zero; i++)
            {
                var subMenuHandler = Win32Helpers.GetSubMenu(Handle, i);
                var menuItem = new Menu(Handle, subMenuHandler, MainWindowHandle);
                lastHandler = menuItem.Handle;
                if (lastHandler != IntPtr.Zero)
                {
                    subMenus.Add(menuItem);
                }
            }

            foreach (var subMenu in subMenus.ToList())
            {
                subMenus.AddRange(subMenu.GetAllSubMenus());
            }

            return subMenus;
        }

        public List<Menu> GetAllSubMenus()
        {
            List<Menu> subMenus = new List<Menu>();

            var menuItemsCount = Win32Helpers.GetMenuItemCount(Handle);

            if (Text.Contains("File"))
            {
                LButtonMouseClick();
            }

            for (int i = 0; subMenus.Count != menuItemsCount; i++)
            {
                var subMenuHandler = Win32Helpers.GetMenuItemID(Handle, i);

                var menuItem = new Menu(Handle, subMenuHandler, mainWindowHandle: MainWindowHandle);

                if (menuItem.Text.Contains("Paste"))
                {
                    menuItem.LButtonMouseClick();
                }

                if (menuItem.Handle != IntPtr.Zero)
                {
                    subMenus.Add(menuItem);
                }
            }

            return subMenus;
        }
    }
}
