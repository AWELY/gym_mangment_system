using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace gym_mangment_system
{
    public sealed class WaslSendResult
    {
        public bool   Ok         { get; private set; }
        public string Detail     { get; private set; }
        public int    StatusCode { get; private set; }

        private WaslSendResult(bool ok, string detail, int statusCode)
        {
            Ok         = ok;
            Detail     = detail ?? "";
            StatusCode = statusCode;
        }

        public static WaslSendResult Success(string detail) =>
            new WaslSendResult(true, detail, 200);

        public static WaslSendResult Fail(string detail, int statusCode = 0) =>
            new WaslSendResult(false, detail, statusCode);
    }

    /// <summary>
    /// Normalizes phone numbers for the WASL API: digits only, with country code,
    /// and no leading '+', spaces or dashes (e.g. <c>218910137119</c>).
    /// </summary>
    public static class WaslPhone
    {
        public static string ToDigits(string raw, string defaultCountryDigits)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";

            string d = new string(Array.FindAll(raw.ToCharArray(), char.IsDigit));
            if (d.Length == 0) return "";

            string cc = new string(Array.FindAll((defaultCountryDigits ?? "218").ToCharArray(), char.IsDigit));
            if (string.IsNullOrEmpty(cc)) cc = "218";

            if (d.StartsWith("966", StringComparison.Ordinal)) return d;
            if (d.StartsWith("218", StringComparison.Ordinal)) return d;
            if (d.StartsWith(cc, StringComparison.Ordinal))     return d;

            if (d.StartsWith("0", StringComparison.Ordinal) && d.Length >= 2)
                return cc + d.Substring(1);

            if (d.Length == 9)
                return cc + d;

            return d;
        }
    }

    /// <summary>
    /// Sends WhatsApp messages via the WASL (Wasel) REST API (Bearer token).
    /// Text:     POST {base}/messages/send/text     { from, phone, body }
    /// Document: POST {base}/messages/send/document { from, phone, media, file_name, caption }
    /// </summary>
    public sealed class WaslWhatsAppClient : IDisposable
    {
        private static readonly HttpClient SharedHttp = CreateSharedHttp();

        private static HttpClient CreateSharedHttp()
        {
            var c = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return c;
        }

        public static string ReadSetting(string key, string fallback = "")
        {
            try
            {
                string v = ConfigurationManager.AppSettings[key];
                return string.IsNullOrWhiteSpace(v) ? fallback : v.Trim();
            }
            catch
            {
                return fallback;
            }
        }

        private static bool TryBuildContext(
            string baseUrl,
            string apiToken,
            string fromDigits,
            string phoneDigits,
            string endpointPath,
            out string sendUrl,
            out WaslSendResult failure)
        {
            sendUrl = null;
            failure = null;

            if (string.IsNullOrWhiteSpace(apiToken))
            {
                failure = WaslSendResult.Fail("لم يُضبط مفتاح WaslApiToken في App.config (إعدادات التطبيق).");
                return false;
            }

            if (string.IsNullOrWhiteSpace(fromDigits))
            {
                failure = WaslSendResult.Fail("رقم المرسل (WaslSenderPhone) غير مضبوط بصيغة أرقام مع مفتاح الدولة.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(phoneDigits))
            {
                failure = WaslSendResult.Fail("رقم المستلم غير صالح للإرسال عبر الـ API.");
                return false;
            }

            string root = (baseUrl ?? "").Trim().TrimEnd('/');
            if (string.IsNullOrEmpty(root))
            {
                failure = WaslSendResult.Fail("لم يُضبط WaslBaseUrl في App.config.");
                return false;
            }

            sendUrl = root + endpointPath;
            return true;
        }

        private async Task<WaslSendResult> PostJsonAsync(string apiToken, string sendUrl, string json)
        {
            using (var req = new HttpRequestMessage(HttpMethod.Post, sendUrl))
            {
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiToken.Trim());
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage resp;
                try
                {
                    resp = await SharedHttp.SendAsync(req).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return WaslSendResult.Fail("تعذر الاتصال بـ WASL:\n" + ex.Message);
                }

                string body = "";
                try { body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false); }
                catch { /* ignore */ }

                if (resp.IsSuccessStatusCode)
                    return WaslSendResult.Success(string.IsNullOrEmpty(body) ? resp.ReasonPhrase : body);

                return WaslSendResult.Fail(
                    (int)resp.StatusCode + " " + resp.ReasonPhrase + (string.IsNullOrEmpty(body) ? "" : "\n" + body),
                    (int)resp.StatusCode);
            }
        }

        /// <summary>POST /messages/send/text — plain text (max 4096 chars).</summary>
        public async Task<WaslSendResult> SendTextAsync(
            string baseUrl,
            string apiToken,
            string fromDigits,
            string phoneDigits,
            string message)
        {
            if (!TryBuildContext(baseUrl, apiToken, fromDigits, phoneDigits, "/messages/send/text", out string sendUrl, out WaslSendResult bad))
                return bad;

            var payload = new Dictionary<string, object>
            {
                ["from"]  = fromDigits,
                ["phone"] = phoneDigits,
                ["body"]  = message ?? ""
            };

            string json = new JavaScriptSerializer().Serialize(payload);
            return await PostJsonAsync(apiToken, sendUrl, json).ConfigureAwait(false);
        }

        /// <summary>
        /// POST /messages/send/document — sends a local file (PDF, Office, archive, ...).
        /// <c>media</c> is raw base64 with NO <c>data:</c> URI prefix.
        /// </summary>
        public async Task<WaslSendResult> SendDocumentFromFileAsync(
            string baseUrl,
            string apiToken,
            string fromDigits,
            string phoneDigits,
            string absoluteFilePath,
            string caption)
        {
            if (!TryBuildContext(baseUrl, apiToken, fromDigits, phoneDigits, "/messages/send/document", out string sendUrl, out WaslSendResult bad))
                return bad;

            if (string.IsNullOrWhiteSpace(absoluteFilePath) || !File.Exists(absoluteFilePath))
                return WaslSendResult.Fail("الملف غير موجود في المسار المحدد.");

            const long maxBytes = 48L * 1024 * 1024;
            long len;
            try { len = new FileInfo(absoluteFilePath).Length; }
            catch (Exception ex) { return WaslSendResult.Fail("تعذر قراءة معلومات الملف:\n" + ex.Message); }

            if (len > maxBytes)
                return WaslSendResult.Fail("حجم الملف كبير جداً (الحد الأقصى 48 ميجابايت).");

            string fileName = Path.GetFileName(absoluteFilePath.Trim());
            if (string.IsNullOrEmpty(fileName))
                fileName = "document.pdf";

            string pathCopy = absoluteFilePath;
            string json;
            try
            {
                json = await Task.Run(
                    () =>
                    {
                        byte[] raw = File.ReadAllBytes(pathCopy);
                        // The WASL/WuzApi server requires a data: URI (the public docs
                        // saying "raw base64, no prefix" are wrong). Prefix with the
                        // file's MIME type so it arrives as the correct document type.
                        string media = "data:" + MimeFromFileName(fileName) + ";base64," + Convert.ToBase64String(raw);
                        var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
                        var payload = new Dictionary<string, object>
                        {
                            ["from"]      = fromDigits,
                            ["phone"]     = phoneDigits,
                            ["media"]     = media,
                            ["file_name"] = fileName,
                            ["caption"]   = caption ?? ""
                        };
                        return serializer.Serialize(payload);
                    },
                    CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return WaslSendResult.Fail("تعذر إعداد الملف للإرسال:\n" + ex.Message);
            }

            return await PostJsonAsync(apiToken, sendUrl, json).ConfigureAwait(false);
        }

        /// <summary>Maps a file name extension to a MIME type for the data: URI.</summary>
        private static string MimeFromFileName(string fileName)
        {
            string ext = Path.GetExtension(fileName ?? "").TrimStart('.').ToLowerInvariant();
            switch (ext)
            {
                case "pdf":  return "application/pdf";
                case "doc":  return "application/msword";
                case "docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "xls":  return "application/vnd.ms-excel";
                case "xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "ppt":  return "application/vnd.ms-powerpoint";
                case "pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case "zip":  return "application/zip";
                case "rar":  return "application/vnd.rar";
                case "txt":  return "text/plain";
                case "csv":  return "text/csv";
                case "png":  return "image/png";
                case "jpg":
                case "jpeg": return "image/jpeg";
                default:     return "application/octet-stream";
            }
        }

        /// <summary>Per-instance dispose is a no-op; <see cref="HttpClient"/> is shared for connection reuse.</summary>
        public void Dispose() { }
    }
}
