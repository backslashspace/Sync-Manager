using System;
using System.Data.SQLite;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Remoting.Messaging;
using static System.Net.Mime.MediaTypeNames;

namespace SyncMan
{
    internal static partial class Backend
    {
        internal static Boolean EnsureRepoState(SQLiteConnection databaseConnection)
        {
            SQLiteCommand command = databaseConnection.CreateCommand();
            

            Guid ggg = Guid.NewGuid();
            //
            //command.CommandText = "INSERT INTO 'Transaction' VALUES (0, @Machine, NULL, 1)";
            //command.Parameters.Add("@Machine", DbType.Binary, 16).Value = ggg.ToByteArray();
            //command.ExecuteNonQuery();
            //
            //
            command.CommandText = "SELECT * FROM 'Transaction' LIMIT 1";

            //return false;

            SQLiteDataReader dataReader = command.ExecuteReader(CommandBehavior.KeyInfo);

            dataReader.Read();

            dataReader.IsDBNull(2);

            Byte[] rawGuid = new Byte[16];

            Int64 startTime_FileTimeUTC = dataReader.GetInt64(0);
            dataReader.GetBytes(1, 0, rawGuid, 0, rawGuid.Length);

            Int64? endTime_FileTimeUTC;

            if (dataReader.IsDBNull(2))
            {
                endTime_FileTimeUTC = null;
            }
            else
            {
                endTime_FileTimeUTC = dataReader.GetInt64(2);
            }

            Int16 actionID = dataReader.GetInt16(3);

            return false;
        }

        
    }
}
