using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HustleCastleSimulator.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MENUINFO
    {
        public UInt32 cbSize;
        public UInt32 fMask;
        public UInt32 dwStyle;
        public uint cyMax;
        public IntPtr hbrBack;
        public UInt32 dwContextHelpID;
        public UIntPtr dwMenuData;
    }
}
