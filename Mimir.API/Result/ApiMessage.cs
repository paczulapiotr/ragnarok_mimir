using Newtonsoft.Json;

namespace Mimir.API.Result
{
    public enum ApiMessageType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }

    public class ApiMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public ApiMessageType Type { get; set; }

        public ApiMessage(string text, ApiMessageType type = ApiMessageType.Info)
        {
            Text = text;
            Type = type;
        }

        public static ApiMessage Info(string text) => new ApiMessage(text, ApiMessageType.Info);
        public static ApiMessage Warning(string text) => new ApiMessage(text, ApiMessageType.Warning);
        public static ApiMessage Error(string text) => new ApiMessage(text, ApiMessageType.Error);

    }
}
