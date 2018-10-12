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

        public IntPtr FindWindowWithText()
        {
            var windowsHWNDS = _process.GetAllWindows();
            return IntPtr.Zero;
        }
    }
}
