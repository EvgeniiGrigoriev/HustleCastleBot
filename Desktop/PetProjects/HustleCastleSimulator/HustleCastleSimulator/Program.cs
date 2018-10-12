using HustleCastleSimulator.Helpers;
using System.Diagnostics;

namespace HustleCastleSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessHelper process = new ProcessHelper("notepad.exe");

            process.Start();

            process.FindWindowWithText();

            //process.Finish();
        }
    }
}
