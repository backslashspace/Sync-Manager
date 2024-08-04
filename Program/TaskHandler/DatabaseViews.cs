using System;
using System.Data;
using System.Data.SQLite;

namespace SyncMan.DatabaseViews
{
    internal enum State : Byte
    {
        Uploading = 0,
        Downloading = 1,

        Upload_Success = 2,
        Download_Success = 3,

        Upload_Failed = 4,
        Download_Failed = 5,

        Unknown = 255,
    }

    internal sealed class Transaction
    {
        /// <summary>Gets the last transaction</summary>
        internal Transaction(SQLiteConnection databaseConnection)
        {
            SQLiteCommand command = databaseConnection.CreateCommand();
            command.CommandText = "SELECT StartTimeAsFileTimeUTC, Machine, Alias, EndTimeAsFileTimeUTC, State, KeepAliveAsFileTimeUTC from 'Transaction' JOIN Machine ORDER BY StartTimeAsFileTimeUTC DESC LIMIT 1";
            SQLiteDataReader dataReader = command.ExecuteReader(CommandBehavior.SingleRow);

            if (!dataReader.Read()) return;

            StartTimeAsFormattedString = $"{DateTime.FromFileTimeUtc(dataReader.GetInt64(0)):G}";
            MachineID = dataReader.GetInt64(1);
            MachineName = dataReader.GetString(2);
            EndTimeAsFileTimeUTC = dataReader.IsDBNull(3) ? null : dataReader.GetInt64(3);
            EndTimeAsFormattedString = EndTimeAsFileTimeUTC == null ? "-- --" : $"{DateTime.FromFileTimeUtc((Int64)EndTimeAsFileTimeUTC): G}";
            Byte state = dataReader.GetByte(4);
            State = state < 6 ? (State)state : State.Unknown;
            KeepAliveAsFileTimeUTC = dataReader.GetInt64(5);
            KeepAliveAsFormattedString = $"{DateTime.FromFileTimeUtc(KeepAliveAsFileTimeUTC):G}";
        }

        /// <summary>Gets the last transaction from the specified Machine</summary>
        internal Transaction(SQLiteConnection databaseConnection, Int64 id)
        {
            SQLiteCommand command = databaseConnection.CreateCommand();
            command.CommandText = $"SELECT StartTimeAsFileTimeUTC, Machine, Alias, EndTimeAsFileTimeUTC, State, KeepAliveAsFileTimeUTC from 'Transaction' JOIN Machine WHERE Machine = {id} ORDER BY StartTimeAsFileTimeUTC DESC LIMIT 1";
            SQLiteDataReader dataReader = command.ExecuteReader(CommandBehavior.SingleRow);

            if (!dataReader.Read()) return;

            StartTimeAsFileTimeUTC = dataReader.GetInt64(0);
            StartTimeAsFormattedString = $"{DateTime.FromFileTimeUtc(StartTimeAsFileTimeUTC):G}";
            MachineID = dataReader.GetInt64(1);
            MachineName = dataReader.GetString(2);
            EndTimeAsFileTimeUTC = dataReader.IsDBNull(3) ? null : dataReader.GetInt64(3);
            EndTimeAsFormattedString = EndTimeAsFileTimeUTC == null ? "-- --" : $"{DateTime.FromFileTimeUtc((Int64)EndTimeAsFileTimeUTC): G}";
            Byte state = dataReader.GetByte(4);
            State = state < 6 ? (State)state : State.Unknown;
            KeepAliveAsFileTimeUTC = dataReader.GetInt64(5);
            KeepAliveAsFormattedString = $"{DateTime.FromFileTimeUtc(KeepAliveAsFileTimeUTC):G}";
        }

        internal readonly Int64 StartTimeAsFileTimeUTC;
        internal readonly String StartTimeAsFormattedString;
        internal readonly Int64 MachineID;
        internal readonly String MachineName;
        internal readonly Int64? EndTimeAsFileTimeUTC;
        internal readonly String EndTimeAsFormattedString;
        internal readonly State State;
        internal readonly Int64 KeepAliveAsFileTimeUTC;
        internal readonly String KeepAliveAsFormattedString;
    }   
}