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
            try
            {
                await ObtainGuid();

                String userAlias = (String)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Sync Manager", "MachineAlias", null);

                if (userAlias != null)
                {
                    State.MachineAlias.Alias = userAlias;
                    State.MachineAlias.IsUserDefined = true;
                }
            }
            catch (Exception ex)
            {
                Message.NotifyUser("Load error", $"An error occurred during initialization:\n\n{ex.Message}\n\n{ex.StackTrace}", MessageBox.MessageBox.Icons.Shield_Error);

                Environment.Exit(-1);
            }

            UI.MainWindow.Dispatcher.Invoke(static () => UI.MainWindow.MachineName.Text = State.MachineAlias.Alias);
        }

        // ###############################################

        private static async Task ObtainGuid()
        {
            Object data = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Sync Manager", "machineID", null);

            if (data != null && data.GetType() == typeof(Int64))
            {
                State.MachineID = (Int64)data;

                LogBox.Add($"Loaded Machine ID: {State.MachineID}\n", Brushes.LightCyan);
                return;
            }
            else
            {
                LogBox.Add("Generating new Machine ID...\n", Brushes.LightCyan);

                Byte[] randomBytes;
                if ((randomBytes = await Util.GetRandomBytes()) != null)
                {
                    State.MachineID = BitConverter.ToInt64(randomBytes, 0);
                    LogBox.Add($"Generated new machine id using random.org\nID: {State.MachineID}\n", Brushes.LightGreen);
                }
                else
                {
                    new Random().NextBytes(randomBytes);
                    State.MachineID = BitConverter.ToInt64(randomBytes, 0);
                    LogBox.Add($"Failed to generate new machine id using random.org, using local fallback\nID: {State.MachineID}\n", Brushes.LightGoldenrodYellow);
                }

                Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Sync Manager", "machineID", unchecked(State.MachineID), RegistryValueKind.QWord);

                return;
            }
        }
    }
}