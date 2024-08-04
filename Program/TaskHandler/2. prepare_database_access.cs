using System;

namespace SyncMan
{
    internal static partial class Backend
    {
        private static ValueTuple<Boolean, Boolean> PrepareDatabaseAccess(Config.Parameter parameter)
        {
            Boolean isRemotePath;

            if (parameter.DatabaseFilePath.Length < 3)
            {
                Message.NotifyUser("Error", "Config error:\nInvalid database path length?\n(less than 3 chars)");
                return (false, false);
            }

            isRemotePath = IsRemotePath(parameter);

            if (isRemotePath)
            { 
                Util.Run("cmd.exe", $"/c net use \"{parameter.DatabasePath}\"", waitForExit:true);
            }

            return (true, isRemotePath);
        }

        // #######################################################################################################

        private static Boolean IsRemotePath(Config.Parameter parameter)
        {
            UInt16 i = 0;

            if (parameter.DatabaseFilePath[0] == '/' && parameter.DatabaseFilePath[1] == '/') ++i;
            if (parameter.UploadCommand[0] == '/' && parameter.UploadCommand[1] == '/') ++i;
            if (parameter.DownloadCommand[0] == '/' && parameter.DownloadCommand[1] == '/') ++i;

            return i > 0 ? true : false;
        }
    }
}