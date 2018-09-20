
using System;
using System.Windows;

namespace MyWpfSysMenuApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Win Sys-Menu Mechanism
        // Import the Win32 dlls
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        /// Define Win32 constants
        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_SEPARATOR = 0x800;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 MF_STRING = 0x0;

        // Our own custom constant menu
        public const Int32 _CloseSysMenuID = 1000;
        public const Int32 _AboutSysMenuID = 1001;

        public IntPtr Handle
        {
            get
            {
                return new System.Windows.Interop.WindowInteropHelper(this).Handle;
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Check if a System Command has been executed
            if (msg == WM_SYSCOMMAND)
            {
                // Execute the appropriate code for the System Menu item that was clicked
                switch (wParam.ToInt32())
                {
                    case _CloseSysMenuID:
                        CloseTheApp();
                        handled = true;
                        break;
                    case _AboutSysMenuID:
                        About();
                        handled = true;
                        break;
                }
            }

            return IntPtr.Zero;
        }

        private void InitSysMenu()
        {
            /// Get the Handle for the Forms System Menu
            IntPtr systemMenuHandle = GetSystemMenu(Handle, false);

            /// Create our new System Menu items just before the Close menu item
            InsertMenu(systemMenuHandle, 5, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty); // <-- Add a menu seperator
            InsertMenu(systemMenuHandle, 6, MF_BYPOSITION, _AboutSysMenuID, "About");
            InsertMenu(systemMenuHandle, 7, MF_BYPOSITION, _CloseSysMenuID, "Close");

            // Attach our WndProc handler to this Window
            System.Windows.Interop.HwndSource source = System.Windows.Interop.HwndSource.FromHwnd(Handle);
            source.AddHook(new System.Windows.Interop.HwndSourceHook(WndProc));
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        /// <summary>
        /// About
        /// </summary>
        private void About()
        {
            MessageBox.Show("About MyWpfSysMenuApp");
        }
        
        /// <summary>
        /// CloseTheApp
        /// </summary>
        private void CloseTheApp()
        {
            Close();
        }
        
        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize the Sys-Menu Mechanism this is necessary at this point, 
            // e.g. because the window has not yet been created in the constructor
            InitSysMenu();
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        #endregion
    }
}
