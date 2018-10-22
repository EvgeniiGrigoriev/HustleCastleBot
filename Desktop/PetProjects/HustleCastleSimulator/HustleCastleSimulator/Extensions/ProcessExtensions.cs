using HustleCastleSimulator.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HustleCastleSimulator.Extensions
{    
    public static class ProcessExtensions
    {
        public static IEnumerable<IntPtr> WindowHandles(this Process process)
        {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in process.Threads)
                Win32Helpers.EnumThreadWindows((uint)thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            return handles;
        }

        public static IntPtr SendMessage(this Process p, out IntPtr hwnd, UInt32 msg, IntPtr wParam, IntPtr lParam)
        {
            hwnd = p.WindowHandles().FirstOrDefault();
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.SendMessage(hwnd, msg, wParam, lParam);
            else
                return IntPtr.Zero;
        }

        //Posts a message to the first enumerated window in the first enumerated thread with at least one window, and returns the handle of that window through the hwnd output parameter if such a window was enumerated.  If a window was enumerated, the return value is the return value of the PostMessage call, otherwise the return value is false.
        public static bool PostMessage(this Process p, out IntPtr hwnd, UInt32 msg, IntPtr wParam, IntPtr lParam)
        {
            hwnd = p.WindowHandles().FirstOrDefault();
            if (hwnd != IntPtr.Zero)
                return Win32Helpers.PostMessage(hwnd, msg, wParam, lParam);
            else
                return false;
        }

        //Posts a thread message to the first enumerated thread (when ensureTargetThreadHasWindow is false), or posts a thread message to the first enumerated thread with a window, unless no windows are found in which case the call fails.  If an appropriate thread was found, the return value is the return value of PostThreadMessage call, otherwise the return value is false.
        public static bool PostThreadMessage(this Process p, uint msg, IntPtr wParam, IntPtr lParam, bool ensureTargetThreadHasWindow = true)
        {
            uint targetThreadId = 0;
            if (ensureTargetThreadHasWindow)
            {
                IntPtr hwnd = p.WindowHandles().FirstOrDefault();
                uint processId = 0;
                if (hwnd != IntPtr.Zero)
                    targetThreadId = Win32Helpers.GetWindowThreadProcessId(hwnd, out processId);
            }
            else
            {
                targetThreadId = (uint)p.Threads[0].Id;
            }
            if (targetThreadId != 0)
                return Win32Helpers.PostThreadMessage(targetThreadId, msg, wParam, lParam);
            else
                return false;
        }

        public static IEnumerable<IntPtr> GetAllWindows(this Process p)
        {
            var handles = new List<IntPtr>();
            System.Threading.Thread.Sleep(100);
            foreach (ProcessThread thread in p.Threads)
                Win32Helpers.EnumThreadWindows((uint)thread.Id,
                    (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }
    }
}
