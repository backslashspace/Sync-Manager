using System;
using System.Data.SQLite;

namespace SyncMan
{
    internal static partial class Backend
    {
        private static Boolean HandleLastTransactionState(SQLiteConnection databaseConnection, Action action)
        {
            DatabaseViews.Transaction lastTransaction = new(databaseConnection);
            DatabaseViews.Transaction lastTransactionFromThisMachine = new(databaseConnection, State.MachineID);

            // db empty
            if (lastTransaction.StartTimeAsFormattedString == null) return true;

            Boolean transactionsAreEqual = lastTransaction.StartTimeAsFormattedString == lastTransactionFromThisMachine.StartTimeAsFormattedString; // check primary keys
            Boolean lastTransactionWasSuccessful = lastTransaction.State == DatabaseViews.State.Download_Success || lastTransaction.State == DatabaseViews.State.Upload_Success;

            if (!lastTransactionWasSuccessful) HandleErrorState(databaseConnection, ref lastTransaction);

            if (action == Action.Upload)
            {
                if (lastTransaction.MachineID == State.MachineID && lastTransactionWasSuccessful) return true;

                return lastTransaction.State switch
                {
                    DatabaseViews.State.Download_Success => Message.YesNo("Warning", $"There might be newer data on {lastTransaction.MachineName}!\n" +
                    $"(Last action on {lastTransaction.MachineName} was DOWNLOAD)\n\n" +
                    $"Do you still wish to continue UPLOADING?\n" +
                    $"Machine id: {lastTransaction.MachineID}\n" +
                    $"Download start time: {lastTransaction.StartTimeAsFormattedString} and took {DateTime.FromFileTimeUtc((Int64)lastTransaction.EndTimeAsFileTimeUTC).Subtract(DateTime.FromFileTimeUtc(lastTransaction.StartTimeAsFileTimeUTC)):HH:mm}", MessageBox.MessageBox.Icons.Shield_Question),
                    
                    DatabaseViews.State.Upload_Success => Message.YesNo("Warning", $"There might be newer data in the remote location!\n" +
                    $"(Last action on {lastTransaction.MachineName} was UPLOAD)\n\n" +
                    $"Do you still wish to continue UPLOADING and OVERWRITE the remote location?\n" +
                    $"Machine id: {lastTransaction.MachineID}\n" +
                    $"Upload start time: {lastTransaction.StartTimeAsFormattedString} and took {DateTime.FromFileTimeUtc((Int64)lastTransaction.EndTimeAsFileTimeUTC).Subtract(DateTime.FromFileTimeUtc(lastTransaction.StartTimeAsFileTimeUTC)):HH:mm}", MessageBox.MessageBox.Icons.Shield_Question),
                    
                    DatabaseViews.State.Uploading => Message.YesNo("Warning", $"{lastTransaction.MachineName} is currently UPLOADING!\n" +
                    $"Do you still wish to continue UPLOADING?\n" +
                    $"Continuing might lead to data corruption!\n\n" +
                    $"Machine id: {lastTransaction.MachineID}\n" +
                    $"Upload start time: {lastTransaction.StartTimeAsFormattedString}\n" +
                    $"Last keep alive: {lastTransaction.KeepAliveAsFormattedString}", MessageBox.MessageBox.Icons.Shield_Error),
                    
                    DatabaseViews.State.Downloading => Message.YesNo("Warning", $"{lastTransaction.MachineName} is currently DOWNLOADING!\n" +
                    $"Do you still wish to continue UPLOADING?\n" +
                    $"Continuing might lead to data corruption!\n\n" +
                    $"Machine id: {lastTransaction.MachineID}\n" +
                    $"Download start time: {lastTransaction.StartTimeAsFormattedString}\n" +
                    $"Last keep alive: {lastTransaction.KeepAliveAsFormattedString}", MessageBox.MessageBox.Icons.Shield_Error),
                    
                    DatabaseViews.State.Upload_Failed => Message.YesNo("Incomplete transaction", $"Last UPLOAD from {lastTransaction.MachineName} FAILD!\n\n" +
                    $"Do you wish to continue UPLOADING?\n\n" +
                    $"Machine id: {lastTransaction.MachineID}\n" +
                    $"Upload start time: {lastTransaction.StartTimeAsFormattedString}\n" +
                    $"Upload abortion time: {lastTransaction.EndTimeAsFormattedString}", MessageBox.MessageBox.Icons.Gear),

                    DatabaseViews.State.Download_Failed => Message.YesNo("Incomplete transaction", $"Last DOWNLOAD from {lastTransaction.MachineName} FAILD!\n\n" +
                    $"Do you wish to continue UPLOADING?\n\n" +
                    $"Machine id: {lastTransaction.MachineID}\n" +
                    $"Download start time: {lastTransaction.StartTimeAsFormattedString}\n" +
                    $"Download abortion time: {lastTransaction.EndTimeAsFormattedString}", MessageBox.MessageBox.Icons.Gear),

                    _ => Message.YesNo("Unknown", $"The last transaction in the database contained invalid data\nDo you wish to continue UPLOADING?\n\nAdditional Information:\nUser Alias: {lastTransaction.MachineName}\nMachine id: {lastTransaction.MachineID}\nAction start time: {lastTransaction.StartTimeAsFormattedString}\nAction end time: {lastTransaction.EndTimeAsFormattedString}\nAction keep alive: {lastTransaction.KeepAliveAsFormattedString}", MessageBox.MessageBox.Icons.Admin_Shield)
                };
            }
            else
            {
                /ff
            }
        }

        private static void HandleErrorState(SQLiteConnection databaseConnection, ref DatabaseViews.Transaction lastTransaction)
        {
            if (!(lastTransaction.State == DatabaseViews.State.Uploading || lastTransaction.State == DatabaseViews.State.Downloading)) return;

            // log older than 5 min
            if (lastTransaction.KeepAliveAsFileTimeUTC + 3000000000 > DateTime.Now.ToFileTimeUtc()) return;

            Message.NotifyUser("Warning", "Unfinished transaction found:\n" +
                $"Last operation: {lastTransaction.State}\n" +
                $"From: {lastTransaction.MachineName} | ID: {lastTransaction.MachineID}\n" +
                $"Transaction start time: {lastTransaction.StartTimeAsFormattedString}\n\n" +
                $"Last transaction keep alive: {lastTransaction.KeepAliveAsFormattedString}\n" +
                "=> more than 5 minutes, should be less than 30 seconds.\n\n" +
                "Marking last transaction as FAILED.", MessageBox.MessageBox.Icons.Shield_Error);

            DatabaseViews.State newState = lastTransaction.State == DatabaseViews.State.Uploading ? DatabaseViews.State.Upload_Failed : DatabaseViews.State.Download_Failed;

            SQLiteCommand command = databaseConnection.CreateCommand();
            command.CommandText = $"UPDATE 'Transaction' SET EndTimeAsFileTimeUTC = {DateTime.Now.ToFileTimeUtc()}, State = {(Byte)newState} WHERE StartTimeAsFileTimeUTC = {lastTransaction.StartTimeAsFileTimeUTC}";
            command.ExecuteNonQuery();

            lastTransaction = new(databaseConnection);
        }
    }
}