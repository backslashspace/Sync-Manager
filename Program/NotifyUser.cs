using System;

namespace SyncMan
{
    internal static class Message
    {
        internal static void NotifyUser(String title, String message)
        {
            UI.Dispatcher.Invoke(() =>
            {
                MessageBox.MessageBox messageBox = new(title, message, MessageBox.MessageBox.Icons.Circle_Error, "Ok");
                messageBox.ShowDialog();
            });
        }

        internal static void NotifyUser(String title, String message, MessageBox.MessageBox.Icons icon)
        {
            UI.Dispatcher.Invoke(() =>
            {
                MessageBox.MessageBox messageBox = new(title, message, icon, "Ok");
                messageBox.ShowDialog();
            });
        }

        // ###############################################################################################################################

        internal static Boolean YesNo(String title, String message)
        {
            Boolean _continue = false;

            UI.Dispatcher.Invoke(() =>
            {
                MessageBox.MessageBox messageBox = new(title, message, MessageBox.MessageBox.Icons.Circle_Question, "Continue", "Cancel");
                messageBox.ShowDialog();
                _continue = messageBox.Result == 1 ? true : false;
            });

            return _continue;
        }

        internal static Boolean YesNo(String title, String message, MessageBox.MessageBox.Icons icon)
        {
            Boolean _continue = false;

            UI.Dispatcher.Invoke(() =>
            {
                MessageBox.MessageBox messageBox = new(title, message, icon, "Continue", "Cancel");
                messageBox.ShowDialog();
                _continue = messageBox.Result == 1 ? true : false;
            });

            return _continue;
        }
    }
}