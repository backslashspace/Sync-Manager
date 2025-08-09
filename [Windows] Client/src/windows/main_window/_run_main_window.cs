using BSS.Interop;
using System;
using System.Drawing;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;

internal static partial class MainWindow
{
    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial Boolean SetProcessDPIAware();

    /********************************************************************/

    internal static FontFamily? FontFamily;

    /********************************************************************/

    internal unsafe static void Run()
    {
        SetProcessDPIAware();

        Form mainForm = new()
        {
            AutoScaleMode = AutoScaleMode.Dpi,
            Text = "SyncMan Client (Nitro Edition)",
            Width = 600,
            Height = 450,
            StartPosition = FormStartPosition.CenterScreen,
            AllowTransparency = false,
            MaximizeBox = false,
            FormBorderStyle = FormBorderStyle.FixedSingle,
            ShowInTaskbar = true,
            Icon = Resources.MainIcon,
            HelpButton = false,
        };
        DwmApi.SetTheme(mainForm.Handle, true);
        FontFamily = mainForm.Font.FontFamily;

        BuildMainWindow(mainForm);

        //AddControls(mainForm);
        //
        //if (!Settings.Initialized)
        //{
        //    MessageBox.Show("Failed to initialize Settings", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    return;
        //}

        
        Application.Run(mainForm);
    }
}