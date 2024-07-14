﻿using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SQLite;

namespace SyncMan
{
    internal static partial class Backend
    {
        internal static Task Upload()
        {
            (Boolean success, Config.Parameter parameter) = GetConfig();
            if (!success) return Task.CompletedTask;

            (success, Boolean isRemotePath) = PrepareDatabaseRead(parameter);
            if (!success) return Task.CompletedTask;

            (success, SQLiteConnection databaseConnection) = OpenDatabaseConnection(parameter.DatabaseFilePath);
            if (!success) return Task.CompletedTask;

            success = EnsureRepoState(databaseConnection);
            if (!success) return Task.CompletedTask;






            /*
             * 
             * 
             * 
             * 
             * 
             * 
             */

            return Task.CompletedTask;
        }

        internal static void Download()
        {
            /*
             * 
             * 
             * 
             * 
             */

            return;
        }

        internal static void SetLocalAlias()
        {
            InputBox.InputBox messageBox = new("Machine Alias", "Global machine identifier Tag", "<name>", State.Alias, "Set");
            messageBox.ShowDialog();

            if (messageBox.Result == null)
            {
                return;
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sync Manager", "Alias", messageBox.Result, RegistryValueKind.String);
                LogBox.Add($"Set machine alias to: {messageBox.Result}\n");
            }
        }

        // ##########################################################################

        private static (Boolean success, Config.Parameter parameter) GetConfig()
        {
            Config.Parameter parameter = Config.ReadRunConfig();

            if (parameter.ExitStatus != Config.OperationStatus.Success)
            {
                String errorReason = GetErrorMessage(parameter);

                MessageBox.MessageBox messageBox = new("Error", "An error occurred whilst obtaining the run configuration:\n\n" +
                    $"{errorReason}\n\n" +
                    $"Please make sure the config file has the following format:\n\n" +
                    "DatabaseFilePath=<path>\n" +
                    "UploadCommand=<robocopy-cmd>\n" +
                    "DownloadCommand=<robocopy-cmd>\n\n" +
                    "Do you wish to extract a template config file in the current working directory?",
                    MessageBox.MessageBox.Icons.Gear,
                    "Yes", "No");

                messageBox.ShowDialog();

                if (messageBox.Result == 1)
                {
                    LogBox.Add("Extracting template", LogBox.LogLevel.Message);

                    Util.WriteTemplate();
                }

                return (false, parameter);
            }

            return (true, parameter);
        }

        private static String GetErrorMessage(Config.Parameter parameter)
        {
            if (parameter.ExitStatus == Config.OperationStatus.Error)
            {
                return "unable to read/find file";
            }

            if (parameter.DatabaseFilePath == null)
            {
                return "unable to parse database file path";
            }

            if (parameter.UploadCommand == null)
            {
                return "unable to parse robocopy command (upload)";
            }

            if (parameter.DownloadCommand == null)
            {
                return "unable to parse robocopy command (download)";
            }

            Trace.Fail("internal error, this should not happen");

            return null;
        }
    }
}