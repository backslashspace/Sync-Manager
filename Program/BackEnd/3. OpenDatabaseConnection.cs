using System;
using System.Data.SQLite;
using System.IO;

namespace SyncMan
{
    internal static partial class Backend
    {
        private static (Boolean success, SQLiteConnection databaseConnection) OpenDatabaseConnection(String databasePath)
        {
            SQLiteConnection databaseConnection;

            if (!File.Exists(databasePath))
            {
                UI.Dispatcher.Invoke(() => 
                {
                    MessageBox.MessageBox messageBox = new("Database error", $"Unable to find database in specified path:\n\n{databasePath}", MessageBox.MessageBox.Icons.Triangle_Exclamation_Mark, "Ok");
                    messageBox.ShowDialog();
                });

                return (false, null);
            }

            if ((databaseConnection = CreateDatabaseConnection(ref databasePath)) == null)
            {
                UI.Dispatcher.Invoke(() =>
                {
                    MessageBox.MessageBox messageBox = new("Database error", $"Unable to open database in specified path:\n\n{databasePath}", MessageBox.MessageBox.Icons.Triangle_Exclamation_Mark, "Ok");
                    messageBox.ShowDialog();
                });

                return (false, null);
            }

            return (true, databaseConnection);
        }

        private static SQLiteConnection CreateDatabaseConnection(ref readonly String databasePath)
        {
            SQLiteConnection databaseConnection = new($"Data Source={databasePath}; Version=3;");

            try
            {
                databaseConnection.Open();
            }
            catch { }

            return databaseConnection;
        }
    }
}