using System;
using System.Data.SQLite;
using System.Data;

namespace SyncMan
{
    internal static partial class Backend
    {
        private const String MachineTableSchema = "CREATE TABLE \"Machine\" (\n\t\"ID\"\tINTEGER NOT NULL,\n\t\"Alias\"\tTEXT NOT NULL,\n\tPRIMARY KEY(\"ID\")\n)";
        private const String StateTableSchema = "CREATE TABLE \"State\" (\n\t\"ID\"\tINTEGER,\n\t\"Name\"\tTEXT NOT NULL,\n\tPRIMARY KEY(\"ID\")\n)";
        private const String VersionTableSchema = "CREATE TABLE \"Version\" (\n\t\"UpdateTimeAsFileTimeUTC\"\tINTEGER NOT NULL,\n\t\"Version\"\tTEXT NOT NULL,\n\t\"ChangeLog\"\tTEXT,\n\tPRIMARY KEY(\"UpdateTimeAsFileTimeUTC\")\n)";
        private const String TransactionTableSchema = "CREATE TABLE \"Transaction\" (\n\t\"StartTimeAsFileTimeUTC\"\tINTEGER,\n\t\"Machine\"\tINTEGER NOT NULL,\n\t\"EndTimeAsFileTimeUTC\"\tINTEGER,\n\t\"State\"\tINTEGER NOT NULL,\n\tFOREIGN KEY(\"Machine\") REFERENCES \"Machine\"(\"ID\"),\n\tFOREIGN KEY(\"State\") REFERENCES \"State\"(\"ID\"),\n\tPRIMARY KEY(\"StartTimeAsFileTimeUTC\")\n)";

        private static Boolean VerifyDatabase(SQLiteConnection databaseConnection)
        {
            DataTable tables = databaseConnection.GetSchema("Tables");
            UInt16 goodTablesCounter = 0;

            if (tables.Rows.Count < State.TargetTableCount)
            {
                Message.NotifyUser("Database error", "Invalid database file");
                return false;
            }

            for (UInt16 i = 0; i < tables.Rows.Count; ++i)
            {
                switch (tables.Rows[i].ItemArray[2])
                {
                    case "Machine":
                        if ((String)tables.Rows[i].ItemArray[6] == MachineTableSchema) ++goodTablesCounter;
                        break;

                    case "State":
                        if ((String)tables.Rows[i].ItemArray[6] == StateTableSchema) ++goodTablesCounter;
                        break;

                    case "Version":
                        if ((String)tables.Rows[i].ItemArray[6] == VersionTableSchema) ++goodTablesCounter;
                        break;

                    case "Transaction":
                        if ((String)tables.Rows[i].ItemArray[6] == TransactionTableSchema) ++goodTablesCounter;
                        break;
                }

                if (goodTablesCounter == State.TargetTableCount) return VerifyDatabaseVersion(databaseConnection);
            }

            Message.NotifyUser("Database error", "Invalid database file\nOne or more database schemas are invalid", MessageBox.MessageBox.Icons.Gear);
            return false;
        }

        private static Boolean VerifyDatabaseVersion(SQLiteConnection databaseConnection)
        {
            SQLiteCommand command = databaseConnection.CreateCommand();
            command.CommandText = "SELECT Version FROM Version ORDER BY UpdateTimeAsFileTimeUTC DESC LIMIT 1";
            SQLiteDataReader dataReader = command.ExecuteReader(CommandBehavior.SingleRow);

            if (!dataReader.Read())
            {
                Message.NotifyUser("Database error", "Unable to obtain database version, table empty?");
                return false;
            }

            String databaseVersion = dataReader.GetString(0);
            if (State.DatabaseVersion == databaseVersion) return true;

            Message.NotifyUser("Invalid database", $"Database does not have the required version\n\nRequired version: '{State.DatabaseVersion}'\nDatabase version: '{databaseVersion}'");
            return false;
        }
    }
}