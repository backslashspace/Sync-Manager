using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InputBox
{
    public sealed partial class InputBox : Window
    {
        internal String Result = null;

        private readonly String BoxHint;

        internal InputBox(String title, String prompt, String boxHint, String defaultValue, String confirmButtonText)
        {
            InitializeComponent();

            Title = title;

            BoxHint = boxHint;

            if (defaultValue == null)
            {
                UserInputBox.Text = boxHint;
            }
            else
            {
                UserInputBox.Text = defaultValue;
                UserInputBox.Foreground = (SolidColorBrush)SyncMan.UI.MainWindow.FindResource("FontColor");
            }

            if (boxHint == null || boxHint == "")
            {
                UserInputBox.Foreground = (SolidColorBrush)SyncMan.UI.MainWindow.FindResource("FontColor");
                UserInputBox.IsKeyboardFocusedChanged -= TextBoxKeyboardFocusChanged;
            }

            Cofirm.IsEnabled = false;
            Cofirm.Content = confirmButtonText;

            UserInputBox.Focus();

            if (prompt == null)
            {
                Height = 166d;
            }
            else
            {
                UserPromt.Text = prompt;
                UInt16 lineCount = 0;

                for (Int16 i = 0; i < prompt.Length; ++i)
                {
                    if (prompt[i] == '\n')
                    {
                        if (++lineCount == 3) break; 
                    }
                }

                Height = lineCount switch
                {
                    0 => 199d,
                    1 => 213d,
                    2 => 228d,
                    3 => 242d,
                    _ => 242d
                };
            }

            Loaded += SetWindowTheme;
        }
     
        private void SetWindowTheme(object sender, RoutedEventArgs e)
        {
            try
            {
                SyncMan.DWMAPI.SetTheme(new System.Windows.Interop.WindowInteropHelper(this).Handle, true);
                SyncMan.DWMAPI.SetCaptionColor(new System.Windows.Interop.WindowInteropHelper(this).Handle, SyncMan.Util.RGB_To_COLORREF(0x20u, 0x20u, 0x20u));
            }
            catch { }

            SyncMan.ButtonAnimator.SecondaryButton.Hook(Cofirm);
            SyncMan.ButtonAnimator.SecondaryButton.Hook(Cancel);
        }

        //# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

        private void TextBoxKeyboardFocusChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (UserInputBox.IsKeyboardFocused)
            {
                if (UserInputBox.Text == BoxHint)
                {
                    UserInputBox.Foreground = (SolidColorBrush)SyncMan.UI.MainWindow.FindResource("FontColor");
                    UserInputBox.Text = "";
                }
            }
            else
            {
                if (UserInputBox.Text == "")
                {
                    UserInputBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cfcfcf"));
                    UserInputBox.Text = BoxHint;
                }
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UserInputBox.Text == BoxHint || UserInputBox.Text == "")
            {
                Cofirm.IsEnabled = false;
            }
            else
            {
                Cofirm.IsEnabled = true;
            }
        }

        //# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

        private void ConfirmButton(object sender, RoutedEventArgs e)
        {
            Result = UserInputBox.Text;

            Close();
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}