using System;
using System.Runtime.InteropServices;

namespace Pixelmaniac.Notifications
{
    internal static class NativeMethods
    {
        internal const uint WM_SETICON = 0x0080;
        internal const int GWL_EXSTYLE = -20;
        internal const int WS_EX_DLGMODALFRAME = 0x0001;
        internal const int WS_EX_TOOLWINDOW = 0x00000080;
        internal const int SWP_NOSIZE = 0x0001;
        internal const int SWP_NOMOVE = 0x0002;
        internal const int SWP_NOZORDER = 0x0004;
        internal const int SWP_FRAMECHANGED = 0x0020;

        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
    }
}
