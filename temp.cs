using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Data.SQLite;
using System.Data;

namespace SyncMan
{
    internal static class temp
    {
        internal static void SetTime(SQLiteConnection databaseConnection)
        {
            //SQLiteCommand command = databaseConnection.CreateCommand();
            //command.CommandText = $"INSERT INTO 'Transaction'(StartTimeAsFileTimeUTC, Machine, State) " +
            //    $"VALUES(@startTime, @machineGuid, @state)";
            //command.Parameters.Add("@startTime", DbType.Int64).Value = DateTime.Now.ToFileTimeUtc();
            //command.Parameters.Add("@machineGuid", DbType.Binary, 16).Value = State.MachineGuid.ToByteArray();
            //command.Parameters.Add("@state", DbType.Byte).Value = 2;

            //command.ExecuteNonQuery();
        }





        //command.CommandText = "INSERT INTO 'Transaction' VALUES (0, @Machine, NULL, 1)";
        //command.Parameters.Add("@Machine", DbType.Binary, 16).Value = ggg.ToByteArray();
        //command.ExecuteNonQuery();
    }
}






//command.CommandText = $"INSERT INTO 'Machine'(Guid, Alias) VALUES(@rawGuid, @alias)";
//command.Parameters.Add("@rawGuid", DbType.Binary, 16).Value = State.MachineGuid.ToByteArray();
//command.Parameters.Add("@alias", DbType.String).Value = State.Alias;




//private static DatabaseViews.Transaction GetDBState(SQLiteConnection databaseConnection)
//{
//    DatabaseViews.Transaction transaction = new();

//    SQLiteCommand command = databaseConnection.CreateCommand();
//    command.CommandText = "SELECT * FROM 'Transaction' DESC LIMIT 1";

//    SQLiteDataReader dataReader = command.ExecuteReader(CommandBehavior.KeyInfo);

//    // todo: fails when db empty?
//    dataReader.Read();

//    transaction.StartTimeFileTimeUTC = dataReader.GetInt64(0);

//    Byte[] rawGuid = new Byte[16];
//    dataReader.GetBytes(1, 0, rawGuid, 0, rawGuid.Length);
//    transaction.Machine = new(rawGuid);

//    transaction.EndTimeFileTimeUTC = dataReader.IsDBNull(2) ? null : dataReader.GetInt64(2);

//    transaction.Action = (DatabaseViews.Action)dataReader.GetByte(3);

//    return transaction;
//}