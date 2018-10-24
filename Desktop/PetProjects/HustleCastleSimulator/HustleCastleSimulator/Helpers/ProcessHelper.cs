using HustleCastleSimulator.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HustleCastleSimulator.Helpers
{
    public class ProcessHelper
    {
        private Process _process;
        private string _name;

        public ProcessHelper(string Name)
        {
            this._name = Name;
        }

        public void Start()
        {
            _process = Process.Start(_name);
        }

        public void Finish()
        {
            System.Threading.Thread.Sleep(500);

            _process.CloseMainWindow();

            _process.Close();            
        }

        public void MinimizeWindow()
        {
            var result = _process.SendMessage(out IntPtr hwnd, Constants.WM_SYSCOMMAND, (IntPtr)Constants.SC_MINIMIZE, (IntPtr)0);

            Console.WriteLine(result);
        }

        public void RestoreWindow()
        {
            var result = _process.SendMessage(out IntPtr hwnd, Constants.WM_SYSCOMMAND, (IntPtr)Constants.SC_RESTORE, (IntPtr)0);

            Console.WriteLine(result);
        }

        public List<Window> FindAllWindows(IntPtr handler = default(IntPtr))
        {
            List<IntPtr> hwnds = new List<IntPtr>();

            IntPtr mainWindowHwnd;

            if (handler == IntPtr.Zero)
            {
                System.Threading.Thread.Sleep(200);
                mainWindowHwnd =_process.MainWindowHandle;
            }
            else
            {
                mainWindowHwnd = handler;
            }

            hwnds.Add(mainWindowHwnd);
            hwnds.AddRange(_process.GetAllWindows());

            List<Window> allChildWindows = new List<Window>();

            var menu = new Menu(IntPtr.Zero, Win32Helpers.GetMenu(mainWindowHwnd), mainWindowHandle: mainWindowHwnd);

            foreach (var childWindow in allChildWindows.ToList())
            {
                allChildWindows.AddRange(childWindow.FindAllWindows());
            }

            allChildWindows.OrderBy(w => (int)w.Handler);

            allChildWindows.RemoveAll(w => string.IsNullOrEmpty(w.Text));

            foreach (var window in allChildWindows)
            {
                Console.WriteLine(window.Text);
            }

            return allChildWindows;
        }
    }
}
