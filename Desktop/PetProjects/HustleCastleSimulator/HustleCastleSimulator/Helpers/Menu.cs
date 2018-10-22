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

        public IntPtr Handler { get; set; }

        public string Text
        {
            get
            {
                if (_text == null)
                {
                    var tmpText = new StringBuilder(TextLength + 1);

                    this.SendMessage(Constants.WM_GETTEXT, (IntPtr)tmpText.Capacity, tmpText);

                    _text = tmpText.ToString();
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
                    _textLength = (int)this.SendMessage(Constants.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
                }
                return _textLength.Value;
            }
        }

        public Menu(IntPtr handler)
        {
            this.Handler = handler;
        }

        public IntPtr SendMessage(UInt32 msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hwnd = Handler;
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.SendMessage(hwnd, msg, wParam, lParam);
            else
                return IntPtr.Zero;
        }

        public IntPtr SendMessage(UInt32 msg, IntPtr wParam, StringBuilder lParam)
        {
            IntPtr hwnd = Handler;
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.SendMessage(hwnd, msg, wParam, lParam);
            else
                return IntPtr.Zero;
        }

        public bool PostMessage(UInt32 msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hwnd = Handler;
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.PostMessage(hwnd, msg, wParam, lParam);
            else
                return false;
        }

        public bool PostThreadMessage(UInt32 msg, IntPtr wParam, IntPtr lParam, bool ensureTargetThreadHasWindow = true)
        {
            uint targetThreadId = 0;
            IntPtr hwnd = Handler;
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

        public List<Menu> FindAllMenuItems()
        {
            List<Menu> subMenus = new List<Menu>();

            int menuItemCount = Win32Helpers.GetMenuItemCount(Handler);

            IntPtr subMenu = Win32Helpers.GetSubMenu(Handler, 1);

            bool result = Win32Helpers.GetMenuInfo(Handler, out MENUINFO menuInfo);

            var tmp = new Menu(Win32Helpers.GetMenuItemID(Handler, 0));

            var tmp2 = tmp.Text;

            return subMenus;
        }
    }
}
