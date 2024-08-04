using System;
using System.Data.SQLite;
using System.IO;

namespace SyncMan
{
    internal static partial class Backend
    {
        private static ValueTuple<Boolean, SQLiteConnection> OpenDatabaseConnection(String databasePath)
        {
            SQLiteConnection databaseConnection;

            if (!File.Exists(databasePath))
            {
                Message.NotifyUser("Config error", "$\"Unable to find database in specified path:\\n\\n{databasePath}\"", MessageBox.MessageBox.Icons.Triangle_Exclamation_Mark);
                return (false, null);
            }

            try
            {
                databaseConnection = new($"Data Source={databasePath}; Version=3; Foreign Keys=true;");
                databaseConnection.Open();
            }
            catch (Exception ex)
            {
                Message.NotifyUser("Internal error", $"Unable to open/load database\n\n{ex.Message}", MessageBox.MessageBox.Icons.Triangle_Exclamation_Mark);
                return (false, null);
            }

            return (true, databaseConnection);
        }
    }
}