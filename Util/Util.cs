using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;

namespace SyncMan
{
    internal static class Util
    {
        internal static Byte[] RandomDotOrg_Get_16_Bytes()
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

            HttpResponseMessage responseMessage = httpClient.GetAsync("/cgi-bin/randbyte?nbytes=16&format=d").Result;
            responseMessage.EnsureSuccessStatusCode();

            String[] formattedResponse = responseMessage.Content.ReadAsStringAsync().Result.Substring(0, 63).Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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
    }
}