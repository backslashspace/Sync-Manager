using System;
using System.Runtime.InteropServices;

namespace BSS.Interop
{
    internal static partial class DwmApi
    {
        internal static Boolean Initialized { get => _initialized; }
        private static Boolean _initialized = false;

        internal static Boolean Initialize()
        {
            if (_initialized) return false;

            Object regOutput = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CurrentBuildNumber", null)!;

            if (regOutput == null) return false;
            if (regOutput is not String) return false;
            if (!Int32.TryParse((String)regOutput, out _windowsBuildNumber)) return false;
            if (_windowsBuildNumber < 0) return false;

            if (_windowsBuildNumber >= 18985) // windows 10 '20H1' or newer
            {
                _dwmDarkModeWindowAttribute = DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE;
                _darkModeCompatibilityLevel = DWM_Dark_Mode_Compatibility_Level.IMMERSIVE_DARK_MODE;
            }
            else if (_windowsBuildNumber >= 17763)
            {
                _dwmDarkModeWindowAttribute = DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_18985_EQUAL_OR_AFTER_17763;
                _darkModeCompatibilityLevel = DWM_Dark_Mode_Compatibility_Level.IMMERSIVE_DARK_MODE_BEFORE_18985_EQUAL_OR_AFTER_17763;
            }
            else
            {
                _dwmDarkModeWindowAttribute = 0;
                _darkModeCompatibilityLevel = DWM_Dark_Mode_Compatibility_Level.NONE;
            }

            _initialized = true;
            return true;
        }

        /********************************************************************/

        [LibraryImport("dwmapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static unsafe partial UInt32 DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, UInt32* pvAttribute, UInt32 cbAttribute);

        /********************************************************************/

        /// <summary>
        /// Requires OS version 17763 or later  
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="darkMode"></param>
        /// <returns>HRESULT</returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal static unsafe UInt32 SetTheme(IntPtr hwnd, Boolean darkMode)
        {
            if (!_initialized) throw new Exception("DwmInterop not initialized.");

            UInt32 expandedBoolean = *(UInt32*)&darkMode;

            return DwmSetWindowAttribute(hwnd, _dwmDarkModeWindowAttribute, &expandedBoolean, sizeof(UInt32));
        }

        /// <summary>
        /// Requires OS version 22000 or later  
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="COLORREF"></param>
        /// <returns>HRESULT</returns>
        /// <exception cref="Exception"></exception>
        internal static unsafe UInt32 SetCaptionColor(IntPtr hwnd, UInt32 COLORREF)
        {
            if (!_initialized) throw new Exception("DwmInterop not initialized.");

            return DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR, &COLORREF, sizeof(UInt32));
        }

        /// <summary>
        /// Requires OS version 22000 or later 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="COLORREF"></param>
        /// <returns>HRESULT</returns>
        /// <exception cref="Exception"></exception>
        internal static unsafe UInt32 SetBorderColor(IntPtr hwnd, UInt32 COLORREF)
        {
            if (!_initialized) throw new Exception("DwmInterop not initialized.");

            return DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, &COLORREF, sizeof(UInt32));
        }

        /********************************************************************/

        internal static DWM_Dark_Mode_Compatibility_Level DarkModeCompatibilityLevel { get => _darkModeCompatibilityLevel; }
        private static DWM_Dark_Mode_Compatibility_Level _darkModeCompatibilityLevel;

        internal enum DWM_Dark_Mode_Compatibility_Level : Int32
        {
            NONE = 0,
            IMMERSIVE_DARK_MODE_BEFORE_18985_EQUAL_OR_AFTER_17763 = 1,
            IMMERSIVE_DARK_MODE = 2,
        }

        private static DWMWINDOWATTRIBUTE _dwmDarkModeWindowAttribute = 0;
        private enum DWMWINDOWATTRIBUTE : UInt32
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_18985_EQUAL_OR_AFTER_17763 = 19,
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_WINDOW_CORNER_PREFERENCE = 33,
            DWMWA_BORDER_COLOR = 34,
            DWMWA_CAPTION_COLOR = 35,
            DWMWA_TEXT_COLOR = 36
        }

        private static Int32 _windowsBuildNumber = -1;
    }
}