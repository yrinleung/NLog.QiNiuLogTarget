using Newtonsoft.Json;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NLog.RestTarget
{
    [Target("QiNiuLogService")]
    public sealed class QiNiuLog : TargetWithLayout
    {
        public QiNiuLog()
        {
            QueueName = string.Empty;
            Authorization = string.Empty;
            Parameters = new List<LogParameterInfo>();
        }
        /// <summary>
        /// 队列名称
        /// </summary>
        [RequiredParameter]
        public string QueueName { get; private set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        [RequiredParameter]
        public string Authorization { get; private set; }

        /// <summary>
        /// 参数
        /// </summary>
        [ArrayParameter(typeof(LogParameterInfo), "parameter")]
        public IList<LogParameterInfo> Parameters { get; private set; }

        private LayoutWithHeaderAndFooter LHF
        {
            get => (LayoutWithHeaderAndFooter)base.Layout;
            set => base.Layout = value;
        }

        public override Layout Layout
        {
            get => LHF.Layout;

            set
            {
                if (value is LayoutWithHeaderAndFooter)
                {
                    base.Layout = value;
                }
                else if (LHF == null)
                {
                    LHF = new LayoutWithHeaderAndFooter()
                    {
                        Layout = value
                    };
                }
                else
                {
                    LHF.Layout = value;
                }
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            SendTheMessageToRemoteHost(logEvent);
        }

        private void SendTheMessageToRemoteHost(LogEventInfo logEvent)
        {
            var request = (HttpWebRequest)WebRequest.Create($"http://nb-pipeline.qiniuapi.com/v2/repos/{QueueName}/data");
            request.ContentType = "text/plain";

            request.Headers.Add("Authorization", "Pandora " + Authorization);
            request.Method = "POST";

            if (Parameters.Count == 0)
            {
                Parameters.Add(new LogParameterInfo() { Name = "message", Layout = "${message}" });
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Parameters)
            {
                stringBuilder.Append($"{item.Name}={item.Layout}\t");
            }
            var layout = Layout.FromString(stringBuilder.ToString());

            string message = RenderLogEvent(layout, logEvent);

            message = message.Replace("\r", "\\").Replace("\n", "\\");

            Stream stream = null;
            byte[] postData = Encoding.UTF8.GetBytes(message);
            request.ContentLength = postData.Length;
            stream = request.GetRequestStream();
            stream.Write(postData, 0, postData.Length);


            Task.Factory
                .StartNew(() =>
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        string rs = sr.ReadToEnd().Trim();
                        sr.Close();
                    }
                })
                .ContinueWith((t) =>
                {
                    throw new NLogRuntimeException(JsonConvert.SerializeObject(t));

                }, TaskContinuationOptions.OnlyOnFaulted);
        }


    }
    public class LogParameterInfo
    {
        public string Name { get; set; }

        public string Layout { get; set; }
    }
}
