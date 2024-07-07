using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SyncMan
{
    public partial class MainWindow
    {
        private static async Task ObtainConfiguration()
        {
            State.MachineGuid = await ObtainGuid();

            String alias = ObtainAlias();
            alias ??= "<not set>";
            await UI.MainWindow.Dispatcher.BeginInvoke(() => UI.MainWindow.MachineName.Text += alias);
        }

        // ###############################################

        private static String ObtainAlias()
        {
            String alias = null;

            try
            {
                alias = (String)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sync Manager", "Alias", null);
            }
            catch { }

            return alias;
        }

        private static async Task<Guid> ObtainGuid()
        {
            Guid guid;

            Byte[] data = (Byte[])Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sync Manager", "Guid", null);

            if (data == null || data.Length < 16)
            {
                LogBox.Add("Generating new Guid...\n", Brushes.LightCyan);

                try
                {
                    data = await Util.RandomDotOrg_Get_16_Bytes();
                    guid = new(data);

                    LogBox.Add($"Generated new guid using random.org\nGuid: {guid}\n", Brushes.LightGreen);
                }
                catch
                {
                    guid = new();

                    LogBox.Add("Failed to generate new guid using random.org, using local fallback\nGuid: " + guid, Brushes.LightGoldenrodYellow);
                }

                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sync Manager", "Guid", data, RegistryValueKind.Binary);
            }
            else
            {
                guid = new(data);

                LogBox.Add("Loaded Guid: " + guid, Brushes.LightCyan);
            }

            return guid;
        }
    }
}