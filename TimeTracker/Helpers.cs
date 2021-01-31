using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace TimeTracker
{
    public static class Helpers
    {
        public struct TaskbarDimenions
        {
            public int Top;
            public int Left;
            public int Width;
            public int Height;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public Int32 left; public Int32 top; public Int32 right; public Int32 bottom; }
        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA { public UInt32 cbSize; public IntPtr hWnd; public UInt32 uCallbackMessage; public UInt32 uEdge; public RECT rc; public Int32 lParam; }
        [DllImport("shell32.dll")]
        public static extern UInt32 SHAppBarMessage(UInt32 dwMessage, ref APPBARDATA pData);

        public static TaskbarDimenions GetTaskbarDimensions()
        {
            APPBARDATA msgData = new APPBARDATA();
            msgData.cbSize = (UInt32)Marshal.SizeOf(msgData);
            // get taskbar position
            SHAppBarMessage((UInt32)0x00000005, ref msgData);
            RECT taskRect = msgData.rc;

            return new TaskbarDimenions {
                Top = taskRect.top,
                Left = taskRect.left,
                Width = taskRect.right - taskRect.left, 
                Height = taskRect.bottom - taskRect.top,
            };
        }
    }
}
