using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SyncMan
{
    internal static class Util
    {
        internal static async Task<Byte[]> RandomDotOrg_Get_16_Bytes()
        {
            try
            {
                HttpClientHandler httpHandler = new();
                httpHandler.Proxy = null;
                httpHandler.UseProxy = false;
                httpHandler.AutomaticDecompression = DecompressionMethods.GZip;

                HttpClient httpClient = new(httpHandler);

                StringWithQualityHeaderValue GZip = new("GZip");
                if (!httpClient.DefaultRequestHeaders.AcceptEncoding.Contains(GZip))
                {
                    httpClient.DefaultRequestHeaders.AcceptEncoding.Add(GZip);
                }

                httpClient.BaseAddress = new Uri("https://www.random.org");

                HttpResponseMessage responseMessage = await httpClient.GetAsync("/cgi-bin/randbyte?nbytes=16&format=d");
                responseMessage.EnsureSuccessStatusCode();

                String response = await responseMessage.Content.ReadAsStringAsync();
                String[] formattedResponse = response.Substring(0, 63).Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (formattedResponse.Length != 16)
                {
                    Trace.Fail("Received unexpected response from random.org, invalid length, api changed?");
                    throw new InvalidDataException();
                }

                Byte[] random = [
                Byte.Parse(formattedResponse[0]),
                Byte.Parse(formattedResponse[1]),
                Byte.Parse(formattedResponse[2]),
                Byte.Parse(formattedResponse[3]),
                Byte.Parse(formattedResponse[4]),
                Byte.Parse(formattedResponse[5]),
                Byte.Parse(formattedResponse[6]),
                Byte.Parse(formattedResponse[7]),
                Byte.Parse(formattedResponse[8]),
                Byte.Parse(formattedResponse[9]),
                Byte.Parse(formattedResponse[10]),
                Byte.Parse(formattedResponse[11]),
                Byte.Parse(formattedResponse[12]),
                Byte.Parse(formattedResponse[13]),
                Byte.Parse(formattedResponse[14]),
                Byte.Parse(formattedResponse[15])];

                return random;
            }
            catch
            {
                return null;
            }
        }

        internal static UInt32 RGB_To_COLORREF(UInt32 R, UInt32 G, UInt32 B)
        {
            return (B << 16) | (G << 8) | R;
        }

        internal static void GetAccentColors()
        {
            try
            {
                Object rawData = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Accent", "AccentPalette", null);

                if (rawData is Byte[] data && data.Length >= 32)
                {
                    State.AccentColor[0] = data[4];
                    State.AccentColor[1] = data[5];
                    State.AccentColor[2] = data[6];

                    return;
                }
            }
            catch { }

            State.AccentColor[0] = 0x4c;
            State.AccentColor[1] = 0xc2;
            State.AccentColor[2] = 0xff;
        }
    }
}