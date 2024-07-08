using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SyncMan
{
    public sealed partial class MainWindow
    {
        private static async Task ObtainConfiguration()
        {
            State.MachineGuid = await ObtainGuid();
            ObtainAlias();

            // set ui alias
            await UI.MainWindow.Dispatcher.BeginInvoke(static () => UI.MainWindow.MachineName.Text += State.Alias == null ? "<not set>" : State.Alias);
        }

        // ###############################################

        private static void ObtainAlias()
        {
            try
            {
                State.Alias = (String)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sync Manager", "Alias", null);
            }
            catch { }
        }

        private static async Task<Guid> ObtainGuid()
        {
            Guid guid;

            Byte[] data = (Byte[])Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sync Manager", "Guid", null);

            if (data == null || data.Length < 16)
            {
                LogBox.Add("Generating new Guid...\n", Brushes.LightCyan);

                data = await Util.RandomDotOrg_Get_16_Bytes();
                
                if (data == null)
                {
                    guid = Guid.NewGuid();
                    data = guid.ToByteArray();

                    LogBox.Add($"Failed to generate new guid using random.org, using local fallback\nGuid: {guid}\n", Brushes.LightGoldenrodYellow);
                }
                else
                {
                    guid = new(data);

                    LogBox.Add($"Generated new guid using random.org\nGuid: {guid}\n", Brushes.LightGreen);
                }

                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sync Manager", "Guid", data, RegistryValueKind.Binary);
            }
            else
            {
                guid = new(data);

                LogBox.Add($"Loaded Guid: {guid}", Brushes.LightCyan);
            }

            return guid;
        }
    }
}