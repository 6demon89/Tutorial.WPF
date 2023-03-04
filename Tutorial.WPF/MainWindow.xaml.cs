using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tutorial.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Track time for double click event
        /// </summary>
        readonly Timer timer = new Timer(450);

        /// <summary>
        /// Check if the user is double clicking the chrome
        /// </summary>
        bool FirstClickChrome = false;

        public MainWindow()
        {
            InitializeComponent();
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = false;
            SourceInitialized += Window_SourceInitialized;
            this.Loaded += (s, e) => this.Activate();
            DataContext = new ViewModel.MainWindowViewModel();


        }


        private void Change_Windows_State_Click(object sender, RoutedEventArgs e)
        {
            bool StateMaximized = WindowState == WindowState.Maximized;
            WindowState = StateMaximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        /// Draging window over left mouse button
        /// </summary>
        private void NavigationBar_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Application.Current.MainWindow.DragMove();
        }

        private void Minimize_Windows_State_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// checking double click to change the windows state
        /// </summary>
        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (FirstClickChrome)
            {
                timer.Stop();
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                FirstClickChrome = false;
            }
            else
            {
                timer.Start();
                FirstClickChrome = true;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FirstClickChrome = false;
        }



        /// <summary>
        /// After windows in initialized, we can get the windows handler pointer in order to manipulate it
        /// </summary>
        void Window_SourceInitialized(object sender, EventArgs e)
        {
            //We care getting the pointer of this window over hepler class in interop namespace
            IntPtr mWindowHandle = (new System.Windows.Interop.WindowInteropHelper(this)).Handle;

            //We are adding hook to this pointer, in order to extend/manipulate the behaviour
            System.Windows.Interop.HwndSource.FromHwnd(mWindowHandle)
                .AddHook(new System.Windows.Interop.HwndSourceHook(WindowProc));
        }

        /// <summary>
        /// Our hook will check the messaging system of the user32.dll to catch the request for window change event
        /// we are returning Zero pointer, since we are processing this message, 
        /// in order to release windows to do that for us
        /// </summary>
        private static System.IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                //Sent to a window when the size or position of the window is about to change.
                //An application can use this message to override the window's default maximized size and position,
                //or its default minimum or maximum tracking size.
                //https://docs.microsoft.com/en-us/windows/win32/winmsg/wm-getminmaxinfo
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    break;
            }

            return IntPtr.Zero;
        }


        /// <summary>
        /// Processing the window possition changes
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            POINT lMousePosition;
            GetCursorPos(out lMousePosition);

            IntPtr lPrimaryScreen = MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
            MONITORINFO lPrimaryScreenInfo = new MONITORINFO();

            // Checking pointers with recieved monitor information
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
            {
                return;
            }

            //Getting pointer of screen with the applicaiton window
            IntPtr lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);

            //Getting minmax information from winuser.h
            MINMAXINFO lMmi = (MINMAXINFO)System.Runtime.InteropServices.Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            //changing applicaiton window screen information based on primaryscreen new position
            if (lPrimaryScreen.Equals(lCurrentScreen) == true)
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcWork.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcWork.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top;
            }
            //changing applicaiton window screen information based on nearest screen new position
            else
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcMonitor.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcMonitor.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcMonitor.Right - lPrimaryScreenInfo.rcMonitor.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcMonitor.Bottom - lPrimaryScreenInfo.rcMonitor.Top;
            }
            System.Runtime.InteropServices.Marshal.StructureToPtr(lMmi, lParam, true);
        }

        /// <summary>
        /// Get Point of the current mouse position 
        /// </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);


        /// <summary>
        /// The MonitorFromPoint function retrieves a handle to the display monitor that contains a specified point.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-monitorfrompoint
        /// </summary>
        /// <returns>
        /// If the point is contained by a display monitor, the return value is an HMONITOR handle to that display monitor.
        /// If the point is not contained by a display monitor, the return value depends on the value of dwFlags.
        /// </returns>
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        /// <summary>
        /// Monitor option Flags
        /// </summary>
        enum MonitorOptions : uint
        {
            //Returns NULL.
            MONITOR_DEFAULTTONULL = 0x00000000,

            //Returns a handle to the primary display monitor.
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,

            //Returns a handle to the display monitor that is nearest to the point.
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);


        /// <summary>
        /// The MONITORINFO structure contains information about a display monitor.
        /// The GetMonitorInfo function stores information in a MONITORINFO structure or a MONITORINFOEX structure.
        /// The MONITORINFO structure is a subset of the MONITORINFOEX structure.The MONITORINFOEX structure adds a string member to contain a name for the display monitor.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                MessageBox.Show("Return pressed");
            if (e.Key == Key.Enter)
                MessageBox.Show("Enter pressed");
        }
    }
}
