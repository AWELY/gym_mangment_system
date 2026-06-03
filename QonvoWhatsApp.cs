using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Threading;

namespace gym_mangment_system
{
    public sealed class QonvoSendResult
    {
        public bool   Ok          { get; private set; }
        public string Detail      { get; private set; }
        public int    StatusCode { get; private set; }

        private QonvoSendResult(bool ok, string detail, int statusCode)
        {
            Ok          = ok;
            Detail      = detail ?? "";
            StatusCode  = statusCode;
        }

        public static QonvoSendResult Success(string detail) =>
            new QonvoSendResult(true, detail, 200);

        public static QonvoSendResult Fail(string detail, int statusCode = 0) =>
            new QonvoSendResult(false, detail, statusCode);
    }

    /// <summary>
    /// Builds E.164-style numbers with a leading + for the Qonvo API.
    /// </summary>
    public static class QonvoPhone
    {
        public static string ToE164(string raw, string defaultCountryDigits)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";

            string d = new string(Array.FindAll(raw.ToCharArray(), char.IsDigit));
            if (d.Length == 0) return "";

            string cc = (defaultCountryDigits ?? "218").TrimStart('+');
            if (d.StartsWith("966", StringComparison.Ordinal)) return "+" + d;
            if (d.StartsWith("218", StringComparison.Ordinal)) return "+" + d;
            if (d.StartsWith(cc, StringComparison.Ordinal)) return "+" + d;

            if (d.StartsWith("0", StringComparison.Ordinal) && d.Length >= 2)
                return "+" + cc + d.Substring(1);

            if (d.Length == 9)
                return "+" + cc + d;

            return "+" + d;
        }
    }

    /// <summary>
    /// Sends WhatsApp messages via Qonvo REST API (Bearer token).
    /// </summary>
    public sealed class QonvoWhatsAppClient : IDisposable
    {
        private static readonly HttpClient SharedHttp = CreateSharedHttp();

        private static HttpClient CreateSharedHttp()
        {
            var c = new HttpClient();
            c.Timeout = TimeSpan.FromMinutes(10);
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

        private static bool TryGetSendEndpoint(
            string baseUrl,
            string apiToken,
            string senderPhoneE164,
            string recipientPhoneE164,
            out string sendUrl,
            out QonvoSendResult failure)
        {
            sendUrl = null;
            failure = null;

            if (string.IsNullOrWhiteSpace(apiToken))
            {
                failure = QonvoSendResult.Fail("لم يُضبط مفتاح QonvoApiToken في App.config (إعدادات التطبيق).");
                return false;
            }

            if (string.IsNullOrWhiteSpace(senderPhoneE164) || !senderPhoneE164.StartsWith("+", StringComparison.Ordinal))
            {
                failure = QonvoSendResult.Fail("رقم المرسل (QonvoSenderWhatsAppPhone) يجب أن يكون بصيغة دولية تبدأ بـ +.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(recipientPhoneE164) || !recipientPhoneE164.StartsWith("+", StringComparison.Ordinal))
            {
                failure = QonvoSendResult.Fail("رقم المستلم غير صالح للإرسال عبر الـ API.");
                return false;
            }

            string root = (baseUrl ?? "").Trim().TrimEnd('/');
            if (string.IsNullOrEmpty(root))
            {
                failure = QonvoSendResult.Fail("لم يُضبط QonvoBaseUrl في App.config.");
                return false;
            }

            sendUrl = root + "/whatsapp/messages/send";
            return true;
        }

        private async Task<QonvoSendResult> PostSendJsonAsync(string apiToken, string sendUrl, string json)
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
                    return QonvoSendResult.Fail("تعذر الاتصال بـ Qonvo:\n" + ex.Message);
                }

                string body = "";
                try { body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false); }
                catch { /* ignore */ }

                if (resp.IsSuccessStatusCode)
                    return QonvoSendResult.Success(string.IsNullOrEmpty(body) ? resp.ReasonPhrase : body);

                return QonvoSendResult.Fail(
                    (int)resp.StatusCode + " " + resp.ReasonPhrase + (string.IsNullOrEmpty(body) ? "" : "\n" + body),
                    (int)resp.StatusCode);
            }
        }

        /// <summary>
        /// POST /whatsapp/messages/send — text message.
        /// </summary>
        public async Task<QonvoSendResult> SendTextAsync(
            string baseUrl,
            string apiToken,
            string senderPhoneE164,
            string recipientPhoneE164,
            string message)
        {
            if (!TryGetSendEndpoint(baseUrl, apiToken, senderPhoneE164, recipientPhoneE164, out string sendUrl, out QonvoSendResult bad))
                return bad;

            var payload = new
            {
                sender_whatsapp_number_phone = senderPhoneE164,
                recipient_number             = recipientPhoneE164,
                type                         = "text",
                message                      = message ?? ""
            };

            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(payload);
            return await PostSendJsonAsync(apiToken, sendUrl, json).ConfigureAwait(false);
        }

        /// <summary>
        /// POST /whatsapp/messages/send — document from a local file.
        /// <c>media_base64</c> uses a data URI; <c>media_mimetype</c> must be an allowed value (e.g. <c>application/pdf</c> for PDFs).
        /// </summary>
        public async Task<QonvoSendResult> SendDocumentFromFileAsync(
            string baseUrl,
            string apiToken,
            string senderPhoneE164,
            string recipientPhoneE164,
            string absoluteFilePath,
            string caption)
        {
            if (!TryGetSendEndpoint(baseUrl, apiToken, senderPhoneE164, recipientPhoneE164, out string sendUrl, out QonvoSendResult bad))
                return bad;

            if (string.IsNullOrWhiteSpace(absoluteFilePath) || !File.Exists(absoluteFilePath))
                return QonvoSendResult.Fail("ملف PDF غير موجود في المسار المحدد.");

            const long maxBytes = 48L * 1024 * 1024;
            long len;
            try { len = new FileInfo(absoluteFilePath).Length; }
            catch (Exception ex) { return QonvoSendResult.Fail("تعذر قراءة معلومات الملف:\n" + ex.Message); }

            if (len > maxBytes)
                return QonvoSendResult.Fail("حجم الملف كبير جداً (الحد الأقصى 48 ميجابايت).");

            string fileName = Path.GetFileName(absoluteFilePath.Trim());
            if (string.IsNullOrEmpty(fileName))
                fileName = "diet-plan.pdf";
            else if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                fileName += ".pdf";

            string pathCopy = absoluteFilePath;
            string json;
            try
            {
                json = await Task.Run(
                    () =>
                    {
                        byte[] raw = File.ReadAllBytes(pathCopy);
                        string b64 = Convert.ToBase64String(raw);
                        // Qonvo validates the document data URI against octet-stream,
                        // regardless of the real file type (sent via media_mimetype below).
                        string dataUriPrefix = "data:application/octet-stream;base64,";
                        string mediaBase64  = dataUriPrefix + b64;
                        var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
                        var payload = new Dictionary<string, object>
                        {
                            ["sender_whatsapp_number_phone"] = senderPhoneE164,
                            ["recipient_number"]             = recipientPhoneE164,
                            ["type"]                         = "document",
                            ["media_base64"]                 = mediaBase64,
                            ["media_filename"]               = fileName,
                            ["media_mimetype"]               = "application/pdf",
                            ["caption"]                      = caption ?? ""
                        };
                        return serializer.Serialize(payload);
                    },
                    CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return QonvoSendResult.Fail("تعذر إعداد الملف للإرسال:\n" + ex.Message);
            }

            return await PostSendJsonAsync(apiToken, sendUrl, json).ConfigureAwait(false);
        }

        /// <summary>Per-instance dispose is a no-op; <see cref="HttpClient"/> is shared for connection reuse.</summary>
        public void Dispose() { }
    }
}
