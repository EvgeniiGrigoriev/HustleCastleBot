using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HustleCastleSimulator.Helpers
{
    public static class Constants
    {
        public static uint WM_SYSCOMMAND = 0x0112;

        public static int SC_MINIMIZE = 0xF020;
        public static int SC_RESTORE = 0xF120;

        public static uint WM_GETTEXT = 0x000D;
        public static uint WM_GETTEXTLENGTH = 0x000E;
    }
}
