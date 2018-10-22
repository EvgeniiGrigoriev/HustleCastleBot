using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HustleCastleSimulator.Helpers.Win32Helpers;

namespace HustleCastleSimulator.Helpers
{
    public class Window
    {
        private string _text = null;

        private int? _textLength = null;

        public IntPtr Handler { get; set; }

        public string Text
        {
            get
            {
                if(_text == null)
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
                if(!_textLength.HasValue)
                {
                    _textLength = (int)this.SendMessage(Constants.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
                }
                return _textLength.Value;
            }
        }

        public Window(IntPtr handler)
        {
            Handler = handler;
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

        public List<Window> FindAllWindows()
        {
            List<Window> allChildWindows = new List<Window>();

            allChildWindows.AddRange(new WindowHandleInfo(Handler).GetAllChildHandles());

            foreach (var childWindow in allChildWindows.ToList())
            {
                allChildWindows.AddRange(childWindow.FindAllWindows());         
            }

            return allChildWindows;
        }
    }
}
