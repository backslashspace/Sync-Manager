using System;
using System.Diagnostics;
using System.Windows.Forms;

internal static partial class MainWindow
{
#if DEBUG
    private static readonly Button _debugButton = new()
    {
        Text = "Enum",
        UseVisualStyleBackColor = true,
        FlatStyle = FlatStyle.System,
        Location = new(20, 20),
        Height = 32,
        Width = 72,
    };
#endif

    private static void BuildMainWindow(Form mainForm)
    {
        #if DEBUG
        _debugButton.Font = new(FontFamily!, 10);
        _debugButton.Click += Debug;
        mainForm.Controls.Add(_debugButton);
#endif
    }
}