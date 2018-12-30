using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using log4net;

namespace Pinger.Pingers
{
    public static class PingerHelper
    {
        private static readonly ILog _log = LogManager.GetLogger("LOG");

        public static bool PingByICMP(string url, string name = null)
        {
            var nameWithUrl = name == null
                ? url
                : $"\"{name}\" ({url})";

            _log.Debug($"Pinging {nameWithUrl}...");

            using (var ping = new Ping())
            {
                bool result;

                try
                {
                    var pingReply = ping.Send(url);
                    result = pingReply?.Status == IPStatus.Success;
                }
                catch (Exception ex)
                {
                    _log.Warn($"Ping {nameWithUrl} caused an exception:\n", ex);
                    result = false;
                }

                LogResult(RequestType.Ping, result, nameWithUrl);

                return result;
            }
        }

        public static bool DoHttpWebRequest(string url, out string response, string name = null)
        {
            var nameWithUrl = name == null
                ? url
                : $"{name} ({url})";

            _log.Debug($"Requesting {nameWithUrl}...");

            bool result;
            response = null;
            try
            {
                var uri = new Uri(url);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Timeout = 3000;
                request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                request.Method = "HEAD";

                using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (var dataStream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(dataStream))
                        {
                            response = reader.ReadToEnd();
                            reader.Close();
                        }

                        dataStream.Close();
                    }
                }

                result = true;
            }
            catch (WebException ex)
            {
                _log.Warn($"Requesting {nameWithUrl} caused an exception:\n", ex);

                response = null;
                result = false;
            }

            LogResult(RequestType.HttpRequest, result, nameWithUrl, response);

            return result;
        }

        public static bool WebClientRequest(string url, out string response, string name = null)
        {
            var nameWithUrl = name == null
                ? url
                : $"{name} ({url})";

            _log.Debug($"Requesting {nameWithUrl}...");

            bool result;
            response = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    response = client.DownloadString(url);
                }

                result = true;
            }
            catch (WebException ex)
            {
                _log.Warn($"Requesting {nameWithUrl} caused an exception:\n", ex);

                response = null;
                result = false;
            }

            LogResult(RequestType.WebClientRequest, result, nameWithUrl, response);

            return result;
        }

        private static void LogResult(RequestType requestType, bool result, string nameWithUrl, string response = null)
        {
            var resMsg = $"{requestType} {nameWithUrl} result: {(result ? "succeed" : "FAILED!")}" +
                         $"{(string.IsNullOrEmpty(response) ? "" : $"\nAdditional response: {response}")}";
            if (result)
            {
                _log.Info(resMsg);
            }
            else
            {
                _log.Error(resMsg);
            }
        }
    }
}