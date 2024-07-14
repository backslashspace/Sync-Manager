using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SyncMan
{
    internal static class Config
    {
        internal enum OperationStatus
        {
            Error = 0,
            Incomplete = 1,
            Success = 2
        }

        internal class Parameter
        {
            internal String DatabasePath = null;
            internal String DatabaseFilePath = null;
            internal String UploadCommand = null;
            internal String DownloadCommand = null;

            internal OperationStatus ExitStatus = OperationStatus.Error;
        }

        internal static Parameter ReadRunConfig()
        {
            Parameter parameter = new();
            List<String> lines = new();

            try
            {
                using (StreamReader streamReader = new("config.txt", Encoding.Default))
                {
                    String item;
                    while ((item = streamReader.ReadLine()) != null)
                    {
                        lines.Add(item);
                    }
                }
            }
            catch
            {
                return null;
            }

            String[] configLines = lines.ToArray();

            Match match;
            Byte numberOfAddedParameter = 0;

            for (UInt32 i = 0; i < configLines.Length; ++i)
            {
                if (parameter.DatabaseFilePath == null)
                {
                    if ((match = Regex.Match(configLines[i], "DatabaseFilePath=(.+?)$", RegexOptions.IgnoreCase)).Success)
                    {
                        parameter.DatabaseFilePath = match.Groups[1].Value.Trim();
                        parameter.DatabasePath = parameter.DatabaseFilePath.Substring(0, parameter.DatabaseFilePath.Length - State.DatabaseName.Length - 1);
                        ++numberOfAddedParameter;

                        continue;
                    }
                }

                if (parameter.UploadCommand == null)
                {
                    if ((match = Regex.Match(configLines[i], "UploadCommand=(.+?)$", RegexOptions.IgnoreCase)).Success)
                    {
                        parameter.UploadCommand = match.Groups[1].Value.Trim();
                        ++numberOfAddedParameter;
                        continue;
                    }
                }

                if (parameter.DownloadCommand == null)
                {
                    if ((match = Regex.Match(configLines[i], "DownloadCommand=(.+?)$", RegexOptions.IgnoreCase)).Success)
                    {
                        parameter.DownloadCommand = match.Groups[1].Value.Trim();
                        ++numberOfAddedParameter;
                        continue;
                    }
                }

                if (numberOfAddedParameter == 3) break;
            }

            if (numberOfAddedParameter == 3)
            {
                parameter.ExitStatus = OperationStatus.Success;
            }
            else
            {
                parameter.ExitStatus = OperationStatus.Incomplete;
            }

            return parameter;
        }
    }
}